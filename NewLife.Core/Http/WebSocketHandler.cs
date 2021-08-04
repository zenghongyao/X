﻿using System;
using NewLife.Log;

namespace NewLife.Http
{
    /// <summary>WebSocket处理器</summary>
    public class WebSocketHandler : IHttpHandler
    {
        #region 属性
        private IHttpContext _context;
        #endregion

        /// <summary>处理请求</summary>
        /// <param name="context"></param>
        public virtual void ProcessRequest(IHttpContext context)
        {
            var ws = context.WebSockets;
            ws.Handler = ProcessMessage;

            _context = context;

            WriteLog("WebSocket连接 {0}", context.Connection.Remote);
        }

        /// <summary>处理消息</summary>
        /// <param name="message"></param>
        public virtual void ProcessMessage(WebSocketMessage message)
        {
            var ws = _context.WebSockets;
            switch (message.Type)
            {
                //case WebSocketMessageType.Data:
                //    break;
                case WebSocketMessageType.Text:
                    var msg = message.Payload?.ToStr();
                    WriteLog("WebSocket收到[{0}] {1}", message.Type, msg);
                    ws.Send("你在说，" + msg);
                    break;
                //case WebSocketMessageType.Binary:
                //    break;
                case WebSocketMessageType.Close:
                    WriteLog("WebSocket关闭 {0}", _context.Connection.Remote);
                    break;
                case WebSocketMessageType.Ping:
                case WebSocketMessageType.Pong:
                    WriteLog("WebSocket心跳[{0}]", message.Type);
                    break;
                default:
                    WriteLog("WebSocket收到[{0}]", message.Type);
                    break;
            }
        }

        private void WriteLog(String format, params Object[] args) => XTrace.WriteLine(format, args);
    }
}