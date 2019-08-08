using Resources.Common.DataModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Common.Abstractions
{
    public interface IResourcesDataContext
    {
        ConcurrentDictionary<int, ResourceRecord> Records { get; }

        Task<ResourceRecord> GetRecord(int id);

        Task<List<ResourceRecord>> GetRecords();

        Task<OperationResult> AddRecord(string value);

        Task<OperationResult> UpdateRecord(int id, Func<ResourceRecord, bool> updateFunction, string subEvent);

        Task<OperationResult> DeleteRecord(int id);

        Task InitialDataContext();

    }
}
