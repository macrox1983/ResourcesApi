using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.Common.Abstractions
{
    public interface ITelemetry
    {
        string ModuleName { get; }

        IDictionary<string, object> Data { get; }

        object this[string key] { get; set; }

        bool ContainsKey(string key);

        bool TryAdd(string key, object value);        
    }
}
