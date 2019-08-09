using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Common.Abstractions
{
    /// <summary>
    /// Интерфейс сборщика телеметрии
    /// </summary>
    public interface ITelemetryCollector
    {
        Task<List<ITelemetry>> GetData();

        Task WithStopwatch(Action action, [CallerFilePath] string moduleName = "", [CallerMemberName] string methodName = "");

        Task<TResult> WithStopwatch<TResult>(Func<Task<TResult>> action, [CallerFilePath] string moduleName = "", [CallerMemberName] string methodName = "");

        void RegisterCollectTelemetry(Func<KeyValuePair<string, object>> collectFunc, [CallerFilePath] string moduleName = "", [CallerMemberName] string methodName = "");
    }
}
