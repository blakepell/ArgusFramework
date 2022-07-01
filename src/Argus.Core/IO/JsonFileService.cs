/*
 * @author            : Microsoft, Blake Pell
 * @initial date      : 2021-10-03
 * @last updated      : 2022-07-01
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 */

using Argus.IO;

#if NET6_0_OR_GREATER
using Argus.Data;
using System.Text.Json;
#else
using Newtonsoft.Json;
#endif

using System.Text;
using System.Threading.Tasks;

namespace Argus.IO
{
#if NET6_0_OR_GREATER
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
        /// <param name="knownFolder">Well know system folder.</param>
        /// <param name="folderName">Name of the folder in the knownFolder path.</param>
        public JsonFileService(Environment.SpecialFolder knownFolder, string folderName)
        {
            this.FolderPath = Path.Join(Environment.GetFolderPath(knownFolder), folderName);
            FileSystemUtilities.CreateDirectory(this.FolderPath);
        }

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
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        public T? Read<T>(string filename)
        {
            string fullPath = Path.Combine(this.FolderPath, filename);

            if (File.Exists(fullPath))
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(fullPath));
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
#else
    /// <summary>
    /// File service for reading and writing JSON objects to the file system.
    /// </summary>
    public class JsonFileService
    {
        /// <summary>
        /// The full path of the folder to save the JSON files in.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// The name of the file to save.
        /// </summary>
        public string Filename { get; set; } = "Generic.json";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="knownFolder">Well know system folder.</param>
        /// <param name="folderName">Name of the folder in the knownFolder path.</param>
        public JsonFileService(Environment.SpecialFolder knownFolder, string folderName)
        {
            this.FolderPath = Path.Combine(Environment.GetFolderPath(knownFolder), folderName);
            FileSystemUtilities.CreateDirectory(this.FolderPath);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="knownFolder">Well know system folder.</param>
        /// <param name="folderName">Name of the folder in the knownFolder path.</param>
        /// <param name="filename"></param>
        public JsonFileService(Environment.SpecialFolder knownFolder, string folderName, string filename)
        {
            this.Filename = filename;
            this.FolderPath = Environment.GetFolderPath(knownFolder);
            FileSystemUtilities.CreateDirectory(this.FolderPath);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="folderPath">The full path of the folder to save the JSON files in.</param>
        /// <param name="filename"></param>
        public JsonFileService(string folderPath, string filename)
        {
            this.Filename = filename;
            this.FolderPath = folderPath;
            FileSystemUtilities.CreateDirectory(this.FolderPath);
        }

        /// <summary>
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T Read<T>()
        {
            string fullPath = Path.Combine(this.FolderPath, this.Filename);

            if (File.Exists(fullPath))
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(fullPath));
            }

            return default;
        }

        /// <summary>
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public T Read<T>(string folderPath, string fileName)
        {
            this.FolderPath = folderPath;
            this.Filename = fileName;

            return Read<T>(Path.Combine(folderPath, fileName));
        }

        /// <summary>
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullPath"></param>
        public T Read<T>(string fullPath)
        {
            this.FolderPath = Path.GetDirectoryName(fullPath);
            this.Filename = Path.GetFileName(fullPath);

            if (File.Exists(fullPath))
            {
                var json = File.ReadAllText(fullPath);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }

        /// <summary>
        /// Serializes an object and saves the JSON file to the file system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        public void Save<T>(T content)
        {
            File.WriteAllText(Path.Combine(this.FolderPath, this.Filename), JsonConvert.SerializeObject(content), Encoding.UTF8);
        }

        /// <summary>
        /// Serializes an object and saves the JSON file to the file system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void Save<T>(T content, string folderPath, string fileName)
        {
            this.FolderPath = folderPath;
            this.Filename = fileName;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileContent = JsonConvert.SerializeObject(content);
            File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
        }

        /// <summary>
        /// Serializes an object and saves the JSON file to the file system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullPath"></param>
        /// <param name="content"></param>
        public void Save<T>(T content, string fullPath)
        {
            this.FolderPath = Path.GetDirectoryName(fullPath);
            this.Filename = Path.GetFileName(fullPath);

            File.WriteAllText(fullPath, JsonConvert.SerializeObject(content), Encoding.UTF8);
        }

        /// <summary>
        /// Deletes the file at the specified location.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void Delete(string folderPath, string fileName)
        {
            this.FolderPath = folderPath;
            this.Filename = fileName;

            if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
            {
                File.Delete(Path.Combine(folderPath, fileName));
            }
        }
    }
#endif

}