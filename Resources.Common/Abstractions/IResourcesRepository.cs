using Resources.Common.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Common.Abstractions
{
    public interface IResourcesRepository
    {
        /// <summary>
        /// Получить список всех ресурсов
        /// </summary>
        /// <returns></returns>
        Task<List<ResourceRecord>> Get();

        /// <summary>
        /// Получить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResourceRecord> Get(int id);

        /// <summary>
        /// Получить подстроку ресурса по индексу начала и длине подстроки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        Task<string> Substring(int id, int start, int lenght);

        /// <summary>
        /// Создать ресурс (с возможностью предоставить начальное значение)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<OperationResult> Create(string value);

        /// <summary>
        /// Перезаписать ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        Task<OperationResult> Update(int id, string newValue);

        /// <summary>
        /// Удалить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(int id);

        /// <summary>
        /// Вставка подстроки в начало
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        Task<OperationResult> AddToStart(int id, string substring);

        /// <summary>
        /// Вставка подстроки в конец
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<OperationResult> Append(int id, string value);

        /// <summary>
        /// Вставка подстроки в любое место по индексу
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        Task<OperationResult> Insert(int id, string value, int index);

        /// <summary>
        /// Удаление подстроки по индексу и длине
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        Task<OperationResult> DeleteSubstring(int id, int start, int lenght);

        /// <summary>
        /// Замена подстроки на другую
        /// </summary>
        /// <param name="id"></param>
        /// <param name="substring"></param>
        /// <param name="newSubstring"></param>
        /// <returns></returns>
        Task<OperationResult> Replace(int id, string oldValue, string newValue);
    }
}
