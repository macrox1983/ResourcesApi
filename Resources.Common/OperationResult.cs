using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.Common
{
    public struct OperationResult
    {
        public bool Success { get; set; }

        public int ResourceId { get; set; }

        public string EventType { get; set; }

        public string NewValue { get; set; }
    }
}
