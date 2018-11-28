using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Sockets;
using System.Text;
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
            int port = 19731; //�˿�
            IPEndPoint point = new IPEndPoint(ipA, port);

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //socket�󶨼�����ַ
            serverSocket.Bind(point);
            Console.WriteLine("Listen Success");
            //����ͬʱ���Ӹ���
            serverSocket.Listen(10);

            //�����̺߳�ִ̨�м���,�����������
            Thread thread = new Thread(Listen);
            thread.IsBackground = true; //��̨�߳�
            thread.Start(serverSocket);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="o"></param>
        private void Listen(object o)
        {
            var serverSocket = o as Socket;
            while (true)
            {
                //�ȴ����Ӳ��Ҵ���һ������ͨѶ��socket
                var send = serverSocket.Accept();

                //��ȡ���ӵ�IP��ַ
                var sendIpoint = send.RemoteEndPoint.ToString();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $" {sendIpoint} Connection");
                //����һ�����̲߳�ͣ������Ϣ
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(send);
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="o"></param>
        private void Recive(object o)
        {
            var serverSocket = o as Socket;
            MarshalEndian endian = new MarshalEndian();
            while (true)
            {
                //��ȡ���͹�������Ϣ����
                byte[] buffer = new byte[1024 * 1024 * 2];

                var effective = serverSocket?.Receive(buffer) ?? 0;

                //��Ч�ֽ�Ϊ0������
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
    }
}
