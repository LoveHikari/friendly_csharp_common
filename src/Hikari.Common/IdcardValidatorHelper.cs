using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Hikari.Common.Net.Http;
using NSoup.Nodes;

namespace Hikari.Common
{
    /// <summary>
    /// 身份证验证工具类
    /// </summary>
    public class IdcardValidatorHelper
    {
        private readonly IDictionary<string, string> _addressCodeTable;
        /// <summary>
        /// 构造函数
        /// </summary>
        public IdcardValidatorHelper()
        {
            _addressCodeTable = GetAddressCode();
        }
        /// <summary>
        /// 获得地址代码
        /// </summary>
        private IDictionary<string, string> GetAddressCode()
        {
            string url = "http://www.mca.gov.cn/article/sj/xzqh/2020/2020/202003301019.html";
            HttpClientHelper httpClient = new HttpClientHelper();
            string html = httpClient.GetAsync(url).GetAwaiter().GetResult();
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(html);
            IDictionary<string, string> hashtable = new Dictionary<string, string>();
            var trs = doc.Select("tr[height=19]");
            foreach (Element tr in trs)
            {
                var tb = tr.Select("td");
                hashtable.Add(tb[1].Text(), tb[2].Text());
            }

            return hashtable;
        }
        /// <summary>
        /// 检查地址代码是否正确
        /// </summary>
        /// <param name="addressCode">前6位地址代码</param>
        /// <returns></returns>
        private bool IsValidAddressCode(string addressCode)
        {
            return _addressCodeTable.Keys.Contains(addressCode);
        }
        /// <summary>
        /// 判断日期是否有效
        /// </summary>
        /// <param name="inDate">生日部分</param>
        /// <returns></returns>
        private bool IsValidDate(string inDate)
        {
            string birth = inDate.Insert(6, "-").Insert(4, "-");

            return DateTime.TryParse(birth, out _);
        }
        /// <summary>
        /// 计算身份证的第十八位校验码
        /// </summary>
        /// <param name="cardId">身份证前17位</param>
        /// <returns></returns>
        private string SumPower(string cardId)
        {
            char[] cardIdArray = cardId.ToCharArray();
            int[] power = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //身份证第18位校检码
            String[] refNumber = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += power[i] * int.Parse(cardIdArray[i].ToString());
            }

            return refNumber[(sum % 11)];
        }

        /// <summary>
        /// 校验身份证第18位是否正确(只适合18位身份证)
        /// </summary>
        /// <param name="idNo">完整身份证号码</param>
        /// <returns></returns>
        private bool CheckIdNoLastNum(String idNo)
        {
            string power = SumPower(idNo.Remove(17));
            string lastNum = idNo.Substring(17, 1);

            return power.ToLower() == lastNum.ToLower();
        }
        /// <summary>
        /// 二代身份证正则表达式
        /// </summary>
        /// <param name="idNo">身份证号码</param>
        /// <returns></returns>
        private bool IsIdNoPattern(String idNo)
        {
            return Regex.IsMatch(idNo, "^[1-9]\\d{5}(18|19|([23]\\d))\\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\\d{3}[0-9Xx]$");
        }
        /// <summary>
        /// 二代身份证号码有效性校验
        /// </summary>
        /// <param name="idNo">身份证号码</param>
        /// <returns></returns>
        public bool IsValidIdNo(String idNo)
        {
            return IsIdNoPattern(idNo) && IsValidAddressCode(idNo.Substring(0, 6))
                                       && IsValidDate(idNo.Substring(6, 8)) && CheckIdNoLastNum(idNo);
        }
    }
}