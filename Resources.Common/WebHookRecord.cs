using Resources.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.Common
{
    public class WebHookRecord:IMessage
    {
        public int ResourceId { get; set; }

        public string EventType { get; set; }

        public string NewValue { get; set; }
    }
}
