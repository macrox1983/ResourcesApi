using Resources.Common.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resources.MessageBus
{
    /// <summary>
    /// Шина сообщений для отправки веб-хуков
    /// </summary>
    public class MessageBus : IMessageBus
    {
        private ConcurrentDictionary<Type, ConcurrentBag<Action<object>>> _subscribers;

        public MessageBus()
        {
            _subscribers = new ConcurrentDictionary<Type, ConcurrentBag<Action<object>>>();
        }

        /// <summary>
        /// Метод подписки на получение сообщения типа TMessage
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="receiveCallback"></param>
        public void Subscribe<TMessage>(Action<TMessage> receiveCallback) where TMessage:class, IMessage
        {
            ConcurrentBag<Action<object>> subscribers = null;
            if (_subscribers.TryGetValue(typeof(TMessage), out subscribers) && subscribers != null)
            {
                    var newSubscribers = new ConcurrentBag<Action<object>>(subscribers);
                    newSubscribers.Add(o => receiveCallback((TMessage)o));
                    _subscribers.TryUpdate(typeof(TMessage), newSubscribers, subscribers);
            }
            else
            {
                _subscribers.TryAdd(typeof(TMessage), new ConcurrentBag<Action<object>>() { o => receiveCallback((TMessage)o) });
            }            
        }

        /// <summary>
        /// Метод отправки сообщения типа TMessage
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        public void SendMessage<TMessage>(TMessage message) where TMessage : class, IMessage
        {
            ConcurrentBag<Action<object>> subscribers;

            if(_subscribers.TryGetValue(message.GetType(), out subscribers) && subscribers != null)
            {
                Parallel.ForEach(subscribers.ToArray(), s=>s(message));                           
            }
        }

        public int GetSubscriberCount<TMessage>() where TMessage : class, IMessage
        {
            return GetSubscriberCount(typeof(TMessage));
        }

        public int GetSubscriberCount(Type messageType)
        {
            ConcurrentBag<Action<object>> subscribers;

            if (_subscribers.TryGetValue(messageType, out subscribers) && subscribers != null)
            {
                return subscribers.Count;
            }
            return 0;
        }
    }
}
