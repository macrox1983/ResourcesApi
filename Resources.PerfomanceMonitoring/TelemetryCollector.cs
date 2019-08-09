using Resources.Common.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Resources.PerfomanceMonitoring
{
    public class TelemetryCollector : ITelemetryCollector
    {
        private ConcurrentDictionary<string, ITelemetry> _telemetry;//key-имя модуля, value - словарь телеметрии ключ, значение
        private ConcurrentDictionary<string, Func<KeyValuePair<string, object>>> _runtimeTelemetry;// 

        public TelemetryCollector()
        {
            _telemetry = new ConcurrentDictionary<string, ITelemetry>();
            _runtimeTelemetry = new ConcurrentDictionary<string, Func<KeyValuePair<string, object>>>();
        }

        public async Task<List<ITelemetry>> GetData()
        {
            return await Task.FromResult(_telemetry.Values.ToList());
        }

        public void RegisterCollectTelemetry(Func<KeyValuePair<string, object>> collectFunc, [CallerFilePath] string moduleName = "", [CallerMemberName] string methodName = "")
        {            
            
            //((System.Reflection.RuntimeMethodInfo)collectFunc.Method).DeclaringType.Name - имя класса из которого вызван метод
            //callerMember - имя метода из которого вызван текущий метод            
        }

        public async Task WithStopwatch(Action action, [CallerFilePath] string moduleName="", [CallerMemberName] string methodName = "")
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();           
            AddTelemetry(Path.GetFileNameWithoutExtension(moduleName), methodName, stopwatch.ElapsedMilliseconds);
        }

        public async Task<TResult> WithStopwatch<TResult>(Func<Task<TResult>> func, [CallerFilePath] string moduleName = "", [CallerMemberName] string methodName = "")
        {
            var stopwatch = Stopwatch.StartNew();
            TResult result = await func();
            stopwatch.Stop();
            AddTelemetry(Path.GetFileNameWithoutExtension(moduleName), methodName, stopwatch.ElapsedMilliseconds);
            return result;
        }

        private void AddTelemetry(string moduleName, string methodName, double executionTime)
        {
            Task.Run(() =>
            {
                ITelemetry telemetry = null;
                if (!_telemetry.ContainsKey(moduleName) ||!_telemetry.TryGetValue(moduleName, out telemetry))
                {

                    telemetry = new Telemetry(moduleName);
                    _telemetry.TryAdd(moduleName, telemetry);
                }
                AddTelemetryConstants(telemetry, methodName, executionTime);

            });            
        }

        private void AddTelemetryConstants(ITelemetry telemetry, string methodName, double executionTime)
        {
            string lastExecTime = $"{methodName}_LastExecutionTime, ms";
            string execCounter = $"{methodName}_ExecutionCount";            
            if (telemetry.ContainsKey(lastExecTime))
            {
                telemetry[lastExecTime] = executionTime;
            }
            else
            {
                telemetry.TryAdd(lastExecTime, executionTime);
            }
            long executionCount = 1;
            if (telemetry.ContainsKey(execCounter))
            {                
                executionCount = ((long)telemetry[execCounter])+1;
                telemetry[execCounter] = executionCount;                
            }
            else
            {
                telemetry.TryAdd(execCounter, executionCount);
            }
            AddAverageExecutionTime(telemetry, methodName, executionTime, executionCount);
        }

        private void AddAverageExecutionTime(ITelemetry telemetry, string methodName, double executionTime, long executionCount)
        {
            string avgExecTime = $"{methodName}_AverageExecutionTime, ms";
            if (telemetry.ContainsKey(avgExecTime))
            {
                var avgExecutionTime = (((double)telemetry[avgExecTime]) + executionTime) / executionCount;
                telemetry[avgExecTime] = avgExecutionTime;
            }
            else
            {
                telemetry.TryAdd(avgExecTime, executionTime);
            }
        }
    }
}
