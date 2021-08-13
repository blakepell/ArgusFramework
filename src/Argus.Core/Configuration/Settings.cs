/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-08-13
 * @last updated      : 2021-08-13
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Argus.IO;
using Newtonsoft.Json;

namespace Argus.Configuration
{
    /// <summary>
    /// A settings class that can serialize and deserialize settings data in
    /// JSON format.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// The directory the settings should be persisted to.
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// The name of the settings file.  If the <see cref="Load{T}"/> and <see cref="Save{T}"/>
        /// overloads are used that specify a filename this property will be overwritten.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The formatting of the saved JSON file.  The default is Indented for
        /// readability purposes.
        /// </summary>
        public Formatting Formatting { get; set; } = Formatting.Indented;

        /// <summary>
        /// Constructor: Saves settings files in the local application data folder under
        /// the calling executables assembly name.
        /// </summary>
        public Settings()
        {
            string assemblyName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? "";
            this.DirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyName);
        }

        /// <summary>
        /// Constructor: Saves settings in the specified folder.
        /// </summary>
        /// <param name="directoryPath"></param>
        public Settings(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }

        /// <summary>
        /// Loads a settings file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T Load<T>()
        {
            string loadFile = Path.Combine(this.DirectoryPath, this.Filename);

            if (!File.Exists(loadFile))
            {
                throw new FileNotFoundException($"File not found: '{loadFile}'");
            }

            string json = File.ReadAllText(this.Filename);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Loads a settings file from the specified file location.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        public T Load<T>(string filename)
        {
            this.Filename = filename;
            return this.Load<T>();
        }

        /// <summary>
        /// Saves the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSave"></param>
        public void Save<T>(T objectToSave)
        {
            string saveFile = Path.Combine(this.DirectoryPath, this.Filename);

            // Create the entire directory tree if it does not exist.
            if (!Directory.Exists(this.DirectoryPath))
            {
                FileSystemUtilities.CreateDirectory(this.DirectoryPath);
            }

            // Write the profile settings file.
            File.WriteAllText(saveFile, JsonConvert.SerializeObject(objectToSave, this.Formatting));
        }

        /// <summary>
        /// Saves the specified object to the specified filename.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSave"></param>
        /// <param name="filename"></param>
        public void Save<T>(T objectToSave, string filename)
        {
            this.Filename = filename;
            this.Save<T>(objectToSave);
        }

        /// <summary>
        /// Deletes currently set save file if it exists.
        /// </summary>
        public void Delete()
        {
            string saveFile = Path.Combine(this.DirectoryPath, this.Filename);

            if (File.Exists(saveFile))
            {
                File.Delete(saveFile);
            }
        }
    }
}
