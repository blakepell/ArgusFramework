using System;
using System.Text;
using System.IO;

namespace Argus.IO
{

    /// <summary>
    /// Utility methods for handling logging entries to file system log files.
    /// </summary>
    public class FileLog : IDisposable
    {
        //*********************************************************************************************************************
        //
        //             Class:  FileLog
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  11/29/2010
        //     Last Modified:  03/29/2019
        //     Programmer(s):  Blake Pell
        //
        //*********************************************************************************************************************      

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logFile">The file to append to.</param>
        /// <remarks></remarks>
        public FileLog(string logFile) : this(logFile, FileMode.Append, Encoding.ASCII, true)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logFile">The file to open for writing</param>
        /// <param name="fileMode">The file mode when opened, whether to append, create new, truncate, etc.</param>
        public FileLog(string logFile, FileMode fileMode) : this(logFile, fileMode, Encoding.ASCII, true)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logFile">The file to open for writing</param>
        /// <param name="fileMode">The file mode when opened, whether to append, create new, truncate, etc.</param>
        /// <param name="encoding">The encoding to use when writing to the log file.</param>
        public FileLog(string logFile, FileMode fileMode, Encoding encoding) : this(logFile, fileMode, Encoding.ASCII, true)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logFile">The file to open for writing</param>
        /// <param name="fileMode">The file mode when opened, whether to append, create new, truncate, etc.</param>
        /// <param name="encoding">The encoding to use when writing to the log file.</param>
        /// <param name="echo">Whether or not the FileLog should echo it's output to the Console.  Echoing is ignored on the static declarations.</param>
        public FileLog(string logFile, FileMode fileMode, Encoding encoding, bool echo)
        {
            this.Encoding = encoding;
            this.LogFile = logFile;
            this.Echo = echo;
            FileStream = new FileStream(logFile, fileMode);
        }

        /// <summary>
        /// Adds an entry to a log file on the file system.
        /// </summary>
        /// <param name="msg"></param>
        /// <remarks></remarks>
        public void AddEntry(string msg)
        {
            msg = FormatLogMessage(msg);

            if (this.Echo)
            {
                Console.Write(msg);
            }

            var bytes = Encoding.ASCII.GetBytes(msg);
            FileStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Adds an entry to a log file on the file system.  This shared/static method will open the file, append to it
        /// and then close the file.  It should not be used for bulk logging, instead, instatiate this class for that.  
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logFile"></param>
        /// <remarks></remarks>
        public static void AddEntry(string msg, string logFile)
        {
            msg = FormatLogMessage(msg);
            Console.Write(msg);
            File.AppendAllText(logFile, msg, Encoding.ASCII);
        }

        /// <summary>
        /// Truncates the log file if it is over the size of the threshold (e.g. 2048 would be about 2MB).
        /// </summary>
        /// <param name="logFile">The log file to truncate if the size threshold is met.</param>
        /// <param name="thresholdInKiloBytes"></param>
        /// <returns>True if the file is truncated, False if it is not.</returns>
        /// <remarks></remarks>
        public static bool TruncateLog(string logFile, int thresholdInKiloBytes)
        {
            FileInfo fi = new FileInfo(logFile);

            if ((fi.Length / 1024) > thresholdInKiloBytes)
            {
                string msg = FormatLogMessage(string.Format("Log Truncated, greater than {0} bytes.", thresholdInKiloBytes));
                File.WriteAllText(logFile, msg, Encoding.ASCII);
                Console.Write(msg);
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Formats a log message in a consistent format.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatLogMessage(string msg)
        {
            return FormatLogMessage(msg, false);
        }

        /// <summary>
        /// Formats a log message in a consistent format.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        /// <param name="escapeTab">Whether or not to escape a tab character so it is visible in the log.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatLogMessage(string msg, bool escapeTab)
        {
            // In case data is logged it is clear what a tab character is in it.
            if (escapeTab && msg.Contains("\t"))
            {
                msg = msg.Replace("\t", "\\t");
            }

            return string.Format("{0,-10}: {1}{2}", DateTime.Now, msg, "\n");
        }

        /// <summary>
        /// Closes the underlaying FileStream and disposes of it.
        /// </summary>
        /// <remarks></remarks>
        public void Close()
        {
            if (FileStream == null)
            {
                return;
            }

            FileStream.Close();
            FileStream.Dispose();
        }

        /// <summary>
        /// The encoding to use when writing to the log file.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// The file to write to.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string LogFile { get; set; } = "";

        /// <summary>
        /// The underlying FileStream.
        /// </summary>
        public FileStream FileStream { get; }

        /// <summary>
        /// To detect redudant calls
        /// </summary>
        private bool _disposedValue = false;

        /// <summary>
        /// Whether or not the FileLog should echo it's output to the Console.  This property is ignored on the static declarations.
        /// </summary>
        public bool Echo { get; set; }

        /// <summary>
        /// Disposes of all resources in this object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this.Close();
                }

            }

            this._disposedValue = true;
        }

        /// <summary>
        /// Disposes of all resources in this object.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

}