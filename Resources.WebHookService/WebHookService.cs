﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Resources.Common;
using Resources.Common.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resources.WebHookService
{
    /// <summary>
    /// Класс сервис периодической отправки веб-хуков
    /// </summary>
    public class WebHookService : IHostedService
    {
        private readonly IMessageBus _messageBus;
        private readonly IOptions<AppSettings> _options;
        private ConcurrentDictionary<int, WebHookInfo> _webHooksInfo; //хранилище доп инфы о веб-хуках
        private ConcurrentQueue<WebHookRecord> _webHooks;//очередь веб-хуков на отправку
        private object _lock = new object();
        private Timer _sendHookTimer;

        public WebHookService(IMessageBus messageBus, IOptions<AppSettings> options)
        {
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _webHooks = new ConcurrentQueue<WebHookRecord>();
            _webHooksInfo = new ConcurrentDictionary<int, WebHookInfo>();            
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.FromResult(Subscribe());
            _sendHookTimer = new Timer(new TimerCallback(SendHook), _options, 5000, 5000);
        }

        /// <summary>
        /// Метод подписки на получение объекта веб-хук
        /// </summary>
        /// <returns></returns>
        private bool Subscribe()
        {
            _messageBus.Subscribe<WebHookRecord>(ReceiveWebHookRecord);
            return true;
        }

        /// <summary>
        /// Отправка веб-хука на uri из настроек
        /// </summary>
        /// <param name="state"></param>
        private void SendHook(object state)
        {
            lock (_lock)
            {
                WebHookRecord hookRecord = null;
                WebHookInfo hooInfo = null;
                if (_webHooks.TryDequeue(out hookRecord)//извлекаем хук
                        && hookRecord != null
                        && _webHooksInfo.TryRemove(hookRecord.ResourceId, out hooInfo) //извлекаем его доп инфу
                        && hooInfo != null
                        && hooInfo.MustDieAt >= DateTime.Now)//проверяем не протух ли веб-хук
                {
                    var httpClient = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, _options.Value.WebHookUri);
                    request.Content =
                        new StringContent($"{{\"resourceid\":\"{hookRecord.ResourceId}\", \"eventtype\":\"{hookRecord.EventType}\",\"newvalue\":\"{hookRecord.NewValue}\"}}"
                        , Encoding.UTF8
                        , "application/json");
                    var sendingTask = httpClient.SendAsync(request);
                    sendingTask.Wait();
                    if (sendingTask.Result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        _webHooksInfo.TryAdd(hooInfo.ResourceId, hooInfo);
                        _webHooks.Enqueue(hookRecord);
                    }
                }                
            }
        }

        /// <summary>
        /// Обработка получения веб-хука от шины сообщений
        /// </summary>
        /// <param name="webHook"></param>
        private void ReceiveWebHookRecord(WebHookRecord webHook)
        {
            lock(_lock)
            {
                var newWebHookInfo = new WebHookInfo { ResourceId = webHook.ResourceId, MustDieAt = DateTime.Now.AddMinutes(_options.Value.WebHookLifetime)};
                if (_webHooksInfo.ContainsKey(webHook.ResourceId))
                {
                    _webHooksInfo.TryGetValue(webHook.ResourceId, out WebHookInfo oldValue);
                    _webHooksInfo.TryUpdate(webHook.ResourceId, newWebHookInfo, oldValue);
                }
                else
                {
                    _webHooksInfo.TryAdd(webHook.ResourceId, newWebHookInfo);                 
                }
                _webHooks.Enqueue(webHook);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.FromResult(Stop());
        }

        private bool Stop()
        {
            return true;
        }
    }
}