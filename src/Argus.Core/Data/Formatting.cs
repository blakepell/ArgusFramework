using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Argus.Extensions;

namespace Argus.Data
{
    /// <summary>
    ///     Various formatting procedures.
    /// </summary>
    public class Formatting
    {
        //*********************************************************************************************************************
        //
        //             Class:  Formatting
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  04/02/2008
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Formats a number without decimal places.  If the string is not numeric it's full contents
        ///     will be returned.  This will return no decimal places.
        /// </summary>
        /// <param name="str"></param>
        public static string FormatNumber(string str)
        {
            if (str.IsNumeric())
            {
                double x = Convert.ToDouble(str);

                return x.ToString("#,0");
            }

            return str;
        }

        /// <summary>
        ///     Formats a number with commas and the specified number of decimal places.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="decimalPlaces"></param>
        public static string FormatNumber(string str, int decimalPlaces)
        {
            if (str.IsNumeric())
            {
                string formatString = "#,0.";

                for (int i = 0; i < decimalPlaces; i++)
                {
                    formatString += "0";
                }

                double x = Convert.ToDouble(str);

                return x.ToString(formatString);
            }

            return str;
        }

        /// <summary>
        ///     Formats a zip code according to the possible different formats (5 or 9 digits)
        /// </summary>
        /// <param name="zipCode"></param>
        public static string FormatZipCode(string zipCode)
        {
            zipCode = zipCode.Replace(" ", "").Replace("-", "");

            switch (zipCode.Length)
            {
                case 0:
                    // they didn't give us anything, so send it back to them.
                    return "";
                case 5:
                    // just 5, return the zip code
                    return zipCode;
                case 9:
                    // add the -
                    return $"{zipCode.Left(5)}-{zipCode.Right(4)}";
                case 10:
                    // already has the -
                    return zipCode;
                default:
                    return zipCode;
            }
        }

        /// <summary>
        ///     Formats phone numbers with dashes.  Supported formats are 8560114, 8128560114, 18128560114.
        /// </summary>
        /// <param name="phoneNumber"></param>
        public static string FormatPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace(" ", "");

            switch (phoneNumber.Length)
            {
                case 0:
                    return "";
                case 7:
                    // 8560114
                    return $"{phoneNumber.Left(3)}-{phoneNumber.Right(4)}";
                case 10:
                    // 8128560114
                    return $"{phoneNumber.Left(3)}-{phoneNumber.Substring(3, 3)}-{phoneNumber.Right(4)}";
                case 11:
                    // 18128560114 or 18005555555
                    return $"{phoneNumber.Left(1)}-{phoneNumber.Mid(2, 3)}-{phoneNumber.Mid(5, 3)}-{phoneNumber.Right(4)}";
                case 12:
                    // Already has the -
                    return phoneNumber;
                default:
                    return phoneNumber;
            }
        }

