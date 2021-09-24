/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2011-09-29
 * @last updated      : 2021-09-24
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions to items Network related.
    /// </summary>
    public static class NetExtensions
    {
        /// <summary>
        /// Returns the filename from the local path of the System.Uri.
        /// </summary>
        /// <param name="uri"></param>
        public static string FileName(this Uri uri)
        {
            return Path.GetFileName(uri.LocalPath);
        }

        /// <summary>
        /// A check of whether the TCP/IP connection is still open by peeking.
        /// </summary>
        public static bool IsConnected(this TcpClient client)
        {
            try
            {
                if (!client.Connected)
                {
                    return false;
                }

                if (client.Client.Poll(0, SelectMode.SelectRead))
                {
                    var buf = new byte[1];

                    if (client.Client.Receive(buf, SocketFlags.Peek) == 0)
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the IP Address from the <see cref="EndPoint"/>.
        /// </summary>
        /// <param name="endPoint"></param>
        public static string ToIpAddressString(this EndPoint endPoint)
        {
            if (endPoint is IPEndPoint ipEndPoint)
            {
                return ipEndPoint.Address.ToString();
            }

            return "N/A";
        }
    }
}