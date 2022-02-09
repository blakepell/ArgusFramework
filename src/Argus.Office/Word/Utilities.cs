/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2013-09-04
 * @last updated      : 2013-09-04
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.IO;
using System.Xml;
using Argus.IO.Compression;

namespace Argus.Office.Word
{
    /// <summary>
    /// Reads the rough contents of a MS Word docx file.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Reads the contents of a docx file back into a single string with no formatting.
        /// </summary>
        /// <param name="filename"></param>
        public static string ReadDocx(string filename)
        {
            if (File.Exists(filename) == false)
            {
                return "";
            }

            using (var zip = ZipFile.Read(filename))
            {
                using (var stream = new MemoryStream())
                {
                    zip.Extract("word/document.xml", stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(stream);

                    return xmlDoc?.DocumentElement?.InnerText ?? "";
                }
            }
        }
    }
}