/*
 * @author            : Microsoft, Blake Pell
 * @initial date      : 2021-10-03
 * @last updated      : 2022-07-01
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 */

#if NET5_0_OR_GREATER

using System.Text.Json;
using System.Threading.Tasks;

namespace Argus.IO
{
    /// <summary>
    /// File service for reading and writing JSON objects to the file system.  An instance of the
    /// object assumes all files are saved in the <see cref="FolderPath"/> directory.
    /// </summary>
    public class JsonFileService
    {
        /// <summary>
        /// The full path of the folder to save the JSON files in.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="folderPath">The full path of the folder to save the JSON files in.</param>
        public JsonFileService(string folderPath)
        {
            this.FolderPath = folderPath;
            FileSystemUtilities.CreateDirectory(this.FolderPath);
        }

        /// <summary>
        /// Constructor: Uses a folder with the assemblies name in the specified SpecialFolder.
        /// </summary>
        /// <param name="knownFolder">Well know system folder.</param>
        public JsonFileService(Environment.SpecialFolder knownFolder)
        {
            string assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "";
            this.FolderPath = Path.Join(Environment.GetFolderPath(knownFolder), assemblyName);
            FileSystemUtilities.CreateDirectory(this.FolderPath);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="knownFolder">Well know system folder.</param>
        /// <param name="folderName">Name of the folder in the knownFolder path.</param>
        public JsonFileService(Environment.SpecialFolder knownFolder, string folderName)
        {
            this.FolderPath = Path.Join(Environment.GetFolderPath(knownFolder), folderName);
            FileSystemUtilities.CreateDirectory(this.FolderPath);
        }
         
        /// <summary>
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        public T? Read<T>(string filename)
        {
            string fullPath = Path.Combine(this.FolderPath, filename);

            if (File.Exists(fullPath))
            {
                using var json = File.OpenRead(fullPath);
                return JsonSerializer.Deserialize<T>(json);
            }

            return default;
        }

        /// <summary>
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        public async Task<T> ReadAsync<T>(string filename)
        {
            string fullPath = Path.Combine(this.FolderPath, filename);
        
            if (File.Exists(fullPath))
            {
                await using var json = File.OpenRead(fullPath);
                return await JsonSerializer.DeserializeAsync<T>(json);
            }

            return default;
        }

        /// <summary>
        /// Serializes an object and saves the JSON file to the file system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        public void Save<T>(T content, string filename)
        {
            using var stream = File.Create(Path.Combine(this.FolderPath, filename));
            JsonSerializer.Serialize(stream, content);
        }

        /// <summary>
        /// Serializes an object and saves the JSON file to the file system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        public async Task SaveAsync<T>(T content, string filename)
        {
            await using var stream = File.Create(Path.Combine(this.FolderPath, filename));
            await JsonSerializer.SerializeAsync(stream, content);
        }

        /// <summary>
        /// Deletes the file at the specified location.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void Delete(string folderPath, string fileName)
        {
            this.FolderPath = folderPath;

            if (File.Exists(Path.Combine(folderPath, fileName)))
            {
                File.Delete(Path.Combine(folderPath, fileName));
            }
        }
    }
}
#endif