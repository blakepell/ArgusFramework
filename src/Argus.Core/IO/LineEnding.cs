namespace Argus.IO
{
    //*********************************************************************************************************************
    //
    //              Enum:  LineEnding
    //      Organization:  http://www.blakepell.com  
    //      Initial Date:  04/06/2019
    //     Last Modified:  04/06/2019
    //     Programmer(s):  Blake Pell, blakepell@hotmail.com
    //
    //*********************************************************************************************************************      

    /// <summary>
    /// Line endings the ReverseFileReader supports.
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
    };

}