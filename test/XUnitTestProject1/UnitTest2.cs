﻿using System;
using FsLib.CreditCardUtils;
using FsLib.TuChuangUtils;
using Hikari.Common.Net.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using Hikari.Common;
using ServiceStack.Text;
using Xunit;
using Xunit.Abstractions;

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
        public void Test2()
        {
            //System.Globalization.CultureInfo.CurrentCulture = new CultureInfo("zh-CN");
            // var mow = DateTime.Parse("1949-6-4");
            //System.Globalization.ChineseLunisolarCalendar cc = new System.Globalization.ChineseLunisolarCalendar();
            //var s1 = cc.IsLeapYear(mow.Year).ToString();//;False
            //var s2 = cc.GetLeapMonth(mow.Year).ToString();//;0，注意：表示所闰月份。如果返回5，表示闰4月。
            string url = "https://translate.google.cn/_/TranslateWebserverUi/data/batchexecute";
            Dictionary<string, object> p = new Dictionary<string, object>()
                {{"f.req", "[[[\"MkEWBc\",\"[[\\\"公司\\\",\\\"auto\\\",\\\"en\\\",true],[null]]\",null,\"generic\"]]]"}};

            Dictionary<string, string> h = new Dictionary<string, string>()
                {{"Content-Type", "application/x-www-form-urlencoded"}};

            HttpClientHelper helper = new HttpClientHelper();
            var  v = helper.PostAsync(url, p, "utf-8", h).Result;
            
            Regex regex = new Regex("\\\\\"(.+?)\\\\\"");
            var ms = regex.Matches(v);

            var v1 = ms[2].Groups[1].Value;

            Assert.True(true);
        }
        [Fact]
        public async void Test3()
        {
            var d = DateTime.Now;
            var v = d.ToUnixTime();
            var v1 = d.ToUnixTimeMsAlt();
            //var v2 = d.ToStableUniversalTime();
            var v3 = d.ToUnixTimeSeconds();

            var d1 = v3.FromUnixTimeSeconds();
            var d2 = v1.FromUnixTimeMs();
            var d3 = v1.FromUnixTimeMillisecondsToUtc();

            Assert.True(true);
        }
    }
}