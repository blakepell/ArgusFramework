using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;

namespace Argus.Network.Authentication
{
    /// <summary>
    /// Shared methods for validating and managing user credentials against an LDAP provider.
    /// </summary>
    public class LdapAuthentication
    {

        //*********************************************************************************************************************
        //
        //             Class:  AdsAuthentication
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  Unknown
        //      Last Updated:  06/03/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

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
                        return (results != null);
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
                        entry.Close();
                        entry.Dispose();
                        searcher.Dispose();
                    }

                }
            }

        }

    }

}