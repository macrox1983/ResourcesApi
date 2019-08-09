using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resources.Common.Abstractions
{
    public class Telemetry : ITelemetry
    {
        private IDictionary<string, object> _data;

        public Telemetry(string moduleName)
        {
            ModuleName = moduleName ?? throw new ArgumentNullException(nameof(moduleName));

            _data = new Dictionary<string, object>();
        }

        public object this[string key] { get => _data[key]; set => _data[key] = value; }

        public string ModuleName { get; }

        public IDictionary<string, object> Data => _data;

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public bool TryAdd(string key, object value)
        {
            return _data.TryAdd(key, value);
        }
    }
}
