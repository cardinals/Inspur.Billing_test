using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Net
{
    public class MessageModel
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public byte MessageId { get; set; }
        /// <summary>
        /// 返回的流
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 流解析的文本
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 校验码
        /// </summary>
        public string Crc { get; set; }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
