using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using FsLib.CreditCardUtils;
using FsLib.TuChuangUtils;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

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
            DateTime dt = DateTime.Now; //DateTime.Parse("令和5-10-1");
            _output.WriteLine("今天是：" + dt.ToString("ggyy-MM-dd"));
            _output.WriteLine("今天是"  + dt.Year + "年的第" + dt.WeekOfYear() + "周");
            _output.WriteLine("今天是" + dt.Month + "月的第" + dt.WeekOfMonth() + "周");
            _output.WriteLine($"今天是第{dt.QuarterOfYear()}季度的第 {dt.WeekOfQuarter()} 周");
            _output.WriteLine($"今天是第{dt.QuarterOfYear()}季度的第 {dt.DayOfQuarter()} 天");
            _output.WriteLine($"今天是第{dt.QuarterOfYear()}季度的第 {dt.MonthOfQuarter()} 月");
            _output.WriteLine($"今天是第{dt.QuarterOfYear()}季度, 共 {dt.WeeksOfQuarter()} 周");
            _output.WriteLine($"今天是{dt.Month}月, 共 {dt.WeeksOfMonth()} 周");

            _output.WriteLine("今年共" + dt.WeeksOfYear() + "周");

            _output.WriteLine($"这周{DateTimeHelper.GetDateWeek(dt)}");
            _output.WriteLine($"{dt.Year}第{dt.WeekOfYear()}周{DateTimeHelper.GetDateWeek(dt.Year, dt.WeekOfYear())}");
            _output.WriteLine(dt.DayNameOfWeek());

            Assert.True(true);
        }

    }
}