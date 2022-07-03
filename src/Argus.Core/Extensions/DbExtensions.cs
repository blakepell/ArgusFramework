/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2008-05-07
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Data.Common;
using System.Threading.Tasks;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions of interfaces and base classes in the <see cref="System.Data"/> namespace.
    /// </summary>
    public static class DbExtensions
    {
        /// <summary>
        /// Opens a database connection.  This will attempt to retry a specified number of times (the default
        /// is 3).  If the connection is open it will return but if it is Connecting, Broken or Fetching it will
        /// attempt to Close and then re-establish the connection.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="retries"></param>
        public static void OpenWithRetry(IDbConnection conn, int retries = 3)
        {
            if (conn == null)
            {
                throw new Exception("The connection object is null.");
            }

            switch (conn.State)
            {
                case ConnectionState.Open:
                    return;
                case ConnectionState.Connecting:
                case ConnectionState.Broken:
                case ConnectionState.Fetching:
                case ConnectionState.Executing:
                    conn.Close();

                    break;
            }

            int tries = 0;

            while (tries <= retries)
            {
                tries++;

                try
                {
                    conn.Open();

                    return;
                }
                catch
                {
                    // Throw the exception on the third try
                    if (tries >= 3)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Opens a database connection.  This will attempt to retry a specified number of times (the default
        /// is 3).  If the connection is open it will return but if it is Connecting, Broken or Fetching it will
        /// attempt to Close and then re-establish the connection.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="retries"></param>
        public static async Task OpenWithRetryAsync(DbConnection conn, int retries = 3)
        {
            if (conn == null)
            {
                throw new Exception("The connection object is null.");
            }

            switch (conn.State)
            {
                case ConnectionState.Open:
                    return;
                case ConnectionState.Connecting:
                case ConnectionState.Broken:
                case ConnectionState.Fetching:
                case ConnectionState.Executing:
                    await conn.CloseAsync();
                    break;
            }

            int tries = 0;

            while (tries <= retries)
            {
                tries++;

                try
                {
                    await conn.OpenAsync();
                    return;
                }
                catch
                {
                    // Throw the exception on the third try
                    if (tries >= 3)
                    {
                        throw;
                    }
                }
            }
        }

    }
}