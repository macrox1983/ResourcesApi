using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Common.Abstractions
{
    public interface IPerfomanceMonitor
    {
        Task<List<ITelemetry>> GetTelemetry();
    }
}
