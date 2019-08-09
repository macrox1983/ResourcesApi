using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Common.Abstractions
{
    /// <summary>
    /// Интерфейс шины сообщений для отправки веб-хуков
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Метод подписки на получение сообщения типа TMessage
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="receiveCallback"></param>
        void Subscribe<TMessage>(Action<TMessage> receiveCallback) where TMessage : IMessage;        
       
        /// <summary>
        /// Метод отправки сообщения типа TMessage
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        void SendMessage<TMessage>(TMessage message) where TMessage : IMessage;
    }
}
