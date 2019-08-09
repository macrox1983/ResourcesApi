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
            var mName = Path.GetFileNameWithoutExtension(moduleName);
            AddStartConstant(mName, methodName);
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();           
            AddTelemetry(Path.GetFileNameWithoutExtension(moduleName), methodName, stopwatch.ElapsedMilliseconds);
        }

        public async Task<TResult> WithStopwatch<TResult>(Func<Task<TResult>> func, [CallerFilePath] string moduleName = "", [CallerMemberName] string methodName = "")
        {
            var mName = Path.GetFileNameWithoutExtension(moduleName);
            AddStartConstant(mName, methodName);
            var stopwatch = Stopwatch.StartNew();
            TResult result = await func();
            stopwatch.Stop();
            AddTelemetry(mName, methodName, stopwatch.ElapsedMilliseconds);
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
            AddTotalExecutionTime(telemetry, methodName, executionTime);
            AddRequestPerSecondFromStartTelemetry(telemetry, methodName, executionCount);
            AddRequestPerSecondByTotalExecutionTime(telemetry, methodName, executionCount);
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

        private void AddTotalExecutionTime(ITelemetry telemetry, string methodName, double executionTime)
        {
            string totalExecutionTime = $"{methodName}_TotalExecutionTime, sec";
            if (telemetry.ContainsKey(totalExecutionTime))
            {
                double totalExecutionTimeCount = (double)telemetry[totalExecutionTime] + TimeSpan.FromMilliseconds(executionTime).TotalSeconds;
                telemetry[totalExecutionTime] = totalExecutionTimeCount;                
            }
            else
            {
                telemetry.TryAdd(totalExecutionTime, TimeSpan.FromMilliseconds(executionTime).TotalSeconds);
            }
        }

        private void AddStartConstant(string moduleName, string methodName)
        {
            Task.Run(() => 
            {
                string startMethodTelemetry = $"{methodName}_StartTelemetry";
                ITelemetry telemetry = null;
                if (!_telemetry.ContainsKey(moduleName) || !_telemetry.TryGetValue(moduleName, out telemetry))
                {

                    telemetry = new Telemetry(moduleName);
                    _telemetry.TryAdd(moduleName, telemetry);
                }
                if(!telemetry.ContainsKey(startMethodTelemetry))
                    telemetry.TryAdd(startMethodTelemetry, DateTime.Now);
            });            
        }

        private void AddRequestPerSecondFromStartTelemetry(ITelemetry telemetry, string methodName, long executionCount)
        {
            string startMethodTelemetry = $"{methodName}_StartTelemetry";
            string requestPerSecond = $"{methodName}_RequestsPerSecondFromStartTelemetry";
            if(telemetry.ContainsKey(startMethodTelemetry))
            {
                double requestPerSecondCount = executionCount/(DateTime.Now - (DateTime)telemetry[startMethodTelemetry]).TotalSeconds;
                if (telemetry.ContainsKey(requestPerSecond))
                    telemetry[requestPerSecond] = requestPerSecondCount;
                telemetry.TryAdd(requestPerSecond, requestPerSecondCount);
            }
        }

        private void AddRequestPerSecondByTotalExecutionTime(ITelemetry telemetry, string methodName, long executionCount)
        {
            string totalExecutionTime = $"{methodName}_TotalExecutionTime, sec";
            string requestPerSecond = $"{methodName}_RequestsPerSecondByTotalExecutionTime";
            if (telemetry.ContainsKey(totalExecutionTime))
            {
                double totalExecutionTimeCount = (double)telemetry[totalExecutionTime];
                double requestPerSecondCount = executionCount / (totalExecutionTimeCount != 0d? totalExecutionTimeCount : 1d);
                if (telemetry.ContainsKey(requestPerSecond))
                    telemetry[requestPerSecond] = requestPerSecondCount;
                else
                    telemetry.TryAdd(requestPerSecond, requestPerSecondCount);
            }
        }
        //добавить обработку запросов в секунду
    }
}