        /// <summary>
        ///     Returns an Html string for the passed in Url as string.
        /// </summary>
        /// <param name="url">The link address for the link.</param>
        /// <param name="openInNewWindow">Whether the link should open up in a new window or not</param>
        /// <param name="forceHttpFormat">Whether or not the function should cleanup and format an Http address specifically to add missing pieces.</param>
        /// <remarks>The ForceHttpFormat option is there to account for bad data in our databases where the full Url wasn't included.</remarks>
        public static string ReturnLinkHtml(string url, bool openInNewWindow, bool forceHttpFormat)
        {
            var sb = new StringBuilder();

            if (forceHttpFormat)
            {
                if ((url.Contains("www.") == false) & (url.Contains("http://") == false))
                {
                    // No www. and No http://
                    url = "http://www." + url;
                }
                else if ((url.Contains("http://") == false) & url.Contains("www."))
                {
                    // Contains www, but not http://
                    url = "http://" + url;
                }
            }

            if (openInNewWindow)
            {
                sb.AppendFormat("<a href=\"{0}\" target=\"_blank\">{1}</a>", url, url);
            }
            else
            {
                sb.AppendFormat("<a href=\"{0}\">{1}</a>", url, url);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Returns an Html string for the passed in Url as string.  The DisplayText variable is what will be displayed on the screen.
        /// </summary>
        /// <param name="url">The link address for the link.</param>
        /// <param name="displayText">The text that should be displayed for the link.</param>
        /// <param name="openInNewWindow">Whether the link should open up in a new window or not</param>
        /// <param name="forceHttpFormat">Whether or not the function should cleanup and format an Http address specifically to add missing pieces.</param>
        /// <remarks>The ForceHttpFormat option is there to account for bad data in our databases where the full Url wasn't included.</remarks>
        public static string ReturnLinkHtml(string url, string displayText, bool openInNewWindow, bool forceHttpFormat)
        {
            var sb = new StringBuilder();

            if (forceHttpFormat)
            {
                if ((url.Contains("www.") == false) & (url.Contains("http://") == false))
                {
                    // No www. and No http://
                    url = "http://www." + url;
                }
                else if ((url.Contains("http://") == false) & url.Contains("www."))
                {
                    // Contains www, but not http://
                    url = "http://" + url;
                }
            }

            if (openInNewWindow)
            {
                sb.AppendFormat("<a href=\"{0}\" target=\"_blank\">{1}</a>", url, displayText);
            }
            else
            {
                sb.AppendFormat("<a href=\"{0}\">{1}</a>", url, displayText);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Returns an Html string for the passed in Url as a Uri format object.
        /// </summary>
        /// <param name="url">The link address for the link.</param>
        /// <param name="openInNewWindow">Whether the link should open up in a new window or not</param>
        public static string ReturnLinkHtml(Uri url, bool openInNewWindow)
        {
            var sb = new StringBuilder();

            if (openInNewWindow)
            {
                sb.AppendFormat("<a href=\"{0}\" target=\"_blank\">{1}</a>", url, url);
            }
            else
            {
                sb.AppendFormat("<a href=\"{0}\">{1}</a>", url, url);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Returns an Html string for the passed in Url as a Uri format object.  The DisplayText variable is what will be displayed on the screen.
        /// </summary>
        /// <param name="url">The link address for the link.</param>
        /// <param name="displayText">The text that should be displayed for the link.</param>
        /// <param name="openInNewWindow">Whether the link should open up in a new window or not</param>
        public static string ReturnLinkHtml(Uri url, string displayText, bool openInNewWindow)
        {
            var sb = new StringBuilder();

            if (openInNewWindow)
            {
                sb.AppendFormat("<a href=\"{0}\" target=\"_blank\">{1}</a>", url, displayText);
            }
            else
            {
                sb.AppendFormat("<a href=\"{0}\">{1}</a>", url, displayText);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Returns and Html mail link for the passed in e-mail address as a string.
        /// </summary>
        /// <param name="emailAddress"></param>
        public static string ReturnMailLink(string emailAddress)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<a href=\"mailto:{0}\">{1}</a>", emailAddress, emailAddress);

            return sb.ToString();
        }

        /// <summary>
        ///     Returns and Html mail link for the passed in e-mail address as a string.  The DisplayText variable is what will be displayed on the screen.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="displayText"></param>
        public static string ReturnMailLink(string emailAddress, string displayText)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<a href=\"mailto:{0}\">{1}</a>", emailAddress, displayText);

            return sb.ToString();
        }

        /// <summary>
        ///     Format the dollars with 2 decimal places, commas to group digits and parenthesis for negative amounts.
        /// </summary>
        /// <param name="amount"></param>
        public static string FormatDollars(double amount)
        {
            return amount.ToString("C2");
        }

        /// <summary>
        ///     Format the dollars with 2 decimal places, commas to group digits and parenthesis for negative amounts.
        /// </summary>
        /// <param name="amount"></param>
        public static string FormatDollars(string amount)
        {
            // IsNumeric takes into account if a $ is already in the string (and commas, stuff like that involved with the number)
            if (!amount.IsNumeric())
            {
                return amount;
            }

            return FormatDollars(Convert.ToDouble(amount));
        }

        /// <summary>
        ///     Formats a percent with a % sign, - sign for negatives and groups digits
        /// </summary>
        /// <param name="value"></param>
        public static string FormatPercent(double value, int decimalPlaces)
        {
            return value.ToString("N" + decimalPlaces);
        }

        /// <summary>
        ///     Formats a percent with a % sign, - sign for negatives and groups digits
        /// </summary>
        /// <param name="value"></param>
        public static string FormatPercent(string value, int decimalPlaces)
        {
            value = value.Replace("%", "");

            // IsNumeric does not take into account a percent sign that might exist so we've stripped it out before we
            // validate the string as a number.
            if (!value.IsNumeric())
            {
                return value;
            }

            return FormatPercent(Convert.ToDouble(value), decimalPlaces);
        }

        /// <summary>
        ///     Formats a file size if the length (total number of bytes) are passed in.  This will list sizes up
        ///     to and including terabytes.
        /// </summary>
        /// <param name="length"></param>
        public static string FormattedFileSize(double length)
        {
            string[] sizes =
            {
                "B",
                "KB",
                "MB",
                "GB",
                "TB",
                "PB"
            };

            int order = 0;

            while ((length > 1024) & (order + 1 < sizes.Length))
            {
                order += 1;
                length = length / 1024;
            }

            return $"{length:0.##} {sizes[order]}";
        }

        /// <summary>
        ///     Strip HTML from a string.
        /// </summary>
        /// <param name="value"></param>
        public static string StripHtml(string value)
        {
            return Regex.Replace(value, "<(.|\\n)*?>", string.Empty);
        }

        /// <summary>
        ///     Strip HTML from a string, 2nd method.
        /// </summary>
        /// <param name="value"></param>
        public static string StripHtml2(string value)
        {
            // Removes tags from passed HTML
            return Regex.Replace(value, "<[^>]*>", string.Empty);
        }

        /// <summary>
        ///     Converts a CamelCase string to Title Case
        /// </summary>
        /// <param name="text"></param>
        /// <remarks>Example: 'incomeCashAmount' would become 'Income Cash Amount'</remarks>
        public static string CamelCaseToTitleCase(string text)
        {
            text = text.Substring(0, 1).ToUpper() + text.Substring(1);

            return Regex.Replace(text, "(\\B[A-Z])", " $1");
        }

        /// <summary>
        ///     Removes the white space from CSS code.  This can be used to speed the rate at which CSS files transfer over
        ///     the network.
        /// </summary>
        /// <param name="css"></param>
        public static object StripWhitespaceFromCss(string css)
        {
            css = css.Replace("  ", "");
            css = css.Replace(Environment.NewLine, "");
            css = css.Replace("\\t", "");
            css = css.Replace(" {", "{");
            css = css.Replace(" :", ":");
            css = css.Replace(": ", ":");
            css = css.Replace(", ", ",");
            css = css.Replace("; ", ";");
            css = css.Replace(";}", "}");
            css = Regex.Replace(css, "(?<=[>])\\s{2,}(?=[<])|(?<=[>])\\s{2,}(?=&nbsp;)|(?<=&ndsp;)\\s{2,}(?=[<])", string.Empty);

            return css;
        }

        /// <summary>
        ///     Returns a list of capitalized words in a sentence
        /// </summary>
        /// <param name="str"></param>
        public static List<string> ReturnCapitalizedWords(string str)
        {
            var strList = new List<string>();

            var mc = Regex.Matches(str, "[A-Z][a-z]*");

            foreach (Match m in mc)
            {
                strList.Add(m.ToString());
            }

            return strList;
        }

        /// <summary>
        ///     Removes any characters other than numeric digits from a string
        /// </summary>
        /// <param name="buf"></param>
        public static string CleanupNumber(string buf)
        {
            string temp = buf;

            foreach (char c in buf)
            {
                switch (c)
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '0':
                        break;
                    // Do nothing
                    default:
                        temp = temp.Replace(c.ToString(), "");

                        break;
                }
            }

            return temp;
        }
    }
}