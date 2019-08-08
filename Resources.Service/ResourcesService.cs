using Resources.Common;
using Resources.Common.Abstractions;
using Resources.Common.ApiModel;
using Resources.Common.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resources.Service
{
    /// <summary>
    /// Класс для работы с репозиторием и отправки webhook если операция успешна
    /// </summary>
    public class ResourcesService : IResourceService
    {
        private readonly IResourcesRepository _repository;
        private readonly IMessageBus _messageBus;

        public ResourcesService(IResourcesRepository repository, IMessageBus messageBus)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
        }

        /// <summary>
        /// Вставка подстроки в начало
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public async Task<bool> AddToStart(int id, string substring)
        {
            return await WithWebHook(async () => await _repository.AddToStart(id, substring));
        }

        /// <summary>
        /// Вставка подстроки в конец
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Append(int id, string value)
        {
            return await WithWebHook(async () => await _repository.Append(id, value));
        }

        /// <summary>
        /// Создать ресурс (с возможностью предоставить начальное значение)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string> Create()
        {
            return await Task.FromResult("начальное значение");
        }

        /// <summary>
        /// Создать ресурс (с возможностью предоставить начальное значение)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Create(string value)
        {            
            return await WithWebHook(async () => await _repository.Create(value));
        }

        /// <summary>
        /// Удалить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(int id)
        {
            return await WithWebHook(async () => await _repository.Delete(id));
        }

        /// <summary>
        /// Удаление подстроки по индексу и длине
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSubstring(int id, int start, int lenght)
        {
            return await WithWebHook(async () => await _repository.DeleteSubstring(id, start, lenght));
        }

        /// <summary>
        /// Получить список всех ресурсов
        /// </summary>
        /// <returns></returns>
        public async Task<List<Resource>> Get()
        {
            List<ResourceRecord> records = await _repository.Get();

            return records.Select(record => new Resource { Id = record.Id, Value = record.Value }).ToList();
        }

        /// <summary>
        /// Получить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Resource> Get(int id)
        {
            ResourceRecord record = await _repository.Get(id);
            if(record != null)
                return new Resource { Id = record.Id, Value = record.Value };
            return default;
        }

        /// <summary>
        /// Вставка подстроки в любое место по индексу
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<bool> Insert(int id, string value, int index)
        {
            return await WithWebHook(async ()=>await _repository.Insert(id, value, index));            
        }

        /// <summary>
        /// Замена подстроки на другую
        /// </summary>
        /// <param name="id"></param>
        /// <param name="substring"></param>
        /// <param name="newSubstring"></param>
        /// <returns></returns>
        public async Task<bool> Replace(int id, string oldValue, string newValue)
        {
            return await WithWebHook(async ()=>await _repository.Replace(id, oldValue, newValue));
        }

        /// <summary>
        /// Получить подстроку ресурса по индексу начала и длине подстроки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public async Task<string> Substring(int id, int start, int lenght)
        {
            return await _repository.Substring(id, start, lenght);
        }

        /// <summary>
        /// Перезаписать ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public async Task<bool> Update(int id, string newValue)
        {
            return await WithWebHook(async ()=>await _repository.Update(id, newValue));            
        }

        private async Task<bool> WithWebHook(Func<Task<OperationResult>> function)
        {
            OperationResult result = await function();
            if (result.Success)
            {
                _messageBus.SendMessage(new WebHookRecord { ResourceId = result.ResourceId, EventType = result.EventType, NewValue = result.NewValue});
            }
            return result.Success;
        }
    }
}
