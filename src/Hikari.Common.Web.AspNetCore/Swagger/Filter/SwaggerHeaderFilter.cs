using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hikari.Common.Web.AspNetCore.Swagger.Filter
{
    /// <summary>
    /// SwaggerHeader拦截器
    /// </summary>
    internal class SwaggerHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// 应用
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<IOpenApiParameter>();
            var filterPipeline = context.MethodInfo.GetCustomAttributes(typeof(CaptchaAttribute), true).Any();
            if (filterPipeline)
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "X-Client-ID",
                    In = ParameterLocation.Header,
                    Description = "用于验证码验证",
                    Required = true
                });
            }
        }
    }
}