﻿using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Hikari.Common.Web.AspNetCore.Swagger.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Hikari.Common.Web.AspNetCore.Swagger
{
    /// <summary>
    /// Swagger文档扩展类
    /// <see cref="IServiceCollection"/> 扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// 添加Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="openApiInfo"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerCustom(this IServiceCollection services, OpenApiInfo? openApiInfo = null)
        {
            services.AddApiVersioning(options =>
            {
                //options.ApiVersionReader = new MediaTypeApiVersionReader();
                var builder = new MediaTypeApiVersionReaderBuilder();

                options.ApiVersionReader = builder.Parameter("v")
                                                  .Include("application/json")
                                                  .Build();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            }).AddApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVVV";
                o.SubstituteApiVersionInUrl = true;
            });  //添加版本控制、激活媒体类型版本控制
            //注册SwaggerAPI文档服务
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, openApiInfo));
                }

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "请输入带有Bearer的Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                }); // 添加httpHeader参数
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                options.OperationFilter<SwaggerHeaderFilter>();
                //Set the comments path for the swagger json and ui.
                System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file, true);
                });
            });

            return services;
        }
        /// <summary>
        /// 添加Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerCustom(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            //启用Swagger
            app.UseSwagger();
            //启用SwaggerUI
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            return app;
        }

        /// <summary>
        /// 创建版本信息
        /// </summary>
        /// <param name="description">版本描述</param>
        /// <param name="openApiInfo">标题</param>
        /// <returns></returns>
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, OpenApiInfo? openApiInfo)
        {
            if (openApiInfo == null)
            {
                openApiInfo = new OpenApiInfo()
                {
                    Title = $"Sample API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
                    Contact = new OpenApiContact() { Name = "Bill Mei", Email = "bill.mei@somewhere.com" },
                    // TermsOfService = "Shareware",
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                };
            }
            else
            {
                openApiInfo.Title = openApiInfo.Title + " " + description.ApiVersion;
                openApiInfo.Version = description.ApiVersion.ToString();
            }

            if (description.IsDeprecated)
            {
                openApiInfo.Description += " This API version has been deprecated.";
            }

            return openApiInfo;
        }
    }
}