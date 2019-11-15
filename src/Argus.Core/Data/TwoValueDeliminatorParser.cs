using System;

namespace Argus.Data
{

    /// <summary>
    /// Parse out a two value delimated list.
    /// </summary>
    /// <remarks>
    /// The value that should be parsed can be passed in via the constructor, set with the Reload method or
    /// set with an = operator if the value you're setting it equal to is a string.
    /// </remarks>
    public class TwoValueDeliminatorParser
    {
        //*********************************************************************************************************************
        //
        //             Class:  TwoValueDeliminatorParser
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  09/01/2007
        //      Last Updated:  04/18/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        private string _queryValue = "";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <remarks></remarks>
        public TwoValueDeliminatorParser(string delimitedText)
        {
            _queryValue = delimitedText;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>

        public TwoValueDeliminatorParser()
        {
        }

        /// <summary>
        /// Returns the date's short date value.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return _queryValue;
        }

        /// <summary>
        /// Provides the ability to set a TwoValueDelimitatorParse equal to a String.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static implicit operator TwoValueDeliminatorParser(string str)
        {
            return new TwoValueDeliminatorParser(str);
        }

        /// <summary>
        /// Provides the ability to set a String equal to a TwoValueDelimitatorParse.
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static implicit operator string(TwoValueDeliminatorParser dp)
        {
            return dp.ToString();
        }

        /// <summary>
        /// Reloads the value that should be parsed.
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <remarks>
        /// You can also use the = operator with a string to set this value.. e.g.
        /// <code>
        /// Dim buf As String = "Blake_Pell"
        /// Dim delim As New TwoValueDeliminatorParser
        /// delim = buf
        /// </code>
        /// </remarks>
        public void Reload(string delimitedText)
        {
            _queryValue = delimitedText;
        }

        private string _deliminator = "_";
        /// <summary>
        /// The deliminator to split the value on.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>The default value is an underscore (e.g. '_') character.</remarks>
        public string Deliminator
        {
            get { return _deliminator; }
            set { _deliminator = value; }
        }

        /// <summary>
        /// The left hand value.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Value1()
        {
            try
            {
                if (string.IsNullOrEmpty(_queryValue) == true)
                {
                    return "";
                }

                string[] buf = _queryValue.Split(Deliminator.ToCharArray());
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
        /// <returns></returns>
        /// <remarks></remarks>
        public string Value2()
        {
            try
            {
                if (string.IsNullOrEmpty(_queryValue) == true)
                {
                    return "";
                }

                string[] buf = _queryValue.Split(Deliminator.ToCharArray());
                return buf[1];
            }
            catch
            {
                return "";
            }

        }

    }

}