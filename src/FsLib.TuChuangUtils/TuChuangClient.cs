using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Hikari.Common.Net.Http;

namespace FsLib.TuChuangUtils
{
    /// <summary>
    /// 图床客户端
    /// </summary>
    public class TuChuangClient
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">本地文件地址</param>
        /// <returns>文件地址</returns>
        public async Task<string> UploadFile(string filePath)
        {
            string url = "http://notice.chaoxing.com/pc/files/uploadNoticeFile";
            HttpClientHelper httpClient = new HttpClientHelper();
            IDictionary<string, object> param = new Dictionary<string, object>()
            {
                {"attrFile", filePath}
            };
            IDictionary<string, string> headerItem = new Dictionary<string, string>()
            {
                {"Content-Type", "multipart/form-data"}
            };
            httpClient.SetHeaderItem(headerItem);
            var result = await httpClient.PostAsync(url, param);
            var jd = System.Text.Json.JsonDocument.Parse(result);
            string imgUrl = jd.RootElement.GetProperty("url").GetString();

            return imgUrl;
        }
    }
}