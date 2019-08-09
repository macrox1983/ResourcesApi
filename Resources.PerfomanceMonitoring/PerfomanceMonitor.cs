using Resources.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resources.PerfomanceMonitoring
{
    public class PerfomanceMonitor : IPerfomanceMonitor
    {
        private readonly ITelemetryCollector _telemetryCollector;

        public PerfomanceMonitor(ITelemetryCollector telemetryCollector)
        {
            _telemetryCollector = telemetryCollector ?? throw new ArgumentNullException(nameof(telemetryCollector));
        }

        public async Task<List<ITelemetry>> GetTelemetry()
        {
            return await _telemetryCollector.GetData();
        }
    }
}
