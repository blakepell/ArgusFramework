/*
 * @author            : Microsoft, Blake Pell
 * @initial date      : 2021-10-03
 * @last updated      : 2022-06-02
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 */

using System.Threading.Tasks;

namespace Argus.IO
{
    /// <summary>
    /// File service for reading and writing JSON objects to the file system.
    /// </summary>
    public class JsonFileService
    {
        /// <summary>
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullPath"></param>
        public T Read<T>(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                var json = File.ReadAllText(fullPath);
                return JsonConvert.DeserializeObject<T>(json);
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
            return this.Read<T>(Path.Combine(folderPath, fileName));
        }

#if NETSTANDARD2_1 || NET5_0_OR_GREATER
        /// <summary>
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullPath"></param>
        public async Task<T> ReadAsync<T>(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                var json = await File.ReadAllTextAsync(fullPath).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }

        /// <summary>
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public async Task<T> ReadAsync<T>(string folderPath, string fileName)
        {
            return await this.ReadAsync<T>(Path.Combine(folderPath, fileName));
        }
#endif

        /// <summary>
        /// Serializes an object and saves the JSON file to the file system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullPath"></param>
        /// <param name="content"></param>
        public void Save<T>(string fullPath, T content)
        {
            File.WriteAllText(fullPath, JsonConvert.SerializeObject(content), Encoding.UTF8);
        }

        /// <summary>
        /// Serializes an object and saves the JSON file to the file system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public void Save<T>(string folderPath, string fileName, T content)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileContent = JsonConvert.SerializeObject(content);
            File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
        }

#if NETSTANDARD2_1 || NET5_0_OR_GREATER
        /// <summary>
        /// Serializes an object and saves the JSON file to the file system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public async Task SaveAsync<T>(string fullPath, T content)
        {
            await File.WriteAllTextAsync(fullPath, JsonConvert.SerializeObject(content), Encoding.UTF8).ConfigureAwait(false);
        }

        /// <summary>
        /// Serializes an object and saves the JSON file to the file system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public async Task SaveAsync<T>(string folderPath, string fileName, T content)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileContent = JsonConvert.SerializeObject(content);
            await File.WriteAllTextAsync(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8).ConfigureAwait(false);
        }
#endif

        /// <summary>
        /// Deletes the file at the specified location.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void Delete(string folderPath, string fileName)
        {
            if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
            {
                File.Delete(Path.Combine(folderPath, fileName));
            }
        }
    }
}