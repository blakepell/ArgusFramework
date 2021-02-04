/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2013-07-26
 * @last updated      : 2020-12-03
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Argus.Audio.NAudio
{
    /// <summary>
    /// A wrapper for the WasapiLoopbackCapture that will implement basic recording to a file that is overwrite only.
    /// </summary>
    /// <remarks>
    /// This was originally from a blog post I wrote on 7/26/2013.  I had recently noticed that the code provided in that
    /// post had made it's way into 25+ GitHub repositories.  Over the years I have cleaned up and expanded on the code
    /// provided in that post and seeing that it's being used I felt sharing it as a library / Nuget package might be useful
    /// for some folks.  In particular this version allows for the setting of the AudioDevice (not just the default) as well
    /// has the ability to get to get an audio level from the right and level channels (which you can use to determine if
    /// anything is playing or if there is silence).  The original blog post exists at:
    /// https://www.blakepell.com/2013-07-26-naudio-loopback-record-what-you-hear-through-the-speaker
    /// </remarks>
    public class LoopbackRecorder : IDisposable
    {
        /// <summary>
        /// Private variable for the <see cref="AudioDevice" /> property.
        /// </summary>
        private MMDevice _audioDevice;

        /// <summary>
        /// Used to check redundant calls to <see cref="Dispose" />.
        /// </summary>
        private bool _disposed;

        private IWaveIn _waveIn;
        private WaveFileWriter _writer;

        /// <summary>
        /// Constructor.  The default device is setup and used as the initial recording device.
        /// </summary>
        public LoopbackRecorder()
        {
            try
            {
                // We're going to use this to detect the default device.
                this.Devices = new MMDeviceEnumerator();
                this.AudioDevice = this.Devices.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Constructor.  Allows for setting up the initial recording device or skipping that step and
        /// only setting up the Devices object so that the caller can set the device of their choosing.
        /// </summary>
        /// <param name="setupDefaultDevice"></param>
        public LoopbackRecorder(bool setupDefaultDevice)
        {
            try
            {
                if (setupDefaultDevice)
                {
                    // We're going to use this to detect the default device.
                    this.Devices = new MMDeviceEnumerator();
                    this.AudioDevice = this.Devices.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                }
                else
                {
                    // We're going to use this to detect the default device.
                    this.Devices = new MMDeviceEnumerator();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// The name of the file that was set when StartRecording was called.  E.g. the current file
        /// being written to.
        /// </summary>
        public string Filename { get; private set; } = "";

        /// <summary>
        /// Whether or not recording is currently happening.
        /// </summary>
        public bool IsRecording { get; private set; }

        /// <summary>
        /// A list of the available devices.
        /// </summary>
        public MMDeviceEnumerator Devices { get; private set; }

        /// <summary>
        /// The audio device that is currently in use.
        /// </summary>
        public MMDevice AudioDevice
        {
            get => _audioDevice;
            set
            {
                // If the underlying audio device already exists when this is set then dispose of it
                // before setting the new value.
                _audioDevice?.Dispose();
                _audioDevice = value;
            }
        }

        /// <summary>
        /// Returns the current audio level for the left channel that's playing between 1 and 100 for
        /// the default audio device.
        /// </summary>
        public int LeftAudioLevel
        {
            get
            {
                if (this.AudioDevice != null)
                {
                    return (int) this.AudioDevice.AudioMeterInformation.PeakValues[0] * 100;
                }

                return 0;
            }
        }

        /// <summary>
        /// Returns the current audio level for the right channel that's playing between 1 and 100
        /// for the default audio device.
        /// </summary>
        public int RightAudioLevel
        {
            get
            {
                if (this.AudioDevice != null)
                {
                    return (int) this.AudioDevice.AudioMeterInformation.PeakValues[1] * 100;
                }

                return 0;
            }
        }

        /// <summary>
        /// Disposes of all resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Starts the recording.
        /// </summary>
        /// <param name="filename">The location of where the wave file should be saved.</param>
        public void StartRecording(string filename)
        {
            // If we are currently recording then go ahead and exit out.
            if (this.IsRecording)
            {
                return;
            }

            this.Filename = filename;
            _waveIn = new WasapiLoopbackCapture();
            _writer = new WaveFileWriter(filename, _waveIn.WaveFormat);

            _waveIn.DataAvailable += this.OnDataAvailable;
            _waveIn.RecordingStopped += this.OnRecordingStopped;
            _waveIn.StartRecording();
            this.IsRecording = true;
        }

        /// <summary>
        /// Stops the recording
        /// </summary>
        public void StopRecording()
        {
            _waveIn?.StopRecording();
        }

        /// <summary>
        /// Returns the number of seconds that have elapsed in the current recording.  If the WaveFileWriter
        /// is null or the average bytes per second is 0 a -1 is returned.
        /// </summary>
        public int ElapsedSeconds()
        {
            // If this is null for some reason get out.
            if (_writer == null)
            {
                return -1;
            }

            // This shouldn't ever be the case but the check is cheap, let's not divide by zero.
            if (_writer.WaveFormat.AverageBytesPerSecond == 0)
            {
                return -1;
            }

            return (int) (_writer.Length / _writer.WaveFormat.AverageBytesPerSecond);
        }

        /// <summary>
        /// Event handled when recording is stopped.  We will clean up open objects here that are required to be
        /// closed and/or disposed of.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            // Writer Dispose() needs to come first otherwise NAudio will lock up.
            _writer?.Dispose();
            _writer = null;

            // Remove the events on DataAvailable and RecordingStopped.
            _waveIn.DataAvailable -= this.OnDataAvailable;
            _waveIn.RecordingStopped -= this.OnRecordingStopped;

            // Dispose of the IWaveIn.
            _waveIn?.Dispose();
            _waveIn = null;

            this.IsRecording = false;

            if (e.Exception != null)
            {
                throw e.Exception;
            }
        }

        /// <summary>
        /// Event handled when data becomes available.  The data will be written out to disk at this point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            _writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        /// <summary>
        /// Protected implementation of <see cref="Dispose" />.
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
                _writer?.Dispose();
                _writer = null;

                _waveIn?.Dispose();
                _waveIn = null;

                this.Devices?.Dispose();
                this.Devices = null;

                this.AudioDevice?.Dispose();
                this.AudioDevice = null;
            }

            _disposed = true;
            this.IsRecording = false;
        }
    }
}