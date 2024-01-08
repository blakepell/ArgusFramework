/*
 * @author            : Microsoft, Blake Pell
 * @initial date      : 2021-10-03
 * @last updated      : 2024-01-07
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 */

#if NET5_0_OR_GREATER

using Argus.IO.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        /// Options that are passed to the <see cref="JsonSerializer"/>
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="folderPath">The full path of the folder to save the JSON files in.</param>
        public JsonFileService(string folderPath)
        {
            this.FolderPath = folderPath;
            this.Initialize();
        }

        /// <summary>
        /// Constructor: Uses a folder with the assemblies name in the specified SpecialFolder.
        /// </summary>
        /// <param name="knownFolder">Well know system folder.</param>
        public JsonFileService(Environment.SpecialFolder knownFolder)
        {
            string assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "";
            this.FolderPath = Path.Join(Environment.GetFolderPath(knownFolder), assemblyName);
            this.Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="knownFolder">Well know system folder.</param>
        /// <param name="folderName">Name of the folder in the knownFolder path.</param>
        public JsonFileService(Environment.SpecialFolder knownFolder, string folderName)
        {
            this.FolderPath = Path.Join(Environment.GetFolderPath(knownFolder), folderName);
            this.Initialize();
        }

        /// <summary>
        /// Initializes default settings.
        /// </summary>
        private void Initialize()
        {
            FileSystemUtilities.CreateDirectory(this.FolderPath);

            this.JsonSerializerOptions = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = false,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true
            };

            this.JsonSerializerOptions.Converters.Add(new TypeJsonConverter());
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
                return JsonSerializer.Deserialize<T>(json, this.JsonSerializerOptions);
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
                return await JsonSerializer.DeserializeAsync<T>(json, this.JsonSerializerOptions);
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
            JsonSerializer.Serialize(stream, content, this.JsonSerializerOptions);
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
            await JsonSerializer.SerializeAsync(stream, content, this.JsonSerializerOptions);
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

        /// <summary>
        /// Deletes the file inside of the <see cref="FolderPath"/>.
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {
            if (File.Exists(Path.Combine(this.FolderPath, fileName)))
            {
                File.Delete(Path.Combine(this.FolderPath, fileName));
            }
        }
        
        /// <summary>
        /// Deserializes a JSON string into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to.</typeparam>
        /// <param name="json">The JSON string to deserialize from.</param>
        /// <returns>The deserialized object of type T.</returns>
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, this.JsonSerializerOptions);
        }

        /// <summary>
        /// Asynchronously deserializes a JSON string into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to.</typeparam>
        /// <param name="json">The JSON string to deserialize from.</param>
        /// <returns>The deserialized object of type T.</returns>
        public async Task<T> DeserializeAsync<T>(string json)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return await JsonSerializer.DeserializeAsync<T>(stream, this.JsonSerializerOptions);
        }

        /// <summary>
        /// Serializes an object to a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The serialized JSON string.</returns>
        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, this.JsonSerializerOptions);
        }

        /// <summary>
        /// Asynchronously serializes an object to a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The serialized JSON string.</returns>
        public async Task<string> SerializeAsync<T>(T obj)
        {
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, obj, this.JsonSerializerOptions);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }        
    }
}
#endif