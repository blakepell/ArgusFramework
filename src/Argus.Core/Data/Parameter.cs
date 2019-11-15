using System;
using Argus.Extensions;

namespace Argus.Data
{

    /// <summary>
    /// Contains a string name/value pair seperated by an equals sign.  The values can be set via the properties or by passing in a
    /// delimited string.  E.g. FirstName=Blake.
    /// </summary>
    /// <remarks></remarks>
    public class Parameter
    {
        //*********************************************************************************************************************
        //
        //             Class:  Parameter
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  12/19/2012
        //      Last Updated:  04/07/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>

        public Parameter()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <remarks></remarks>
        public Parameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Returns the name/value pair in string format.  E.g. FirstName=Blake
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return $"{this.Name}={this.Value}";
        }

        /// <summary>
        /// Sets the parameter based off of a passed in string via an operator.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static implicit operator Parameter(string str)
        {
            if (string.IsNullOrWhiteSpace(str) == true)
            {
                throw new Exception("The a valid parameter string.  A value parameter string includes a name/value pair seperated by an equals sign.");
            }

            if (str.Contains("=") == false)
            {
                throw new Exception("The a valid parameter string.  A value parameter string includes a name/value pair seperated by an equals sign.");
            }

            // Since there was an equals sign, we will have a name/value pair.
            string[] items = str.SplitPcl("=");

            return new Parameter(items[0], items[1]);
        }

        /// <summary>
        /// Sets the string based off of the current value of the parameter.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static implicit operator string(Parameter p)
        {
            return $"{p.Name}={p.Value}";
        }

        public string Name { get; set; }
        public string Value { get; set; }

    }

}