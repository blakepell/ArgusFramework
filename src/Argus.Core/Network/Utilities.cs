/*
 * @author            : Blake Pell
 * @initial date      : 2009-04-07
 * @last updated      : 2020-10-23
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Argus.Network
{
    /// <summary>
    /// Network utilities for Windows.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Returns multiple IPs if they exist.  This is for the workstation/server that this is being run from.
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
        /// Returns a string value of all ip addresses that are resolved separated by commas.
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
        /// Downloads a file from the Internet (http, https).
        /// </summary>
        /// <param name="remoteUrl"></param>
        public static async Task<byte[]> DownloadFileAsync(string remoteUrl)
        {
            using var wc = new System.Net.WebClient();

            return await wc.DownloadDataTaskAsync(remoteUrl);
        }

        /// <summary>
        /// Downloads a file from the Internet (http, https).  If an exception occurs a
        /// null will be returned.
        /// </summary>
        /// <param name="remoteUrl"></param>
        public static async Task<byte[]> SafeDownloadFileAsync(string remoteUrl)
        {
            try
            {
                using var wc = new System.Net.WebClient();

                return await wc.DownloadDataTaskAsync(remoteUrl);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Downloads a file from the Internet (http, https).
        /// </summary>
        /// <param name="remoteUrl"></param>
        public static byte[] DownloadFile(string remoteUrl)
        {
            using var wc = new System.Net.WebClient();

            return wc.DownloadData(remoteUrl);
        }

        /// <summary>
        /// Downloads a file from the Internet (http, https).  If an exception occurs a
        /// null will be returned.
        /// </summary>
        /// <param name="remoteUrl"></param>
        public static byte[] SafeDownloadFile(string remoteUrl)
        {
            try
            {
                using var wc = new System.Net.WebClient();

                return wc.DownloadData(remoteUrl);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Downloads a file from the Internet (http, https)
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localFileName"></param>
        public static void DownloadFile(string remoteUrl, string localFileName)
        {
            using (var wc = new System.Net.WebClient())
            {
                wc.BaseAddress = remoteUrl;
                wc.DownloadFile(remoteUrl, localFileName);
            }
        }

        /// <summary>
        /// Downloads a file from the Internet (http, https)
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localFileName"></param>
        public static async Task DownloadFileAsync(string remoteUrl, string localFileName)
        {
            using (var wc = new System.Net.WebClient())
            {
                wc.BaseAddress = remoteUrl;
                await wc.DownloadFileTaskAsync(new Uri(remoteUrl), localFileName);
            }
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
        /// Downloads a file from the Internet (http, https)
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localFileName"></param>
        /// <param name="timeout">The timeout in seconds.</param>
        public static async Task DownloadFileAsync(string remoteUrl, string localFileName, int timeout)
        {
            using (var wc = new WebClient())
            {
                wc.Timeout = timeout;
                wc.BaseAddress = remoteUrl;
                await wc.DownloadFileTaskAsync(new Uri(remoteUrl), localFileName);
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

        /// <summary>
        /// Downloads a file from the Internet with provided authentication credentials (http, https, ftp)
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localFileName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="timeout">The timeout in seconds.</param>
        public static async Task DownloadFileAsync(string remoteUrl, string localFileName, string username, string password, int timeout)
        {
            using (var wc = new WebClient())
            {
                wc.Timeout = timeout;
                wc.BaseAddress = remoteUrl;
                wc.Credentials = new NetworkCredential(username, password);
                await wc.DownloadFileTaskAsync(remoteUrl, localFileName);
            }
        }
    }
}