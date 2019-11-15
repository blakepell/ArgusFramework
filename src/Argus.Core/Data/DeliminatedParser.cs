using System.Collections.Generic;
using System.Linq;

namespace Argus.Data
{

    /// <summary>
    /// Class with shared methods to aid in parsing a delimated value list
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class DeliminatedParser
    {
        //*********************************************************************************************************************
        //
        //             Class:  DeliminatedParser
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  09/21/2007
        //      Last Updated:  04/06/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Parses the text and splits it by the delimiter.  
        /// </summary>
        /// <remarks></remarks>
        public static string[] ParseToArray(string text, string delimiter)
        {
            return text.Split(delimiter.ToCharArray());
        }

        /// <summary>
        /// Parses the text and splits it by the delimiter.
        /// </summary>
        /// <remarks></remarks>
        public static List<string> ParseToList(string text, string delimiter)
        {
            return text.Split(delimiter.ToCharArray()).ToList();
        }

    }

}