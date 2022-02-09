/*
 * @author            : Blake Pell
 * @initial date      : 2014-06-30
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace Argus.Network
{
    /// <summary>
    /// An inherited version of the WebClient class that exposes the Timeout property.
    /// </summary>
    /// <remarks>
    /// The .Net WebClient for whatever reason doesn't expose the Timeout property.  In order to get around this we
    /// can inherit the WebClient and override the GetWebRequest in order to set it.
    /// </remarks>
    public class WebClient : System.Net.WebClient
    {
        /// <summary>
        /// The timeout to use in seconds.  The default is 100 seconds.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Returns a WebRequest with the timeout set from the specified Timeout property.
        /// </summary>
        /// <param name="address"></param>
        protected override WebRequest GetWebRequest(Uri address)
        {
            var w = base.GetWebRequest(address);

            if (w != null)
            {
                w.Timeout = this.Timeout * 1000;
            }

            return w;
        }
    }
}