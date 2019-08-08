using Resources.Common.ApiModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resources.Common.Abstractions
{
    /// <summary>
    /// Интерфейс класс для работы с репозиторием и отправки webhook если операция успешна
    /// </summary>
    public interface IResourceService
    {
        /// <summary>
        /// Получить список всех ресурсов
        /// </summary>
        /// <returns></returns>
        Task<List<Resource>> Get();

        /// <summary>
        /// Получить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Resource> Get(int id);

        /// <summary>
        /// Получить подстроку ресурса по индексу начала и длине подстроки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        Task<string> Substring(int id, int start, int lenght);

        /// <summary>
        /// (с возможностью предоставить начальное значение)
        /// </summary>
        /// <returns></returns>
        Task<string> Create();

        /// <summary>
        /// Создать ресурс (с возможностью предоставить начальное значение)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> Create(string value);

        /// <summary>
        /// Перезаписать ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        Task<bool> Update(int id, string newValue);

        /// <summary>
        /// Удалить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(int id);

        /// <summary>
        /// Вставка подстроки в начало
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        Task<bool> AddToStart(int id, string substring);

        /// <summary>
        /// Вставка подстроки в конец
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> Append(int id, string value);

        /// <summary>
        /// Вставка подстроки в любое место по индексу
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        Task<bool> Insert(int id, string value, int index);

        /// <summary>
        /// Удаление подстроки по индексу и длине
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        Task<bool> DeleteSubstring(int id, int start, int lenght);

        /// <summary>
        /// Замена подстроки на другую
        /// </summary>
        /// <param name="id"></param>
        /// <param name="substring"></param>
        /// <param name="newSubstring"></param>
        /// <returns></returns>
        Task<bool> Replace(int id, string oldValue, string newValue);
    }
}
