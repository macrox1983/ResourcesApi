using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.Common
{
    public class AppSettings
    {
        /// <summary>
        /// Адресс для отправки веб-хука
        /// </summary>
        public string WebHookUri { get; set; }

        /// <summary>
        /// Время жизни веб-хука, в минутах
        /// </summary>
        public int WebHookLifetime { get; set; }

        /// <summary>
        /// Периодичность отправки веб-хуков, в миллисекундах
        /// </summary>
        public int WebHookSendPeriod { get; set; }
    }
}
