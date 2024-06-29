/*
 * @author            : Blake Pell
 * @initial date      : 2024-03-05
 * @last updated      : 2024-06-29
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 * @website           : http://www.blakepell.com
 */

using System.Threading.Tasks;

namespace Argus.Data
{
    /// <summary>
    /// Function to encode common data types to Base64 for specific usages.
    /// </summary>
    public static class Base64Encoder
    {
        /// <summary>
        /// Converts the contents of a file to Base64.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>The contents of a file as Base64.</returns>
        public static string ConvertFileToBase64(string filePath)
        {
            // Read the contents of the file into a byte array.
            var fileBytes = File.ReadAllBytes(filePath);

            // Encode the byte array as a base64 string.
            string base64String = Convert.ToBase64String(fileBytes);

            // Return the base64 string.
            return base64String;
        }

        #if NET5_0_OR_GREATER
        /// <summary>
        /// Converts the contents of a file to Base64.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>The contents of a file as Base64.</returns>
        public static async Task<string> ConvertFileToBase64Async(string filePath)
        {
            // Read the contents of the file into a byte array.
            var fileBytes = await File.ReadAllBytesAsync(filePath);

            // Encode the byte array as a base64 string.
            string base64String = Convert.ToBase64String(fileBytes);

            // Return the base64 string.
            return base64String;
        }
        #endif
        
        /// <summary>
        /// Converts a base64 string to an HTML &lt;img&gt; element.
        /// </summary>
        /// <param name="base64">The base64 string representing the image.</param>
        /// <param name="imageType"></param>
        /// <returns>A string containing the HTML &lt;img&gt; element with the given base64 string as the source.</returns>
        public static string Base64HtmlImage(string base64, string imageType = "png")
        {
            // Check if the input string is not null or empty
            if (string.IsNullOrEmpty(base64))
            {
                throw new ArgumentNullException(nameof(base64), "Input string cannot be null or empty.");
            }

            // Return the HTML img tag
            return $"<img src=\"data:image/{imageType};base64,{base64}\" />";
        }

        /// <summary>
        /// Converts a base64 string to a Html image tag.
        /// </summary>
        /// <param name="base64">The base64 string representing the image.</param>
        /// <param name="imageType">The image type, defaults to "png".</param>
        /// <returns>A string containing the Html image tag with the given base64 string as the source.</returns>
        public static string Base64MarkdownImage(string base64, string imageType = "png")
        {
            // Check if the input string is not null or empty
            if (string.IsNullOrEmpty(base64))
            {
                throw new ArgumentNullException(nameof(base64), "Input string cannot be null or empty.");
            }

            // Return the Html image tag
            return $"![Image](data:image/{imageType};base64,{base64})";
        }

        /// <summary>
        /// Converts a base64 string to a CSS background image.
        /// </summary>
        /// <param name="base64">The base64 string representing the image.</param>
        /// <param name="imageType">The image type, defaults to "png".</param>
        /// <returns>A CSS class with the background image set to the given base64 string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input string is null or empty.</exception>
        public static string Base64CssImage(string base64, string imageType = "png")
        {
            // Check if the input string is not null or empty
            if (string.IsNullOrEmpty(base64))
            {
                throw new ArgumentNullException(nameof(base64), "Input string cannot be null or empty.");
            }

            return @$".element {{ background-image: url(data:image/{imageType};base64,{base64}); }}";
        }
    }
}