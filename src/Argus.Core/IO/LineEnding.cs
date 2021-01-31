/*
 * @author            : Blake Pell
 * @initial date      : 2009-04-06
 * @last updated      : 2019-04-06
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */
 
namespace Argus.IO
{
    /// <summary>
    /// Line endings for the ReverseFileReader supports.
    /// </summary>
    public enum LineEnding
    {
        /// <summary>
        /// Carriage Return (character 13) and Line Feed (character 10)
        /// </summary>
        CrLf,

        /// <summary>
        /// Line feed (character 10)
        /// </summary>
        Lf,

        /// <summary>
        /// Carriage return (character 13)
        /// </summary>
        Cr
    }
}