/*
 * @author            : Blake Pell
 * @initial date      : 2016-11-01
 * @last updated      : 2025-05-18
 * @copyright         : Copyright (c) 2003-2025, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using Argus.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Argus.Network
{
    /// <summary>
    /// Downloads a copy of a file from the web and holds it on the local file system until the cache
    /// period is up at which point it will fetch a new copy.
    /// </summary>
    public class WebFileCache
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="saveLocation">The location to save the cached file in.</param>
        public WebFileCache(string saveLocation)
        {
            this.SaveLocation = saveLocation;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebFileCache()
        {
        }

        /// <summary>
        /// The path to the last file that was processed.
        /// </summary>
        public string? LastProcessedFile { get; set; }

        /// <summary>
        /// The directory where cached content should be saved.
        /// </summary>
        public string? SaveLocation { get; set; }

        /// <summary>
        /// If set, the name of the file to use in replace of the filename specified in the URL.
        /// </summary>
        public string? OverrideFilename { get; set; }

        /// <summary>
        /// Downloads a copy of the file if the file on the file system is older than the threshold.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="hoursThreshold">The number of hours the file should be cached for before a new copy is fetched.</param>
        public async Task Download(string uri, int hoursThreshold)
        {
            if (string.IsNullOrWhiteSpace(this.SaveLocation))
            {
                throw new Exception("The directory in SaveLocation has not been set.");
            }

            if (string.IsNullOrWhiteSpace(this.OverrideFilename))
            {
                throw new Exception("The OverrideFilename has not been set.");
            }
            
            // Force it to always be negative
            hoursThreshold = System.Math.Abs(hoursThreshold) * -1;

            string fileName = string.IsNullOrWhiteSpace(this.OverrideFilename) ? $"{this.SaveLocation}{FileSystemUtilities.ExtractFileName(uri)}" : $"{this.SaveLocation}{this.OverrideFilename}";
            var fileInfo = new FileInfo(fileName);

            if (File.Exists(fileName) || File.Exists(fileName) && fileInfo.LastWriteTime < DateTime.Now.AddHours(hoursThreshold))
            {
                using (var hc = new HttpClient())
                {
                    // Required to get some resources off GitHub.
                    hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0)");

                    using (var response = await hc.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead))
                    {
                        using (var s = await response.Content.ReadAsStreamAsync())
                        {
                            using (var fs = File.Open(fileName, FileMode.Create))
                            {
                                await s.CopyToAsync(fs);
                                this.LastProcessedFile = fileName;
                            }
                        }
                    }
                }
            }
        }
    }
}