using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IPAddress ipA = IPAddress.Any;
            int port = 19731; //端口
            IPEndPoint point = new IPEndPoint(ipA, port);

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //socket绑定监听地址
            serverSocket.Bind(point);
            Console.WriteLine("Listen Success");
            //设置同时连接个数
            serverSocket.Listen(10);

            //利用线程后台执行监听,否则程序会假死
            Thread thread = new Thread(Listen);
            thread.IsBackground = true; //后台线程
            thread.Start(serverSocket);
        }

        /// <summary>
        /// 监听连接
        /// </summary>
        /// <param name="o"></param>
        private void Listen(object o)
        {
            var serverSocket = o as Socket;
            while (true)
            {
                //等待连接并且创建一个负责通讯的socket
                var send = serverSocket.Accept();

                //获取链接的IP地址
                var sendIpoint = send.RemoteEndPoint.ToString();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $" {sendIpoint} Connection");
                //开启一个新线程不停接收消息
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(send);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        private void Recive(object o)
        {
            var serverSocket = o as Socket;
            MarshalEndian endian = new MarshalEndian();
            while (true)
            {
                //获取发送过来的消息容器
                byte[] buffer = new byte[1024 * 1024 * 2];

                var effective = serverSocket?.Receive(buffer) ?? 0;

                //有效字节为0则跳过
                if (effective == 0)
                {
                    break;
                }

                var bs = endian.GetDcAppMess(buffer, effective);
                foreach (TSocketMessage message in bs)
                {
                    var str = Encoding.UTF8.GetString(message.MsgBuffer, 0, message.MsgBuffer.Length);
                    Console.WriteLine(str);

                }

            }
        }

        [Fact]
        public void Test2()
        {
            string ids = "";
            for (int i = 1; i < 69; i++)
            {
                string url = "http://ku.iszoc.com/index/search.html?is_find=&is_audio=&uid=&keyword=&page=" + i;
                string html = HttpHelper.GetHttpWebRequest(url);
                NSoup.Nodes.Document htmlDoc = NSoup.NSoupClient.Parse(html);
                var div = htmlDoc.Select(".layui-card .layui-card-body .layui-card");
                foreach (var element in div)
                {
                    string href = element.Select("a").Attr("href");
                    string s = Regex.Match(href, "\\d+").Groups[0].Value;
                    System.Diagnostics.Debug.WriteLine(s);
                    ids += s + ",";


                    //string cookie = "PHPSESSID=c9c44562ba90cb7d0f8f9164cee8e2d4; Hm_lvt_ea5e026ac2ed0205ce7a6417bbd1dcef=1569398402; Hm_lpvt_ea5e026ac2ed0205ce7a6417bbd1dcef=1569399569";
                    //Hashtable hashtable = new Hashtable()
                    //{
                    //    {"Content-Type", "application/x-www-form-urlencoded" }
                    //};
                    //var data = new Dictionary<string, object>()
                    //{
                    //    {"id", s },
                    //    {"type","yes" }
                    //};
                    //string html1 = HttpHelper.PostHttpWebRequest("http://ku.iszoc.com/index/sign.html", data, headerItem: hashtable, cookie: cookie);
                    ////System.Diagnostics.Debug.WriteLine(html1);
                }
            }

            ids = ids.TrimEnd(',');

            string cookie = "PHPSESSID=c9c44562ba90cb7d0f8f9164cee8e2d4; Hm_lvt_ea5e026ac2ed0205ce7a6417bbd1dcef=1569398402; Hm_lpvt_ea5e026ac2ed0205ce7a6417bbd1dcef=1569399569";
            Hashtable hashtable = new Hashtable()
            {
                {"Content-Type", "application/x-www-form-urlencoded" }
            };
            var data = new Dictionary<string, object>()
            {
                {"id", ids },
            };
            string html1 = HttpHelper.PostHttpWebRequest("http://ku.iszoc.com/user/Favorites/export.html", data, headerItem: hashtable, cookie: cookie);
            System.Diagnostics.Debug.WriteLine(html1);

        }
        [Fact]
        public void Test3()
        {
            // var q = QRCodeHelper.EncodeQrCode2("https://www.fczbl.vip/",20,3);

            //string url = "https://tutut.ml/tool/free_ssr";
            //Hashtable headerItem = new Hashtable();
            //var cookies = HttpHelper.GetHttpWebRequest(url, "utf-8", headerItem, cookies: default);
            //url = "https://tutut.ml/tool/api/free_ssr?page=1&limit=10";
            //string html = HttpHelper.GetHttpWebRequest(url, cookie: "__cfduid=dfe25745309f7c3c2116016a77e9ef8d81574224344; JSESSIONID=E0E81E4F56B0241C5CA809A3E209F106;");
            //var i = ImageHelper.GetImage("d:/下载.png");
            //var ss = QRCodeHelper.DecodeQrCode((Bitmap)i);

            //System.Diagnostics.Debug.WriteLine(ss);
            CookieContainer cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookieContainer;
            handler.UseCookies = true;

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                
                var uri = new Uri("https://tutut.ml/tool/free_ssr");
                var message = new HttpRequestMessage(HttpMethod.Get, uri);
                var v = client.SendAsync(message).Result;
                var cookies = cookieContainer.GetCookieHeader(uri);

                uri = new Uri("https://tutut.ml/tool/api/free_ssr?page=1&limit=10");
                message = new HttpRequestMessage(HttpMethod.Get, uri);
                message.Headers.Add("Cookie", cookies);
                var result = client.SendAsync(message).Result;

                var cc = result.Content.ReadAsStringAsync().Result;
            }
        }

        [Fact]
        public async void Test4()
        {
            var v = DateTime.Now.ToString("DD");
        }
    }
}
