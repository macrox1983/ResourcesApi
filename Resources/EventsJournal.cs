using Resources.Common;
using Resources.Common.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resources
{
    /// <summary>
    /// Журнал событий изменения ресурсов
    /// </summary>
    public class EventsJournal : IEventsJournal
    {
        private ConcurrentBag<EventRecord> _journal;

        public EventsJournal()
        {
            _journal = new ConcurrentBag<EventRecord>();
        }

        public async Task<bool> AddRecord(EventRecord record)
        {
            bool result;
            try
            {
                _journal.Add(record);
                result = true;
            }
            catch
            {
                result = false;
            }
            return await Task.FromResult(result);
        }

        public async Task<List<EventRecord>> GetRecords()
        {
            return await Task.FromResult(_journal.ToList());
        }
    }
}
