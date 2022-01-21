using FsLib.CreditCardUtils;
using FsLib.TuChuangUtils;
using Hikari.Common;
using Hikari.Common.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http;
using System.Text;
using Hikari.Common.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace XUnitTestProject1
{
    public class UnitTest2
    {
        private ITestOutputHelper _output;

        public UnitTest2(ITestOutputHelper output)
        {
            _output = output;
        }
        //public static IEnumerable<TestCaseData> InvalidCardNumbersData
        //{
        //    get
        //    {
        //        yield return new TestCaseData(null);
        //        yield return new TestCaseData("");
        //        yield return new TestCaseData(" ");
        //        yield return new TestCaseData("123");
        //        yield return new TestCaseData("abc");
        //        yield return new TestCaseData("123abc");
        //        yield return new TestCaseData("abc123");
        //        yield return new TestCaseData("1111 2222");
        //    }
        //}

        //[TestCaseSource("InvalidCardNumbersData")]
        //public void TestInvalidCardNumberFormat(string cardNumber)
        //{
        //    var result = CreditCardValidator.ValidCardNumber(cardNumber);
        //    Assert.IsTrue(result.CardNumberFormat == CardNumberFormat.Invalid_BadStringFormat);
        //}

        [Fact]
        public void TestLuhnMod10Failure()
        {
            CreditCardValidator creditCardValidator = new CreditCardValidator();
            var result = creditCardValidator.ValidCardNumber("9962514040073500");
            Assert.True(result.CardNumberFormat == CardNumberFormat.Invalid_LuhnFailure);
        }

        [Fact]
        public void TestLuhnMod10Success()
        {
            CreditCardValidator creditCardValidator = new CreditCardValidator();
            var result = creditCardValidator.ValidCardNumber("5371381647477037");
            Assert.True(result.CardNumberFormat == CardNumberFormat.Valid_LuhnOnly);
        }

        //[Test]
        //public void TestValidVisa()
        //{
        //    var result = CreditCardValidator.ValidCardNumber("4012888888881881");
        //    Assert.IsTrue(result.CardNumberFormat == CardNumberFormat.Valid_BINTest && result.CardTypes.Contains("Visa"));
        //}

        //[Test]
        //public void TestValidMasterCard()
        //{
        //    var result = CreditCardValidator.ValidCardNumber("5105105105105100");
        //    Assert.IsTrue(result.CardNumberFormat == CardNumberFormat.Valid_BINTest && result.CardTypes.Contains("MasterCard"));
        //}

        //public class WellsFargoBankValidator : ICardTypeValidator
        //{
        //    public string Name => "Wells Fargo Bank";
        //    public string RegEx => @"^(485246)\d{10}$";
        //}

        //[Test]
        //public void TestCardTypeValidator()
        //{
        //    var result = CreditCardValidator.ValidCardNumber("4852461030260066", new ICardTypeValidator[]
        //    {
        //        new WellsFargoBankValidator()
        //    });
        //    Assert.IsTrue(result.CardNumberFormat == CardNumberFormat.Valid_BINTest && result.CardTypes.Contains("Wells Fargo Bank"));
        //}
        [Fact]
        public void Test1()
        {
            TuChuangClient client = new TuChuangClient();
            var s = client.UploadFile("").GetAwaiter().GetResult();


            Assert.True(true);
        }

        [Fact]
        public void Test3()
        {

            DataTable dt = new DataTable("cart");
            DataColumn dc1 = new DataColumn("prizename");
            dt.Columns.Add(dc1);
            dc1 = new DataColumn("point");
            dt.Columns.Add(dc1);
            dc1 = new DataColumn("number");
            dt.Columns.Add(dc1);
            dc1 = new DataColumn("totalpoint");
            dt.Columns.Add(dc1);
            dc1 = new DataColumn("prizeid");
            dt.Columns.Add(dc1);
            //dt.Columns.Add(dc1);
            //dt.Columns.Add(dc2);
            //dt.Columns.Add(dc3);
            //dt.Columns.Add(dc4);
            //dt.Columns.Add(dc5);
            //以上代码完成了DataTable的构架，但是里面是没有任何数据的  
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["prizename"] = "娃娃";
                dr["point"] = 10;
                dr["number"] = 1;
                dr["totalpoint"] = 10;
                dr["prizeid"] = "001";
                dt.Rows.Add(dr);
            }

            var v = ExcelHelper.ExcelToDataTable("D:\\1.xlsx", "Sheet1", true);
            Assert.True(true);
        }
        private static readonly List<Task> _tasks = new List<Task>();
        [Fact]
        public async void Test4()
        {

            //clientHelper.SetCookies("BAIDUID=E341C17C697F3D4D84469375CBD70AA9:FG=1; BDUSS=DhJZTBTZGhudzhmUUpjfkQyZ1cwWmhzSEdNVUthbGFxb3dXcW9HMm9jQkxrdnhoSVFBQUFBJCQAAAAAAAAAAAEAAAB9~HIH0uy2yNSqAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEsF1WFLBdVhU;");
            //var headerItem = new Dictionary<string, string>()
            //{
            //    {"Content-Type", "text/html; charset=UTF-8"},
            //    {"Referer", "https://aiqicha.baidu.com"},
            //    {"Zx-Open-Url", "https://aiqicha.baidu.com"},
            //    {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:95.0) Gecko/20100101 Firefox/95.0"},
            //    {"X-Requested-With", "XMLHttpRequest"},
            //    //{"Sec-Fetch-User", "document"},
            //};
            //clientHelper.SetHeaderItem(headerItem);
            HttpClient clientHelper = new();
            ThreadPoolHelper.BeginThreadPool();
            for (int i = 0; i < 100000; i++)
            {
                new Thread(async (state) =>
                    {
                        var html = await clientHelper.GetAsync("https://www.aliwork.com/home/?spm=a1zvbc7.26271733.0.0.1aa72546k5yBNa");
                        System.Diagnostics.Debug.WriteLine(html);
                    }).Start(i);

            }

            ThreadPoolHelper.CheckThreadPool();

            Assert.True(true);
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns></returns>
        private long timeGen()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        public string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = null;
            request.Method = "GET";
            //request.CookieContainer = cookies ?? GetCookieContainer(url, cookie);
            request.ContentType = "text/html;charset=UTF-8";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream myResponseStream = response.GetResponseStream())
                {
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                    string retString = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();
                    return retString;
                }
            }
        }
    }
}