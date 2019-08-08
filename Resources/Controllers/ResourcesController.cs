using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Resources.Common;
using Resources.Common.Abstractions;
using Resources.Common.ApiModel;

namespace Resources.Controllers
{
    /// <summary>
    /// Контроллер api всех операций над ресурсами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceService _service;

        public ResourcesController(IResourceService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET api/resources
        [HttpGet]
        public async Task<ActionResult<List<Resource>>> Get()
        {
            return await _service.Get();
        }

        // GET api/resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> Get(int id)
        {
            return await _service.Get(id);
        }

        // GET api/resources/create
        [HttpGet("create")]
        public async Task<ActionResult<string>> Create()
        {
            return await _service.Create();
        }

        //POST api/resources/create
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody]string value)
        {
            if (await _service.Create(value))
                return Created("", null);
            return BadRequest();            
        }

        //PUT api/resources/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] string newValue)
        {
            if (await _service.Update(id, newValue))
                return Ok();
            return BadRequest();
        }

        //DELETE api/resources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (await _service.Delete(id))
                return Ok();
            return BadRequest();
        }

        //PUT api/resources/addtostart/5
        [HttpPut("addtostart/{id}")]
        public async Task<ActionResult> AddToStart(int id, [FromBody]string substring)
        {
            if (await _service.AddToStart(id, substring))
                return Ok();
            return BadRequest();
        }

        //PUT api/resources/append/5
        [HttpPut("append/{id}")]
        public async Task<ActionResult> Append(int id, [FromBody]string substring)
        {
            if (await _service.Append(id, substring))
                return Ok();
            return BadRequest();
        }

        //PUT api/resources/insert/5
        [HttpPut("insert/{id}")]
        public async Task<ActionResult> Insert(int id, [FromBody]InsertParameters parameters)
        {
            if (await _service.Insert(id, parameters.Substring, parameters.Index))
                return Ok();
            return BadRequest();
        }

        //DELETE api/resources/deletesubstring/5
        [HttpDelete("deletesubstring/{id}")]
        public async Task<ActionResult<Resource>> DeleteSubstring(int id, [FromBody]DeleteSubstringParameters parameters)
        {
            if (await _service.DeleteSubstring(id, parameters.Start, parameters.Lenght))
                return Ok();
            return BadRequest();
        }

        //PUT api/resources/replace/5
        [HttpPut("replace/{id}")]
        public async Task<ActionResult<Resource>> Replace(int id, [FromBody]ReplaceParameters parameters)
        {
            if (await _service.Replace(id, parameters.OldValue, parameters.NewValue))
                return Ok();
            return BadRequest();
        }      
    }
}
