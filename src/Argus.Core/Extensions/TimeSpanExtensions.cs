using System;
using System.Text;

namespace Argus.Extensions
{

    public static class TimeSpanExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  TimeSpanExtensionsStd
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  10/18/2012
        //      Last Updated:  04/03/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Diplays a formatted vertical string with line breaks that contains the contents of the current TimeSpan
        /// in a human readable form.
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToVerticalString(this TimeSpan ts)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} Days{1}", ts.Days, Environment.NewLine);
            sb.AppendFormat("{0} Hours{1}", ts.Hours, Environment.NewLine);
            sb.AppendFormat("{0} Minutes{1}", ts.Minutes, Environment.NewLine);
            sb.AppendFormat("{0} Seconds{1}", ts.Seconds, Environment.NewLine);
            sb.AppendFormat("{0} Milliseconds{1}", ts.Milliseconds, Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// Multiplies the TimeSpan by the provided value.
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static TimeSpan Multiply(this TimeSpan ts, long value)
        {
            return new TimeSpan(ts.Ticks * value);
        }

        /// <summary>
        /// Divides the TimeSpan by the provided value.
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static TimeSpan Divide(this TimeSpan ts, long value)
        {
            return new TimeSpan(ts.Ticks / value);
        }

    }

}