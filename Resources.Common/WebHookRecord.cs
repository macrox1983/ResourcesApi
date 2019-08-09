using Resources.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.Common
{
    public struct WebHookRecord : IMessage
    {
        public int ResourceId { get; set; }

        public string EventType { get; set; }

        public string NewValue { get; set; }

        public static bool IsEmpty(WebHookRecord record)
        {
            return record.ResourceId == 0 && string.IsNullOrWhiteSpace(record.EventType) && string.IsNullOrWhiteSpace(record.NewValue);
        }
    }
}
