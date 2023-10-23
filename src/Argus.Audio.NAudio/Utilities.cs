/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2023-10-08
 * @last updated      : 2023-10-22
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NAudio.Lame;
using NAudio.Wave;

namespace Argus.Audio.NAudio
{
    /// <summary>
    /// General audio file utilities.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Convert a WAV to an MP3 file.
        /// </summary>
        /// <param name="waveFileName"></param>
        /// <param name="mp3FileName"></param>
        /// <param name="bitRate"></param>
        public static void WaveToMp3(string waveFileName, string mp3FileName, int bitRate = 128)
        {
            using (var reader = new AudioFileReader(waveFileName))
            {
                using (var writer = new LameMP3FileWriter(mp3FileName, reader.WaveFormat, bitRate))
                {
                    reader.CopyTo(writer);
                }
            }
        }

        /// <summary>
        /// Convert a WAV to an MP3 file.
        /// </summary>
        /// <param name="waveFileName"></param>
        /// <param name="mp3FileName"></param>
        /// <param name="bitRate"></param>
        public static async Task WaveToMp3Async(string waveFileName, string mp3FileName, int bitRate = 128)
        {
            using (var reader = new AudioFileReader(waveFileName))
            {
                using (var writer = new LameMP3FileWriter(mp3FileName, reader.WaveFormat, bitRate))
                {
                    await reader.CopyToAsync(writer);
                }
            }
        }

        /// <summary>
        /// Convert an MP3 to a WAV file.
        /// </summary>
        /// <param name="mp3FileName"></param>
        /// <param name="waveFileName"></param>
        public static void Mp3ToWave(string mp3FileName, string waveFileName)
        {
            using (var reader = new Mp3FileReader(mp3FileName))
            {
                using (var writer = new WaveFileWriter(waveFileName, reader.WaveFormat))
                {
                    reader.CopyTo(writer);
                }
            }
        }

        /// <summary>
        /// Convert an MP3 to a WAV file.
        /// </summary>
        /// <param name="mp3FileName"></param>
        /// <param name="waveFileName"></param>
        public static async void Mp3ToWaveAsync(string mp3FileName, string waveFileName)
        {
            using (var reader = new Mp3FileReader(mp3FileName))
            {
                using (var writer = new WaveFileWriter(waveFileName, reader.WaveFormat))
                {
                    await reader.CopyToAsync(writer);
                }
            }
        }
        
        /// <summary>
        /// Extracts a wave file from the supported MediaFoundationReader source.
        /// </summary>
        /// <param name="inputPath">
        /// The supported media foundation reader source.  Supported formats include: ".mp4", ".mov", ".m4a", ".m4v", ".avi", ".wma", ".wmv", ".asf", ".3g2", ".3gp", ".3gp2", ".3gpp", ".sami", ".smi"
        /// </param>
        /// <param name="outputPath">The output path of the wave file.</param>
        public static void ExtractWaveFile(string inputPath, string outputPath)
        {
            using (var reader = new MediaFoundationReader(inputPath))
            {
                WaveFileWriter.CreateWaveFile(outputPath, reader);
            }
        }

        /// <summary>
        /// Attempts to extract a bitrate from a string.
        /// </summary>
        /// <param name="input">A string description of the bitrate: 128 kbps</param>
        /// <remarks>
        /// Returns -1 if no matching kbps is found within the string.
        /// </remarks>
        public static int ExtractBitrateFromString(string input)
        {
            // Define a regular expression pattern to match the s
            string pattern = @"(\d+) kbps";

            // Use Regex.Match to find the first match in the input string
            var match = Regex.Match(input, pattern);

            // Check if a match was found
            if (match.Success)
            {
                // Parse the matched number as an integer
                if (int.TryParse(match.Groups[1].Value, out int result))
                {
                    return result;
                }
            }

            return -1;
        }

