#if !NETSTANDARD2_0
using System.Collections.Generic;
using System.Web;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public class CookieHelper
    {
        private readonly HttpCookie _cookie;
        /// <summary>
        /// 默认过期时间为关闭浏览器
        /// <param name="cookieObj">cookie对象名称</param>
        /// </summary>
        public CookieHelper(string cookieObj)
        {
            this._cookie = new HttpCookie(cookieObj);//初使化并设置Cookie的名称
        }

        /// <summary>
        /// 构造函数，初始化时传入过期时间
        /// </summary>
        /// <param name="cookieObj">cookie对象名称</param>
        /// <param name="expires">过期时间</param>
        public CookieHelper(string cookieObj, DateTime expires)
        {
            this._cookie = new HttpCookie(cookieObj);//初使化并设置Cookie的名称
            _cookie.Expires = expires;
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        public void SetCookie(string cookiename, string cookievalue)
        {
            IDictionary<string, string> idi = new Dictionary<string, string>();
            idi.Add(cookiename, cookievalue);
            SetCookie(idi);
        }

        /// <summary>
        /// 添加一组Cookie
        /// </summary>
        /// <param name="idi">cookie键值对</param>
        public void SetCookie(IDictionary<string, string> idi)
        {
            foreach (KeyValuePair<string, string> pair in idi)
            {
                _cookie.Values.Add(pair.Key, pair.Value);
            }
            HttpContext.Current.Response.AppendCookie(_cookie);
        }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookies">cookie对象</param>
        /// <param name="cookiename">cookiename</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookies, string cookiename)
        {
            //获取客户端的Cookie对象
            HttpCookie cok = HttpContext.Current.Request.Cookies[cookies];
            string str = string.Empty;
            if (cok != null)
            {
                str = cok.Values[cookiename];
            }
            return str;
        }
        /// <summary>
        /// 获取cookie对象全部值
        /// </summary>
        /// <param name="cookies">cookie对象名称</param>
        /// <returns></returns>
        public static string GetCookieValues(string cookies)
        {
            //获取客户端的Cookie对象
            HttpCookie cok = HttpContext.Current.Request.Cookies[cookies];
            string str = string.Empty;
            if (cok != null)
            {
                str = cok.Value;
            }
            return str;
        }
        /// <summary>
        /// 获取cookie对象全部值,以键值对形式
        /// </summary>
        /// <param name="cookies">cookie对象名称</param>
        /// <returns></returns>
        public static IDictionary<string, string> GetCookieValueListDictionary(string cookies)
        {
            //获取客户端的Cookie对象
            HttpCookie cok = HttpContext.Current.Request.Cookies[cookies];
            IDictionary<string, string> idc = new Dictionary<string, string>();
            if (cok.HasKeys)
            {
                for (int i = 0; i < cok.Values.Count; i++)
                {
                    idc.Add(cok.Values.AllKeys[i], cok.Values[i]);
                }
                return idc;
            }
            else
            {
                idc.Add(cok.Name, cok.Value);
                return idc;
            }
        }

        /// <summary>
        /// 修改cookie
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        public void UpdateCookie(string cookiename, string cookievalue)
        {
            if (_cookie != null)
            {
                _cookie.Values[cookiename] = cookievalue;
                HttpContext.Current.Response.AppendCookie(_cookie);
            }
        }

        /// <summary>
        /// 删除cookie
        /// </summary>
        /// <param name="cookiename"></param>
        public void DeleteCookie(string cookiename)
        {
            if (_cookie != null)
            {
                _cookie.Values.Remove(cookiename);
                HttpContext.Current.Response.AppendCookie(_cookie);
            }
        }
        /// <summary>
        /// 删除全部cookie
        /// </summary>
        /// <param name="cookies">cookie对象名称</param>
        public static void DeleteCookies(string cookies)
        {
            HttpCookie cok = HttpContext.Current.Request.Cookies[cookies];
            if (cok != null)
            {
                TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                cok.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            }
        }
    }
}
#endif
