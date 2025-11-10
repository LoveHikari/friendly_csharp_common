using Hikari.Common.IO;
using RJCP.IO.Ports;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Hikari.Common.Collection;
using Hikari.Common.DateTimeExt;
using Hikari.Common.IO.FileDetector;
using Hikari.Common.Net.Http;
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
            var vv = new { a = 1, b = new {b1 = "1", b2 = "2"} };
            var v = vv.ToDictionary();
            

            Assert.True(true);
        }
        [Fact]
        public async void Test5()
        {
            using (SerialPortStream serialPort = new SerialPortStream("COM3"))
            {
                // 基础功能：打开串口、配置参数
                serialPort.Open();
                serialPort.BaudRate = 115200;
                serialPort.Parity = Parity.Odd;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;

                // 高级功能：数据接收事件处理
                serialPort.DataReceived += (sender, e) =>
                {
                    byte[] receivedData = new byte[serialPort.BytesToRead];
                    serialPort.Read(receivedData, 0, receivedData.Length);
                    Console.WriteLine($"Received: {Encoding.UTF8.GetString(receivedData)}");
                };

                // 发送数据
                byte[] dataToSend = Encoding.UTF8.GetBytes("Hello, Serial!");
                serialPort.Write(dataToSend, 0, dataToSend.Length);

                // 等待一段时间以接收数据
                Console.WriteLine("Waiting for data...");
                Console.ReadLine();
            }
            Assert.True(true);
        }
    }
}


public static class Extensions
{
    extension(IEnumerable<int> source)
    {
        public IEnumerable<int> WhereGreaterThan(int threshold)
            => source.Where(x => x > threshold);

        public bool IsEmpty
            => !source.Any();
    }
}

