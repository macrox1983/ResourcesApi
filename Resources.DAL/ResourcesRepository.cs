using Resources.Common;
using Resources.Common.Abstractions;
using Resources.Common.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resources.DAL
{
    /// <summary>
    /// Класс репозиторий для работы с хранилищем данных
    /// </summary>
    public class ResourcesRepository : IResourcesRepository
    {
        private readonly IResourcesDataContext _dataContext;

        public ResourcesRepository(IResourcesDataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));            
        }

        /// <summary>
        /// Вставка подстроки в начало
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddToStart(int id, string value)
        {
            return await _dataContext.UpdateRecord(id, r =>
            {
                if (string.IsNullOrEmpty(value))
                    return false;
                r.Value = value + r.Value;
                return true;
            }, nameof(AddToStart));
        }

        /// <summary>
        /// Вставка подстроки в конец
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<OperationResult> Append(int id, string value)
        {
            return await _dataContext.UpdateRecord(id, r=> 
            {
                if (string.IsNullOrEmpty(value))
                    return false;
                r.Value += value;
                return true;
            }, nameof(Append));
        }

        /// <summary>
        /// Создать ресурс (с возможностью предоставить начальное значение)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<OperationResult> Create(string value)
        {
            return await _dataContext.AddRecord(value);
        }

        /// <summary>
        /// Удалить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>       
        public async Task<OperationResult> Delete(int id)
        {
            return await _dataContext.DeleteRecord(id);
        }

        /// <summary>
        /// Удаление подстроки по индексу и длине
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteSubstring(int id, int start, int lenght)
        {
            return await _dataContext.UpdateRecord(id,r=> 
            {
                if (r.Value.Length < start || r.Value.Length < start + lenght)
                    return false;
                r.Value = r.Value.Remove(start, lenght);
                return true;
            }, nameof(DeleteSubstring));
        }

        /// <summary>
        /// Получить список всех ресурсов
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResourceRecord>> Get()
        {
            return await _dataContext.GetRecords();
        }

        /// <summary>
        /// Получить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResourceRecord> Get(int id)
        {
            return await _dataContext.GetRecord(id);
        }

        /// <summary>
        /// Вставка подстроки в любое место по индексу
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<OperationResult> Insert(int id, string value, int index)
        {
            return await _dataContext.UpdateRecord(id, r=> 
            {
                if (r.Value.Length <= index)
                    return false;
                r.Value = r.Value.Insert(index, value);
                return true;
            }, nameof(Insert));
        }

        /// <summary>
        /// Замена подстроки на другую
        /// </summary>
        /// <param name="id"></param>
        /// <param name="substring"></param>
        /// <param name="newSubstring"></param>
        /// <returns></returns>
        public async Task<OperationResult> Replace(int id, string oldValue, string newValue)
        {
            return await _dataContext.UpdateRecord(id, r => 
            {
                if (!r.Value.Contains(oldValue))
                    return false;
                r.Value = r.Value.Replace(oldValue, newValue);
                return true;
            }, nameof(Replace));
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
            return (await _dataContext.GetRecord(id))?.Value.Substring(start, lenght);
        }

        /// <summary>
        /// Перезаписать ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(int id, string newValue)
        {
            return await _dataContext.UpdateRecord(id, r=> 
            {
                r.Value = newValue;
                return true;
            }, "");
        }
    }
}