        /// <summary>
        /// Writes an input <see cref="WaveStream"/> to an output <see cref="WaveStream"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
        public static void WriteToWaveFile(WaveStream reader, WaveFileWriter writer)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                writer.Write(buffer, 0, bytesRead);
            }
        }

        /// <summary>
        /// Merges two MP3 Files creating into one output wave file.
        /// </summary>
        /// <param name="inputFile1">The full path to the first source file.  This file will be the first in the merger.</param>
        /// <param name="inputFile2">The full path to the second source file.  This file will be the second in the merger.</param>
        /// <param name="outputFile">The full path to the output wave file.</param>
        /// <param name="enforceBitRate">If the function should throw an exception if the bit rate, sample rate or channels are different.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void MergeMp3Files(string inputFile1, string inputFile2, string outputFile, bool enforceBitRate = false)
        {
            using (var reader1 = new Mp3FileReader(inputFile1))
            {
                using (var reader2 = new Mp3FileReader(inputFile2))
                {
                    var reader1ToWave = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(reader1));
                    var reader2ToWave = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(reader2));

                    if (enforceBitRate)
                    {
                        if (reader1ToWave.WaveFormat.SampleRate != reader2ToWave.WaveFormat.SampleRate ||
                            reader1ToWave.WaveFormat.BitsPerSample != reader2ToWave.WaveFormat.BitsPerSample ||
                            reader1ToWave.WaveFormat.Channels != reader2ToWave.WaveFormat.Channels)
                        {
                            throw new InvalidOperationException("MP3 files have different formats");
                        }
                    }

                    using (var writer = new WaveFileWriter(outputFile, reader1ToWave.WaveFormat))
                    {
                        WriteToWaveFile(reader1ToWave, writer);
                        WriteToWaveFile(reader2ToWave, writer);
                    }
                }
            }
        }

        /// <summary>
        /// Merges an an array of input files into a single MP3.
        /// </summary>
        /// <param name="inputFileList">An array of input MP3 files.</param>
        /// <param name="outputFile">The output wave file.</param>
        /// <param name="enforceBitRate">If this should only run if the bitrates match.</param>
        /// <exception cref="Exception"></exception>
        public static void MergeMp3Files(string[] inputFileList, string outputFile, bool enforceBitRate = false)
        {
            if (inputFileList.Length < 2)
            {
                throw new Exception("Two or more input files must be specified.");
            }

            string outputFileTemp = $"{outputFile}.tmp1";

            for (int i = 0; i < inputFileList.Length; i++)
            {
                // The first two files.
                if (i == 0)
                {
                    string firstFile = inputFileList[0];
                    string secondFile = inputFileList[1];
                    MergeMp3Files(firstFile, secondFile, outputFileTemp, enforceBitRate);
                    File.Move(outputFileTemp, outputFile);
                    i = 2;

                    continue;
                }

                // The rest of the files.
                MergeMp3Files(outputFile, inputFileList[i], outputFileTemp, enforceBitRate);
                File.Move(outputFileTemp, outputFile);
            }
        }

        /// <summary>
        /// Converts a wave file to an MP3 file.
        /// </summary>
        /// <param name="wavFilePath">The full path to the .wav source file.</param>
        /// <param name="mp3FilePath">The full path to the .mp3 output file.</param>
        /// <param name="bitRate">The constant bitrate of the MP3.  128 kbps is the default.</param>
        public static void ConvertWaveToMp3(string wavFilePath, string mp3FilePath, int bitRate = 128)
        {
            using (var reader = new WaveFileReader(wavFilePath))
            {
                using (var writer = new LameMP3FileWriter(mp3FilePath, reader.WaveFormat, bitRate))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        /// <summary>
        /// Uses NAudio to check if a wav file is 8kHz and if so re-samples it to 16kHz.  This only
        /// runs if the sample has a sample rate of 8000.
        /// </summary>
        /// <param name="inputPath">Path to the input WAV file.</param>
        /// <param name="outputPath">Path to the resampled output WAV file.</param>
        /// <param name="lowerThreshold">Only resamples if the threshold is below the specified amount.  The default is 16000.</param>
        /// <param name="resampleTo">Resamples to the specified amount.  The default is 16000.</param>
        public static void ResampleWaveFile(string inputPath, string outputPath, int lowerThreshold = 16000, int resampleTo = 16000)
        {
            string? inputDir = Path.GetDirectoryName(inputPath);

            if (string.IsNullOrWhiteSpace(inputDir))
            {
                return;
            }

            using (var reader = new WaveFileReader(inputPath))
            {
                // If the sample rate is below 16000 we will bring it up to 16000.
                if (reader.WaveFormat.SampleRate < 16000)
                {
                    var newFormat = new WaveFormat(16000, reader.WaveFormat.Channels);

                    using (var resampler = new MediaFoundationResampler(reader, newFormat))
                    {
                        WaveFileWriter.CreateWaveFile(outputPath, resampler);
                    }
                }
                else
                {
                    // If nothing needs to be changed then the output path is a copy
                    // of the input path.
                    File.Copy(inputPath, outputPath, true);
                }
            }
        }
    }
}