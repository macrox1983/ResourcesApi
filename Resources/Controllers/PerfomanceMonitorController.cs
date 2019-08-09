using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources.Common.Abstractions;

namespace Resources.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfomanceMonitorController : ControllerBase
    {
        private readonly IPerfomanceMonitor _perfomanceMonitor;

        public PerfomanceMonitorController(IPerfomanceMonitor perfomanceMonitor)
        {
            _perfomanceMonitor = perfomanceMonitor ?? throw new ArgumentNullException(nameof(perfomanceMonitor));
        }

        //GET api/perfomancemonitor
        [HttpGet]
        public async Task<ActionResult<List<ITelemetry>>> Get()
        {
            List<ITelemetry> telemetry = await _perfomanceMonitor.GetTelemetry();
            return telemetry;
        }
    }
}