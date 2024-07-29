/*
 * @author            : Blake Pell
 * @initial date      : 2009-04-07
 * @last updated      : 2024-07-28
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace Argus.Network
{
    /// <summary>
    /// Network utilities for Windows.
    /// </summary>
    public static class Utilities
    {
        #if NET5_0_OR_GREATER
        /// <summary>
        /// Checks an IP Address to see if a port is open.
        /// </summary>
        /// <param name="ipAddress">The IP Address</param>
        /// <param name="port">The port number.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        public static async Task<bool> IsPortOpen(string ipAddress, int port, int timeout = 100)
        {
            using (var client = new TcpClient())
            {
                using (var cts = new CancellationTokenSource(timeout))
                {
                    try
                    {
                        await client.ConnectAsync(ipAddress, port, cts.Token);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }
        #endif
        
        /// <summary>
        /// Loops over the 255 addresses in the same segment as the provided IP Address (192.168.1.x)
        /// </summary>
        /// <param name="ip"></param>
        public static IEnumerable<IPAddress> IPv4Segments(string ip)
        {
            return IPv4Segments(IPAddress.Parse(ip));
        }
        
        /// <summary>
        /// Loops over the 255 addresses in the same segment as the provided IP Address (192.168.1.x)
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IEnumerable<IPAddress> IPv4Segments(IPAddress ip)
        {
            byte[] bytes = ip.GetAddressBytes();
            if (bytes.Length == 4)
            {
                for (int i = 1; i <= 255; i++)
                {
                    bytes[3] = (byte)i;
                    yield return new IPAddress(bytes);
                }
            }
        }        

        /// <summary>
        /// Loops over the 255 addresses in the same segment as the provided IP Address (192.168.x.x)
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IEnumerable<IPAddress> IPv4SegmentsSubnets(string ip)
        {
            return IPv4SegmentsSubnets(IPAddress.Parse(ip));
        }

        /// <summary>
        /// Loops over the 255 addresses in the same segment as the provided IP Address (192.168.x.x)
        /// </summary>
        /// <param name="ip"></param>
        public static IEnumerable<IPAddress> IPv4SegmentsSubnets(IPAddress ip)
        {
            byte[] bytes = ip.GetAddressBytes();
            if (bytes.Length == 4)
            {
                for (int subnet = 1; subnet <= 255; subnet++)
                {
                    bytes[2] = (byte)subnet;
                    
                    for (int i = 1; i <= 255; i++)
                    {
                        bytes[3] = (byte)i;
                        yield return new IPAddress(bytes);
                    }
                }
            }
        }        

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        /// <summary>
        /// If the Internet is currently available.  Not available on non-windows workstations.
        /// </summary>
        public static bool IsInternetAvailable()
        {
            return InternetGetConnectedState(out int _, 0);
        }

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

        /// <summary>
        /// Downloads a file from the Internet with provided authentication credentials (http, https, ftp) and HTTP request headers.
        /// </summary>
        /// <param name="remoteUrl">The URL of the remote file to download.</param>
        /// <param name="localFileName">The local path where the file should be saved.</param>
        /// <param name="username">The username for authentication. Can be null if no authentication is needed.</param>
        /// <param name="password">The password for authentication. Can be null if no authentication is needed.</param>
        /// <param name="timeout">The timeout in seconds for the request. Can be null for the default timeout.</param>
        /// <param name="headers">A collection of HTTP headers to send with the request. Can be null if no additional headers are needed.</param>
        public static void DownloadFile(string remoteUrl, string localFileName, string? username, string? password, int? timeout, NameValueCollection? headers)
        {
            using (var wc = new WebClient())
            {
                if (timeout != null)
                {
                    wc.Timeout = timeout.Value;
                }
                
                wc.BaseAddress = remoteUrl;

                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    wc.Credentials = new NetworkCredential(username, password);
                }

                if (headers != null)
                {
                    foreach (string key in headers.AllKeys)
                    {
                        wc.Headers.Add(key, headers[key]);
                    }
                }
                
                wc.DownloadFile(remoteUrl, localFileName);
            }
        }

        /// <summary>
        /// Async downloads a file from the Internet with provided authentication credentials (http, https, ftp) and HTTP request headers.
        /// </summary>
        /// <param name="remoteUrl">The URL of the remote file to download.</param>
        /// <param name="localFileName">The local path where the file should be saved.</param>
        /// <param name="username">The username for authentication. Can be null if no authentication is needed.</param>
        /// <param name="password">The password for authentication. Can be null if no authentication is needed.</param>
        /// <param name="timeout">The timeout in seconds for the request. Can be null for the default timeout.</param>
        /// <param name="headers">A collection of HTTP headers to send with the request. Can be null if no additional headers are needed.</param>
        /// <returns>A Task representing the asynchronous download operation.</returns>
        public static async Task DownloadFileAsync(string remoteUrl, string localFileName, string? username, string? password, int? timeout, NameValueCollection? headers)
        {
            using (var wc = new WebClient())
            {
                if (timeout != null)
                {
                    wc.Timeout = timeout.Value;
                }
                
                wc.BaseAddress = remoteUrl;

                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    wc.Credentials = new NetworkCredential(username, password);
                }

                if (headers != null)
                {
                    foreach (string key in headers.AllKeys)
                    {
                        wc.Headers.Add(key, headers[key]);
                    }
                }
                
                await wc.DownloadFileTaskAsync(remoteUrl, localFileName);                
            }
        }
    }
}