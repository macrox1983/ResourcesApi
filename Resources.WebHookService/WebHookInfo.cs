using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.WebHookService
{
    /// <summary>
    /// Класс содержащий информацию о веб-хуке
    /// </summary>
    public struct WebHookInfo
    {
        /// <summary>
        /// ид ресурса
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// время в которое веб-хук должен прекратить свое существование
        /// </summary>
        public DateTime MustDieAt { get; set; }

        public static bool IsEmpty(WebHookInfo hookInfo)
        {
            return hookInfo.ResourceId == 0 && hookInfo.MustDieAt == DateTime.MinValue;
        }
    }
}
