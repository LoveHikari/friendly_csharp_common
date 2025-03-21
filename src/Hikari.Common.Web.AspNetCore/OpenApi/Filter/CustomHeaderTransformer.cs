﻿using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Hikari.Common.Web.AspNetCore.OpenApi.Filter
{
    public sealed class CustomHeaderTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var apiDescriptions = ((Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescriptionGroup[])context.DescriptionGroups).SelectMany(x => x.Items);
            foreach (var pathItem in document.Paths)
            {
                var pathTemplate = pathItem.Key.TrimStart('/');

                foreach (var operation in pathItem.Value.Operations)
                {
                    var operationMethod = operation.Key.ToString().ToUpperInvariant();

                    // 获取对应的API描述
                    var apiDescription = apiDescriptions.FirstOrDefault(api =>
                        string.Equals(api.HttpMethod, operationMethod, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(api.RelativePath, pathTemplate, StringComparison.OrdinalIgnoreCase));

                    if (apiDescription?.ActionDescriptor?.EndpointMetadata != null)
                    {
                        var hasCaptchaAttribute = apiDescription.ActionDescriptor.EndpointMetadata
                            .Any(m => m.GetType() == typeof(CaptchaAttribute));

                        if (hasCaptchaAttribute)
                        {
                            operation.Value.Parameters ??= new List<OpenApiParameter>();

                            // 检查是否已经存在X-Client-ID header
                            var existingHeader = operation.Value.Parameters
                                .FirstOrDefault(p => p.Name == "X-Client-ID" && p.In == ParameterLocation.Header);

                            if (existingHeader == null)
                            {
                                // 添加自定义header
                                operation.Value.Parameters.Add(new OpenApiParameter
                                {
                                    Name = "X-Client-ID",
                                    In = ParameterLocation.Header,
                                    Required = true,
                                    Description = "客户端ID，用于验证码校验",
                                    Schema = new OpenApiSchema
                                    {
                                        Type = JsonSchemaType.String
                                    }
                                });
                            }
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}