using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Argus.Extensions;

namespace Argus.Data
{
    /// <summary>
    /// Searches a string and replaces any variables found within it with their literal value.  Variables are case sensitive.  This
    /// can be used to script parameters that may need to change to the current environment.
    /// </summary>
    /// <remarks></remarks>
    public class VariableParser
    {
        //*********************************************************************************************************************
        //
        //             Class:  VariableParser
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  07/15/2010
        //      Last Updated:  04/27/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// </remarks>
        public VariableParser()
        {
            this.VariableList.Add("@now(0)");
            this.VariableList.Add("@todayForFileSystem(0)");
            this.VariableList.Add("@getDate(0)");
            this.VariableList.Add("@timeStampSqlServerBeginOfDay(0)");
            this.VariableList.Add("@timeStampSqlServerEndOfDay(0)");
            this.VariableList.Add("@timeStampIbmDb2BeginOfDay(0)");
            this.VariableList.Add("@timeStampIbmDb2EndOfDay(0)");
            this.VariableList.Add("@monthBegin(0)");
            this.VariableList.Add("@monthEnd(0)");
            this.VariableList.Add("@calendarYearBegin(0)");
            this.VariableList.Add("@calendarYearEnd(0)");
            this.VariableList.Add("@fiscalYearBegin(0)");
            this.VariableList.Add("@fiscalYearEnd(0)");
            this.VariableList.Add("@username()");
            this.VariableList.Add("@domain()");
            this.VariableList.Add("@interactive()");
            this.VariableList.Add("@currentDirectory()");
            this.VariableList.Add("@commandLine()");
            this.VariableList.Add("@hasShutdownStarted()");
            this.VariableList.Add("@machineName()");
            this.VariableList.Add("@osVersion()");
            this.VariableList.Add("@processors()");
            this.VariableList.Add("@systemDirectory()");
            this.VariableList.Add("@guid()");
            this.VariableList.Add("@randomNumber(1,10)");
            this.VariableList.Sort();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dateOverride">The date override can be used to have the parser act like it is another day when replacing 
        /// variables with their literal values.</param>
        /// <remarks></remarks>
        public VariableParser(DateTime dateOverride) : this()
        {
            this.DateValue = dateOverride;
        }

        /// <summary>
        /// Parses the text and returns a string with the variables replaced.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Parse(string text)
        {

            text = text.Replace("@now()", "@now(0)");
            foreach (Match m in Regex.Matches(text, "@now\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, this.DateValue.AddDays(Convert.ToDouble(m.Groups["Var1"].Value)).ToString());
            }

            text = text.Replace("@todayForFileSystem()", "@todayForFileSystem(0)");
            foreach (Match m in Regex.Matches(text, "@todayForFileSystem\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, this.DateValue.AddDays(Convert.ToDouble(m.Groups["Var1"].Value)).ToString("yyyy-MM-dd"));
            }

            text = text.Replace("@getDate()", "@getDate(0)");
            foreach (Match m in Regex.Matches(text, "@getDate\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, this.DateValue.AddDays(Convert.ToDouble(m.Groups["Var1"].Value)).ToString("MM/dd/yyyy"));
            }

            text = text.Replace("@timeStampSqlServerBeginOfDay()", "@timeStampSqlServerBeginOfDay(0)");
            foreach (Match m in Regex.Matches(text, "@timeStampSqlServerBeginOfDay\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, this.DateValue.AddDays(Convert.ToDouble(m.Groups["Var1"].Value)).ToString("yyyy-MM-dd") + " 00:00:00");
            }

            text = text.Replace("@timeStampSqlServerEndOfDay()", "@timeStampSqlServerEndOfDay(0)");
            foreach (Match m in Regex.Matches(text, "@timeStampSqlServerEndOfDay\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, this.DateValue.AddDays(Convert.ToDouble(m.Groups["Var1"].Value)).ToString("yyyy-MM-dd") + " 23:59:59");
            }

            text = text.Replace("@timeStampIbmDb2BeginOfDay()", "@timeStampIbmDb2BeginOfDay(0)");
            foreach (Match m in Regex.Matches(text, "@timeStampIbmDb2BeginOfDay\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, this.DateValue.AddDays(Convert.ToDouble(m.Groups["Var1"].Value)).ToShortDateString() + "  12:00:00AM");
            }

            text = text.Replace("@timeStampIbmDb2EndOfDay()", "@timeStampIbmDb2EndOfDay(0)");
            foreach (Match m in Regex.Matches(text, "@timeStampIbmDb2EndOfDay\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, this.DateValue.AddDays(Convert.ToDouble(m.Groups["Var1"].Value)).ToShortDateString() + "  11:59:59PM");
            }

            text = text.Replace("@monthBegin()", "@monthBegin(0)");
            foreach (Match m in Regex.Matches(text, "@monthBegin\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, this.DateValue.AddMonths(Convert.ToInt32(m.Groups["Var1"].Value)).Month + "/01/" + this.DateValue.AddMonths(Convert.ToInt32(m.Groups["Var1"].Value)).Year.ToString());
            }

            text = text.Replace("@monthEnd()", "@monthEnd(0)");
            foreach (Match m in Regex.Matches(text, "@monthEnd\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, this.DateValue.AddMonths(Convert.ToInt32(m.Groups["Var1"].Value)).Month + "/" + System.DateTime.DaysInMonth(this.DateValue.AddMonths(Convert.ToInt32(m.Groups["Var1"].Value)).Year, this.DateValue.AddMonths(Convert.ToInt32(m.Groups["Var1"].Value)).Month).ToString() + "/" + this.DateValue.AddMonths(Convert.ToInt32(m.Groups["Var1"].Value)).Year.ToString());
            }

            text = text.Replace("@calendarYearBegin()", "@calendarYearBegin(0)");
            foreach (Match m in Regex.Matches(text, "@calendarYearBegin\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, "01/01/" + this.DateValue.AddYears(Convert.ToInt32(m.Groups["Var1"].Value)).Year.ToString());
            }

            text = text.Replace("@calendarYearEnd()", "@calendarYearEnd(0)");
            foreach (Match m in Regex.Matches(text, "@calendarYearEnd\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
            {
                text = text.Replace(m.Value, "12/31/" + this.DateValue.AddYears(Convert.ToInt32(m.Groups["Var1"].Value)).Year.ToString());
            }

            if (this.DateValue.Month >= 7)
            {
                // if month is July or later, the current fiscal year starts in the same year
                text = text.Replace("@fiscalYearBegin()", "@fiscalYearBegin(0)");
                foreach (Match m in Regex.Matches(text, "@fiscalYearBegin\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
                {
                    text = text.Replace(m.Value, "07/01/" + this.DateValue.AddYears(Convert.ToInt32(m.Groups["Var1"].Value)).Year.ToString());
                }

                // if month is July or later, the current fiscal year ends next year
                text = text.Replace("@fiscalYearEnd()", "@fiscalYearEnd(0)");
                foreach (Match m in Regex.Matches(text, "@fiscalYearEnd\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
                {
                    text = text.Replace(m.Value, "06/30/" + (this.DateValue.AddYears(Convert.ToInt32(m.Groups["Var1"].Value)).Year + 1).ToString());
                }
            }
            else
            {
                // if the month is before July, the current fiscal year starts the year before
                text = text.Replace("@fiscalYearBegin()", "@fiscalYearBegin(0)");
                foreach (Match m in Regex.Matches(text, "@fiscalYearBegin\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
                {
                    text = text.Replace(m.Value, "07/01/" + (this.DateValue.AddYears(Convert.ToInt32(m.Groups["Var1"].Value)).Year - 1).ToString());
                }

                // if the month is before July, the last fiscal year started 2 years ago
                text = text.Replace("@fiscalYearEnd()", "@fiscalYearEnd(0)");
                foreach (Match m in Regex.Matches(text, "@fiscalYearEnd\\(\\s*(?<Var1>[-0-9]+)\\s*\\)"))
                {
                    text = text.Replace(m.Value, "06/30/" + this.DateValue.AddYears(Convert.ToInt32(m.Groups["Var1"].Value)).Year.ToString());
                }

            }

            text = text.Replace("@username()", Environment.UserName);
            text = text.Replace("@domain()", Environment.UserDomainName);
            text = text.Replace("@interactive()", Environment.UserInteractive.ToString());
            text = text.Replace("@currentDirectory()", Environment.CurrentDirectory);
            text = text.Replace("@commandLine()", Environment.CommandLine);
            text = text.Replace("@hasShutdownStarted()", Environment.HasShutdownStarted.ToString());
            text = text.Replace("@machineName()", Environment.MachineName);
            text = text.Replace("@osVersion()", Environment.OSVersion.VersionString);
            text = text.Replace("@processors()", Environment.ProcessorCount.ToString());
            text = text.Replace("@systemDirectory()", Environment.SystemDirectory);

            if (text.Contains("@guid()") == true)
            {
                text = text.Replace("@guid()", Guid.NewGuid().ToString());
            }

            if (text.Contains("@randomNumber") == true)
            {
                // \s* is for optional white space
                foreach (Match m in Regex.Matches(text, "@randomNumber\\(\\s*(?<Var1>[-0-9]+)\\s*,\\s*(?<Var2>[-0-9]+)\\)", RegexOptions.IgnorePatternWhitespace))
                {
                    Random rnd = new Random();
                    int randomNumber = rnd.Next(Convert.ToInt32(m.Groups["Var1"].Value), Convert.ToInt32(m.Groups["Var2"].Value));
                    text = Regex.Replace(text, "@randomNumber\\(\\s*(?<Var1>[-0-9]+)\\s*,\\s*(?<Var2>[-0-9]+)\\)", randomNumber.ToString(), RegexOptions.IgnorePatternWhitespace);
                }

            }

            return text;
        }

        private List<string> _variableList = new List<string>();
        /// <summary>
        /// A list of all of the supported variables.  This can be used to bind to a list or drop down box.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<string> VariableList
        {
            get { return _variableList; }
            set { _variableList = value; }
        }

        private DateTime _dateValue = DateTime.Now;
        /// <summary>
        /// The date override can be used to have the parser act like it is another day when replacing variables with their literal values.  
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime DateValue
        {
            get { return _dateValue; }
            set { _dateValue = value; }
        }

    }

}