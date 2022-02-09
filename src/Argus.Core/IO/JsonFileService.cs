/*
 * @author            : Microsoft, Blake Pell
 * @initial date      : 2021-10-03
 * @last updated      : 2021-10-03
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
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public T Read<T>(string folderPath, string fileName)
        {
            var path = Path.Combine(folderPath, fileName);

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }

#if NETSTANDARD2_1 || NET5_0_OR_GREATER
        /// <summary>
        /// Reads and deserializes a JSON file into the specified generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public async Task<T> ReadAsync<T>(string folderPath, string fileName)
        {
            var path = Path.Combine(folderPath, fileName);

            if (File.Exists(path))
            {
                var json = await File.ReadAllTextAsync(path).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }
#endif

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