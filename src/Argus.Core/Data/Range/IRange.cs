using System.Collections.Generic;
using System.Text;

namespace Argus.Data.Range
{
    //*********************************************************************************************************************
    //
    //         Namespace:  Range
    //      Organization:  http://www.blakepell.com        
    //      Initial Date:  06/26/2012
    //      Last Updated:  04/08/2016
    //     Programmer(s):  Blake Pell, blakepell@hotmail.com
    //
    //*********************************************************************************************************************

    /// <summary>
    /// Range interface that specifies the properties and methods that range's should carry.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks></remarks>
    public interface IRange<T>
    {

        /// <summary>
        /// The start element in the range
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>

        T Start { get; set; }
        /// <summary>
        /// The ending element in the range
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>

        T End { get; set; }
        /// <summary>
        /// A list that includes all elements in the range.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        List<T> ToList();

    }

}