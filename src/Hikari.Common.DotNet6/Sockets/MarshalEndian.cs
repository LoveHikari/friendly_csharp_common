using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hikari.Common.Sockets
{
    /// <summary>
    /// 消息包的封装和拆分
    /// 封包格式为：包头1包头2消息长度消息id消息内容
    /// </summary>
    public class MarshalEndian
    {
        //用于存储剩余未解析的字节数
        private List<byte> _lBuff;
        //默认是utf8的编码格式
        private readonly Encoding _encoding;

        //包头1
        private const Int16 T1 = 0x55;  //占2个字节
        //包头2
        private const Int16 T2 = 0xAA;  //占2个字节
        //字节数常量 两个包头4个字节，一个消息id4个字节，封装消息长度 long 8个字节
        private const long CONST_LENGHT = 12L;  //包头长度，包头=包头1+包头2+包体长度

        public MarshalEndian(string encoding = "utf-8")
        {
            _lBuff ??= new List<byte>();
            _encoding = System.Text.Encoding.GetEncoding(encoding);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool flag)
        {
            if (flag)
            {
                IDisposable disposable = this._lBuff as IDisposable;
                disposable?.Dispose();
            }
        }
        /// <summary>
        /// 封包
        /// </summary>
        /// <param name="msg">包体对象</param>
        /// <returns></returns>
        public byte[] Encode(TSocketMessage msg)
        {
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    #region 封装包头
                    bw.Write((Int16)T1);
                    bw.Write((Int16)T2);
                    #endregion

                    #region 包协议
                    if (msg.MsgBuffer != null)
                    {
                        bw.Write((Int64)(4 + msg.MsgBuffer.Length));
                        bw.Write(msg.MsgId);
                        bw.Write(msg.MsgBuffer);
                    }
                    else
                    {
                        bw.Write((Int64)0);
                    }
                    #endregion

                    bytes = ms.ToArray();
                }
            }

            return bytes;
        }
        /// <summary>
        /// 拆包
        /// </summary>
        /// <param name="buff">封包数据</param>
        /// <param name="len">封包长度</param>
        /// <returns></returns>
        public List<TSocketMessage> GetDcAppMess(byte[] buff, int len)
        {
            //拷贝本次的有效字节
            byte[] _b = new byte[len];
            Array.Copy(buff, 0, _b, 0, _b.Length);
            buff = _b;
            if (this._lBuff.Count > 0)
            {
                //拷贝之前遗留的字节
                this._lBuff.AddRange(_b);
                buff = this._lBuff.ToArray();
                this._lBuff.Clear();
                this._lBuff = new List<byte>(2);
            }

            List<TSocketMessage> list = new List<TSocketMessage>();
            MemoryStream ms = new MemoryStream(buff);
            BinaryReader buffers = new BinaryReader(ms, _encoding);
            try
            {
                byte[] _buff;
                Label_0073:
                //判断本次解析的字节是否满足常量字节数 
                if ((buffers.BaseStream.Length - buffers.BaseStream.Position) < CONST_LENGHT)
                {
                    _buff = new byte[(int)(buffers.BaseStream.Length - buffers.BaseStream.Position)];
                    Array.Copy(buff, (int)buffers.BaseStream.Position, _buff, 0, _buff.Length);
                    this._lBuff.AddRange(_buff);
                    return list;
                }
                #region 包头读取
                //循环读取包头
                Label_00983:
                Int16 tt1 = buffers.ReadInt16();
                Int16 tt2 = buffers.ReadInt16();
                if (!(tt1 == T1 && tt2 == T2))
                {
                    long ttttt = buffers.BaseStream.Seek(-3, SeekOrigin.Current);
                    goto Label_00983;
                }
                #endregion

                #region 包协议
                long offset = buffers.ReadInt64();
                #endregion

                #region 包解析
                //剩余字节数大于本次需要读取的字节数
                if (offset < (buffers.BaseStream.Length - buffers.BaseStream.Position))
                {
                    int msgID = buffers.ReadInt32();
                    _buff = new byte[offset - 4];
                    Array.Copy(buff, (int)buffers.BaseStream.Position, _buff, 0, _buff.Length);
                    list.Add(new TSocketMessage(msgID, _buff));
                    //设置偏移量 然后继续循环读取
                    buffers.BaseStream.Seek(offset, SeekOrigin.Current);
                    goto Label_0073;
                }
                else if (offset == (buffers.BaseStream.Length - buffers.BaseStream.Position))
                {
                    int msgID = buffers.ReadInt32();
                    //剩余字节数刚好等于本次读取的字节数
                    _buff = new byte[offset - 4];
                    Array.Copy(buff, (int)buffers.BaseStream.Position, _buff, 0, _buff.Length);
                    list.Add(new TSocketMessage(msgID, _buff));
                }
                else
                {
                    //剩余字节数刚好小于本次读取的字节数 存起来，等待接受剩余字节数一起解析
                    _buff = new byte[(int)(buffers.BaseStream.Length - buffers.BaseStream.Position + CONST_LENGHT)];
                    Array.Copy(buff, (int)(buffers.BaseStream.Position - CONST_LENGHT), _buff, 0, _buff.Length);
                    buff = _buff;
                    this._lBuff.AddRange(_buff);
                }
                #endregion

            }
            catch { }
            finally
            {
                if (buffers != null) { buffers.Dispose(); }
                buffers.Close();
                if (buffers != null) { buffers.Dispose(); }
                ms.Close();
                if (ms != null) { ms.Dispose(); }
            }
            return list;
        }
        /// <summary>
        /// 心跳包封包
        /// </summary>
        /// <returns></returns>
        public byte[] RncodeHeartbeat()
        {
            TSocketMessage msg = new TSocketMessage(0, System.Text.Encoding.UTF8.GetBytes("心跳"));
            return Encode(msg);
        }
    }
}