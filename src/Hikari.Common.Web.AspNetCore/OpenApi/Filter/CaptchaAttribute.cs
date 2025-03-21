namespace Hikari.Common.Web.AspNetCore.OpenApi.Filter
{
    /// <summary>
    /// 验证码过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CaptchaAttribute : Attribute
    {

    }
}