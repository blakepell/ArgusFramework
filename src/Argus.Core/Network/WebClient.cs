using System;
using System.Net;

namespace Argus.Network
{
    /// <summary>
    ///     An inherited version of the WebClient class that exposes the Timeout property.
    /// </summary>
    /// <remarks>
    ///     The .Net WebClient for whatever reason doesn't expose the Timeout property.  In order to get around this we
    ///     can inherit the WebClient and override the GetWebRequest in order to set it.
    /// </remarks>
    public class WebClient : System.Net.WebClient
    {
        //*********************************************************************************************************************
        //
        //             Class:  WebClient
        //      Initial Date:  06/30/2014
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     The timeout to use in seconds.  The default is 100 seconds.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        ///     Returns a WebRequest with the timeout set from the specified Timeout property.
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