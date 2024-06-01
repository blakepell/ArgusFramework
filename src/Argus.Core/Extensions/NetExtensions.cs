/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2011-09-29
 * @last updated      : 2022-08-25
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

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

                return IsConnected(client.Client);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// A hard check of whether the TCP/IP connection is still open by peeking.
        /// </summary>
        public static bool IsConnected(this Socket socket)
        {
            try
            {
                if (socket.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buff = new byte[1];
                    if (socket.Receive(buff, SocketFlags.Peek) == 0)
                    {
                        // Client disconnected
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
        public static string ToIpAddressAsString(this EndPoint endPoint)
        {
            if (endPoint is IPEndPoint ipEndPoint)
            {
                return ipEndPoint.Address.ToString();
            }

            return "N/A";
        }

        /// <summary>
        /// Returns the IP address from the provided <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        public static string IpAddressAsString(this Socket? socket)
        {
            try
            {
                if (socket?.Connected == false)
                {
                    return "N/A";
                }

                return socket?.RemoteEndPoint?.ToString() ?? "N/A";
            }
            catch
            {
                return "N/A";
            }
        }
    }
}