using System;

namespace Hikari.Common.Web.AspNetCore.Swagger.Filter
{
    /// <summary>
    /// 验证码过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CaptchaAttribute : Attribute
    {

    }
}