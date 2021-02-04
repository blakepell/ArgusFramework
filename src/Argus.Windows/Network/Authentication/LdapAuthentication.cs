/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : Unknown
 * @last updated      : 2016-06-03
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.DirectoryServices;

namespace Argus.Network.Authentication
{
    /// <summary>
    /// Shared methods for validating and managing user credentials against an LDAP provider.
    /// </summary>
    public class LdapAuthentication
    {
        /// <summary>
        /// Validates user credentials via the managed Active Directory objects by trying to run a simple query against the AD.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static bool ValidateLogin(string domain, string username, string password)
        {
            using (var entry = new DirectoryEntry("LDAP://" + domain, username, password))
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.SearchScope = SearchScope.OneLevel;

                    try
                    {
                        var results = searcher.FindOne();

                        return results != null;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }
    }
}