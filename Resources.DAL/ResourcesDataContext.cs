using Resources.Common;
using Resources.Common.Abstractions;
using Resources.Common.DataModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resources.DAL
{
    /// <summary>
    /// Класс хранящий ресурсы и позволяющий производить операции над ними
    /// </summary>
    public class ResourcesDataContext : IResourcesDataContext
    {
        private bool _initialized;

        private object _lock = new object();

        private int _newId;

        /// <summary>
        /// Список всех ресурсов
        /// </summary>
        public ConcurrentDictionary<int, ResourceRecord> Records { get; private set; }        

        /// <summary>
        /// Операция добавления нового ресурса
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddRecord(string value)
        {
            OperationResult result = new OperationResult{ ResourceId = _newId, EventType = "Create" };
            lock(_lock)
            {                
                result.Success = Records.TryAdd(_newId, new ResourceRecord { Id = _newId, Value = value, Version = 1 });
                if (result.Success)
                {
                    result.NewValue = value;
                    _newId++;
                }
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// Удаление ресурса
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteRecord(int id)
        {
            OperationResult result = new OperationResult() { ResourceId = id, EventType = "Delete"};
            lock (_lock)
            {
                result.Success = Records.TryRemove(id, out ResourceRecord record);
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// Функция получения ресурса по ид
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResourceRecord> GetRecord(int id)
        {
            if (Records.ContainsKey(id))
                return await Task.FromResult(Records[id]);
            return default;
        }

        /// <summary>
        /// Функция получения всех ресурсов
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResourceRecord>> GetRecords()
        {
            return await Task.FromResult(Records.Values.ToList());
        }

        /// <summary>
        /// Инициализация тестовыми данными
        /// </summary>
        /// <returns></returns>
        public async Task InitialDataContext()
        {
            if (!_initialized)
            {
                Records = new ConcurrentDictionary<int, ResourceRecord>();

                for (int i = 0; i < 1000; i++)
                {
                    await AddRecord(GenerateValue(10 + i));
                }

                _initialized = true;
            }
        }

        /// <summary>
        /// функция генерирующая значение ресурса
        /// </summary>
        /// <param name="maxLenght"></param>
        /// <returns></returns>
        private string GenerateValue(int maxLenght)
        {            
            string symbols = "абвгдеёжзийклмнопрстуфхцчьыъэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧЬЫЪЭЮЯ0123456789";
            var random = new Random();
            var lenght = random.Next(10, maxLenght);
            string result = "";
            for (int i = 0; i < lenght; i++)
            {
                var index = random.Next(symbols.Length);
                result += symbols[index];
            }
            return result;
        }

        /// <summary>
        /// Операция обновления значения ресурса общая для все операций изменения
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateFunction"></param>
        /// <param name="subEvent"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateRecord(int id, Func<ResourceRecord, bool> updateFunction, string subEvent)
        {
            OperationResult result = new OperationResult() { ResourceId = id, EventType = $"Update{(string.IsNullOrEmpty(subEvent)?"":$".{subEvent}")}" };
            lock (_lock)
            {
                result.Success = Records.TryGetValue(id, out ResourceRecord oldValue);
                var newValue = oldValue;
                result.Success = updateFunction(newValue);
                if(result.Success)
                    result.Success = Records.TryUpdate(id, newValue, oldValue);
                if (result.Success)
                    result.NewValue = newValue.Value;
            }
            return await Task.FromResult(result);
        }
    }
}
