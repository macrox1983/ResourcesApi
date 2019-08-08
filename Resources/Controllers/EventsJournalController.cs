using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources.Common;
using Resources.Common.Abstractions;

namespace Resources.Controllers
{
    /// <summary>
    /// Контроллер api на него замкнем отправку веб-хуков для проверки
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]    
    public class EventsJournalController : ControllerBase
    {
        private readonly IEventsJournal _journal;

        public EventsJournalController(IEventsJournal journal)
        {
            _journal = journal ?? throw new ArgumentNullException(nameof(journal));
        }

        //GET api/eventsjournal
        [HttpGet]
        public async Task<ActionResult<List<EventRecord>>> Get()
        {
            return await _journal.GetRecords();
        }

        //POST api/eventsjournal
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EventRecord record)
        {
            if (await _journal.AddRecord(record))
                return Ok();
            return BadRequest();
        }
    }
}