using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Common.Abstractions
{
    public interface IEventsJournal
    {
        Task<List<EventRecord>> GetRecords();

        Task<bool> AddRecord(EventRecord record);
    }
}
