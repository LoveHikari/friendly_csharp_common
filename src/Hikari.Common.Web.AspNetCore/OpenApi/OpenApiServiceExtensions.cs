using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Hikari.Common.Web.AspNetCore.OpenApi.Filter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Hikari.Common.Web.AspNetCore.OpenApi
{
    /// <summary>
    /// OpenApi文档扩展类
    /// <see cref="IServiceCollection"/> 扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class OpenApiServiceExtensions
    {
        /// <summary>
        /// 添加OpenApi
        /// </summary>
        /// <param name="services"></param>
        /// <param name="openApiInfo"></param>
        /// <returns></returns>
        public static IServiceCollection AddOpenApiCustom(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, OpenApiInfo? openApiInfo = null)
        {
            services.AddApiVersioning(options =>
            {
                //options.ApiVersionReader = new MediaTypeApiVersionReader();
                //var builder = new MediaTypeApiVersionReaderBuilder();

                //options.ApiVersionReader = builder.Parameter("v")
                //                                  .Include("application/json")
                //                                  .Build();
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            }).AddApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });  //添加版本控制、激活媒体类型版本控制
            //注册SwaggerAPI文档服务
            services.AddOpenApi(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.AddDocumentTransformer((document, context, cancellationToken) =>
                    {
                        document.Info = CreateInfoForApiVersion(description, openApiInfo);
                        return Task.CompletedTask;
                    });
                }
                
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                options.AddDocumentTransformer<CustomHeaderTransformer>();
            });

            return services;
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