using System;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Numerics;
using System.Threading;
using Hikari.Common;
using Hikari.Common.Mathematics;
using Hikari.Common.Net.Http;
using Xunit;
using Xunit.Abstractions;
using Hikari.DbCore;

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


            Assert.True(true);
        }

        [Fact]
        public async void Test4()
        {
            //var v = ("18.88").ToInt32(0);
            //_output.WriteLine(b);


            //_output.WriteLine(ConvertHelper.ConvertBase("11110111", 2, 10));


            //BigInteger number = new BigInteger(i);
            //int baseValue = 2; // 目标进制

            //string result = ConvertToBase(number, baseValue);
            //_output.WriteLine(result);

            //double value = 0.75;
            //var fraction = new Fraction(value);
            //System.Diagnostics.Debug.WriteLine($"{value} as a fraction is: {fraction}");

            int num1 = -12;
            int num2 = 18;

            var lcm = MathHelper.GetLeastCommonMultiple(num1, num2);
            System.Diagnostics.Debug.WriteLine($"最小公倍数 LCM({num1}, {num2}) = {lcm}");
            var text = "string";
            var result = text[..^1];
            System.Diagnostics.Debug.WriteLine(text.RemoveRight(3));
            System.Diagnostics.Debug.WriteLine(text[..^3]);
            //value = 2.5;
            //fraction = new Fraction(value);
            //System.Diagnostics.Debug.WriteLine($"{value} as a fraction is: {fraction}");

            //value = 0.3333333333333333; // 近似 1/3
            //fraction = new Fraction(value);
            //System.Diagnostics.Debug.WriteLine($"{value} as a fraction is: {fraction}");
            Assert.True(true);
        }
        string ConvertToBase(BigInteger number, int baseValue)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            List<char> result = new List<char>();

            BigInteger baseValueBigInt = new BigInteger(baseValue);
            BigInteger remainder;

            while (number != 0)
            {
                remainder = BigInteger.Remainder(number, baseValueBigInt);
                number = BigInteger.Divide(number, baseValueBigInt);
                result.Insert(0, chars[(int)remainder]);
            }

            return new string(result.ToArray());
        }
    }
}