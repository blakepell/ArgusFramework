using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

namespace Argus.Network
{
    /// <summary>
    /// Network utilities for Windows.
    /// </summary>
    public static class Utilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  NetworkUtilities
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  04/07/2009
        //      Last Updated:  11/18/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Returns multiple IP's if they exist.  This is for the workstation/server that this is being run from.
        /// </summary>
        public static List<IPAddress> GetLocalIpAddresses()
        {
            var list = new List<IPAddress>();
            string hostName = Dns.GetHostName();

            foreach (var ipAddress in Dns.GetHostEntry(hostName).AddressList)
            {
                list.Add(ipAddress);
            }

            return list;
        }

        /// <summary>
        /// Returns a string value of all ip addresses that are resolved seperated by commas.
        /// </summary>
        /// <param name="ipAddress"></param>
        public static List<string> Resolve(string ipAddress)
        {
            var host = Dns.GetHostEntry(ipAddress);
            var list = new List<string>();

            foreach (var ip in host.AddressList)
            {
                list.Add(ip.ToString());
            }

            return list;
        }

        /// <summary>
        /// Downloads a file from the Internet (http, https)
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localFileName"></param>
        /// <param name="timeout">The timeout in seconds.</param>
        public static void DownloadFile(string remoteUrl, string localFileName, int timeout)
        {
            using (var wc = new WebClient())
            {
                wc.Timeout = timeout;
                wc.BaseAddress = remoteUrl;
                wc.DownloadFile(remoteUrl, localFileName);
            }
        }

        /// <summary>
        /// Downloads a file from the Internet with provided authentication credentials (http, https, ftp)
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localFileName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="timeout">The timeout in seconds.</param>
        public static void DownloadFile(string remoteUrl, string localFileName, string username, string password, int timeout)
        {
            using (var wc = new WebClient())
            {
                wc.Timeout = timeout;
                wc.BaseAddress = remoteUrl;
                wc.Credentials = new NetworkCredential(username, password);
                wc.DownloadFile(remoteUrl, localFileName);
            }
        }

    }
}
