using System;

namespace Argus.Diagnostics
{
    /// <summary>
    ///     A portable replacement for the .NET Stopwatch class that is not provided in all versions of the framework.
    /// </summary>
    public class Stopwatch
    {
        //*********************************************************************************************************************
        //
        //             Class:  Stopwatch
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  10/05/2012
        //      Last Updated:  04/05/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     The elapsed number of ticks.
        /// </summary>
        private long _elapsedTicks;

        /// <summary>
        ///     The time the Stopwatch started.
        /// </summary>
        private long _startTimestamp;

        /// <summary>
        ///     The time elapsed as a TimeSpan that the Stopwatch was in the running state.
        /// </summary>
        public TimeSpan Elapsed => new TimeSpan(this.ElapsedTicks);

        /// <summary>
        ///     The time elapsed in milliseconds that the Stopwatch was in the running state.
        /// </summary>
        public long ElapsedMilliseconds => this.ElapsedTicks / TimeSpan.TicksPerMillisecond;

        /// <summary>
        ///     The time elapsed in Ticks that the Stopwatch was in the running state.
        /// </summary>
        /// <value></value>
        public long ElapsedTicks
        {
            get
            {
                long et = _elapsedTicks;

                if (this.IsRunning)
                {
                    et += this.ElapsedTicksSinceLastStart;
                }

                return et;
            }
        }

        /// <summary>
        ///     Whether the Stopwatch is currently running or not.
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        ///     The elapsed ticks since the Stopwatch was last started.
        /// </summary>
        private long ElapsedTicksSinceLastStart => DateTime.UtcNow.Ticks - _startTimestamp;

        /// <summary>
        ///     Starts the Stopwatch.
        /// </summary>
        public void Start()
        {
            if (!this.IsRunning)
            {
                _startTimestamp = DateTime.UtcNow.Ticks;
                this.IsRunning = true;
            }
        }

        /// <summary>
        ///     Stops the Stopwatch.
        /// </summary>
        public void Stop()
        {
            if (this.IsRunning)
            {
                _elapsedTicks += this.ElapsedTicksSinceLastStart;
                this.IsRunning = false;
            }
        }

        /// <summary>
        ///     Resets and then starts the Stopwatch.
        /// </summary>
        public void Restart()
        {
            this.Reset();
            this.Start();
        }

        /// <summary>
        ///     Resets and stops the Stopwatch
        /// </summary>
        public void Reset()
        {
            _startTimestamp = 0;
            _elapsedTicks = 0;
            this.IsRunning = false;
        }
    }
}