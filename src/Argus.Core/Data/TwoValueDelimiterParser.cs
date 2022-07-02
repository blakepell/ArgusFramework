/*
 * @author            : Blake Pell
 * @initial date      : 2005-09-01
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace Argus.Data
{

    /// <summary>
    /// Parse out a two value delimited list.
    /// </summary>
    /// <remarks>
    /// The value that should be parsed can be passed in via the constructor, set with the Reload method or
    /// set with an = operator if the value you're setting it equal to is a string.  This by design will split
    /// a string like it's a key pair and not more values.
    /// </remarks>
    public class TwoValueDelimiterParser
    {
        private string _queryValue = "";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="delimitedText"></param>
        public TwoValueDelimiterParser(string delimitedText)
        {
            _queryValue = delimitedText;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TwoValueDelimiterParser()
        {
        }

        /// <summary>
        /// Returns the date's short date value.
        /// </summary>
        public override string ToString()
        {
            return _queryValue;
        }

        /// <summary>
        /// Provides the ability to set a TwoValueDelimiterParser equal to a String.
        /// </summary>
        /// <param name="str"></param>
        public static implicit operator TwoValueDelimiterParser(string str)
        {
            return new TwoValueDelimiterParser(str);
        }

        /// <summary>
        /// Provides the ability to set a String equal to a TwoValueDelimiterParser.
        /// </summary>
        /// <param name="dp"></param>
        public static implicit operator string(TwoValueDelimiterParser dp)
        {
            return dp.ToString();
        }

        /// <summary>
        /// Reloads the value that should be parsed.
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <remarks>
        /// You can also use the = operator with a string to set this value.. e.g.
        /// </remarks>
        public void Reload(string delimitedText)
        {
            _queryValue = delimitedText;
        }

        /// <summary>
        /// The deliminator to split the value on.
        /// </summary>
        /// <remarks>The default value is an underscore (e.g. '_') character.</remarks>
        public string Delimiter { get; set; } = "_";

        /// <summary>
        /// The left hand value.
        /// </summary>
        public string Value1()
        {
            try
            {
                if (string.IsNullOrEmpty(_queryValue))
                {
                    return "";
                }

                string[] buf = _queryValue.Split(this.Delimiter.ToCharArray());
                return buf[0];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// The right hand value.
        /// </summary>
        public string Value2()
        {
            try
            {
                if (string.IsNullOrEmpty(_queryValue))
                {
                    return "";
                }

                string[] buf = _queryValue.Split(this.Delimiter.ToCharArray());
                return buf[1];
            }
            catch
            {
                return "";
            }
        }
    }
}