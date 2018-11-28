namespace System.Sockets
{
    /// <summary>
    /// 底层通信消息
    /// </summary>
    public class TSocketMessage : IDisposable
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public Int32 MsgId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public byte[] MsgBuffer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId">消息ID</param>
        /// <param name="msg">消息内容</param>
        public TSocketMessage(int msgId, byte[] msg)
        {
            this.MsgId = msgId;
            this.MsgBuffer = msg;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool flag1)
        {
            if (flag1) { this.MsgBuffer = null; }
        }
    }
}