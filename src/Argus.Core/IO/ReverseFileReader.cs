/*
 * @author            : Blake Pell
 * @initial date      : 2010-11-29
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System;
using System.IO;
using System.Text;

namespace Argus.IO
{
    /// <summary>
    /// Reads lines from a file or a Stream in reverse order one line at a time.
    /// </summary>
    public class ReverseFileReader : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// The stream object to read backwards line by line.
        /// </summary>
        private Stream _stream;

        /// <summary>
        /// Opens a Stream as a FileStream.  This will work most places except Windows Universal/UWP apps.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public ReverseFileReader(string path)
        {
            _stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            _stream.Seek(0, SeekOrigin.End);
        }

        /// <summary>
        /// Accepts a Stream to read backwards.
        /// </summary>
        /// <param name="s"></param>
        public ReverseFileReader(Stream s)
        {
            _stream = s;
            _stream.Seek(0, SeekOrigin.End);
        }

        /// <summary>
        /// Opens the file as a FileStream.  This will work most places except Windows Universal/UWP apps.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="encoding">The encoding that should be used when reading the stream.</param>
        public ReverseFileReader(string path, Encoding encoding)
        {
            _stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            _stream.Seek(0, SeekOrigin.End);
            this.Encoding = encoding;
        }

        /// <summary>
        /// Line Endings of the Stream to be read.
        /// </summary>
        public LineEnding LineEnding { get; set; } = LineEnding.CrLf;

        /// <summary>
        /// The encoding to use when reading the file.
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Whether or not the start of file has been reached.
        /// </summary>
        public bool StartOfFile => _stream.Position == 0;

        /// <summary>
        /// Whether the underlying stream is at the end.
        /// </summary>
        public bool EndOfFile => _stream.Position == _stream.Length;

        /// <summary>
        /// Closes and disposes of resources.  The underlying Stream whether passed in
        /// or created here is Disposed of.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Reads the next line in from the end looking for the specified carriage return and/or line feed combination.
        /// </summary>
        public string ReadLine()
        {
            switch (this.LineEnding)
            {
                case LineEnding.CrLf:
                    return this.ReadLineCrLf();
                case LineEnding.Lf:
                    return this.ReadLine('\n');
                case LineEnding.Cr:
                    return this.ReadLine('\r');
                default:
                    throw new Exception("Unknown LineEnding specified.");
            }
        }

        /// <summary>
        /// Reads the file line backwards looking for a carriage return/line feed combination.
        /// </summary>
        private string ReadLineCrLf()
        {
            var buf = new byte[1];
            long position = _stream.Position;

            while (_stream.Position > 0)
            {
                buf.Initialize();

                _stream.Seek(-1, SeekOrigin.Current);

                // Read one char
                _stream.Read(buf, 0, 1);

                // Move it back
                _stream.Seek(-1, SeekOrigin.Current);

                if (buf[0] == 10)
                {
                    // Move it back again.
                    _stream.Seek(-1, SeekOrigin.Current);

                    _stream.Read(buf, 0, 1);

                    if (buf[0] == 13)
                    {
                        break;
                    }
                }
            }

            int count = (int) (position - _stream.Position);
            var line = new byte[count];

            _stream.Read(line, 0, count);
            _stream.Seek(-count, SeekOrigin.Current);

            return this.Encoding.GetString(line).Trim('\r', '\n');
        }

        /// <summary>
        /// Reads the file backwards looking for either a carriage return or line feed as it is specified
        /// in the line ending property.  It will only get here if one of those has been set.
        /// </summary>
        private string ReadLine(char lineEnding)
        {
            var buf = new byte[1];
            long position = _stream.Position;

            while (_stream.Position > 0)
            {
                buf.Initialize();

                // Go back one
                _stream.Seek(-1, SeekOrigin.Current);

                // Read one char forward
                _stream.Read(buf, 0, 1);

                // Go back one char
                _stream.Seek(-1, SeekOrigin.Current);

                // If it's a link ending then proceed to the snarfing.
                if (buf[0] == lineEnding)
                {
                    break;
                }
            }

            int count = (int) (position - _stream.Position);
            var line = new byte[count];

            _stream.Read(line, 0, count);
            _stream.Seek(-count, SeekOrigin.Current);

            return this.Encoding.GetString(line).TrimStart(lineEnding);
        }

        /// <summary>
        /// The percentage of the Stream the reader has read through rounded to two decimal places.
        /// </summary>
        /// <returns>A decimal value between 1 and 100.</returns>
        public decimal PercentComplete()
        {
            // No divide by zero stuff
            if (_stream.Length == 0)
            {
                return 100;
            }

            // Because we're going in reverse order this calculation needs to re-calculate the
            // position so it goes the right direction.
            long position = _stream.Length - _stream.Position;

            return System.Math.Round((decimal) position / _stream.Length * 100, 2);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _stream?.Close();
                _stream?.Dispose();
                _stream = null;
            }

            _disposed = true;
        }
    }
}