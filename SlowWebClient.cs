
namespace ZohoSync
{
    using System;
    using System.Net;

    /// <summary>
    /// slow web client
    /// </summary>
    internal class SlowWebClient : WebClient
    {
        /// <summary>
        /// constructor
        /// </summary>
        public SlowWebClient()
        {
        }

        /// <summary>
        /// override getwebrequest
        /// </summary>
        /// <param name="address">address</param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);

            // increase timeout
            request.Timeout = 20 * 60 * 1000;

            return request;
        }
    }
}
