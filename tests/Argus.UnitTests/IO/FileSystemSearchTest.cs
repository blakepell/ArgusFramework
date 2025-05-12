/*
 * @author            : Blake Pell
 * @initial date      : 2025-05-11
 * @last updated      : 2025-05-11
 * @copyright         : Copyright (c) 2003-2025, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System.IO;
using System.Linq;
using Argus.IO;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Argus.UnitTests.IO;

[TestClass]
[TestSubject(typeof(FileSystemSearch))]
public class FileSystemSearchTest
{
    /// <summary>
    /// Constructor: Setup the test directories
    /// </summary>
    public FileSystemSearchTest()
    {
        this.CreateDirectoryIfNotExists(@"C:\Temp");
        this.CreateDirectoryIfNotExists(@"C:\Temp\Temp");
        this.CreateDirectoryIfNotExists(@"C:\Music");
        this.CreateDirectoryIfNotExists(@"C:\Music\playlists");
        
        if (!File.Exists(@"C:\Temp\test.txt"))
        {
            File.WriteAllText(@"C:\Temp\test.txt", "");
        }
    }
    
    /// <summary>
    /// Help utility to create a folder if it does not exist
    /// </summary>
    /// <param name="path"></param>
    private void CreateDirectoryIfNotExists(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }
        
        if (!Directory.Exists(path))    
        {
            Directory.CreateDirectory(path);
        }
    }
    
    /// <summary>
    /// Test for directories in the top level folder only.
    /// </summary>
    [TestMethod]
    public void DirectoryTestTopOnly()
    {
        var fs = new FileSystemSearch(@"C:\", SearchOption.TopDirectoryOnly);

        Assert.IsTrue(fs.Any(x => x.FullName.Equals(@"C:\Windows")));
        Assert.IsTrue(fs.Any(x => x.FullName.Equals(@"C:\Temp")));
        Assert.IsFalse(fs.Any(x => x.FullName.Equals(@"C:\ThisDoesNotExist")));
    }
    
    /// <summary>
    /// Test for directories in the top level or in a subfolder.
    /// </summary>
    [TestMethod]
    public void DirectoryTestAllDirectories()
    { 
        var fs = new FileSystemSearch(@"C:\", SearchOption.AllDirectories)
        {
            IncludeDirectories = true
        };
        
        Assert.IsTrue(fs.Any(x => x.FullName.Equals(@"C:\Windows")));
        Assert.IsTrue(fs.Any(x => x.FullName.Equals(@"C:\Music\playlists")));
        Assert.IsTrue(fs.Any(x => x.FullName.Equals(@"C:\Temp\Temp")));
    }

    [TestMethod]
    public void FileExistsTopLevel()
    {
        var fs = new FileSystemSearch(@"C:\", SearchOption.TopDirectoryOnly);
        
        Assert.IsFalse(fs.Any(x => x.FullName.Equals(@"C:\test.txt")));
        Assert.IsFalse(fs.Any(x => x.FullName.Equals(@"C:\Temp\test.txt")));
    }

    [TestMethod]
    public void FileExistsAllDirectories()
    {
        var fs = new FileSystemSearch(@"C:\Temp", SearchOption.AllDirectories);

        // The false case on the root of a drive will be forced to loop through every file in the
        // tree which could be in the tens of millions.  In a real scenario we would use File.Exists
        // if we were only checking on a single file.  For a pattern match we would still need this.
        Assert.IsFalse(fs.Any(x => x.FullName.Equals(@"C:\test.txt")));
        Assert.IsTrue(fs.Any(x => x.FullName.Equals(@"C:\Temp\test.txt")));
        Assert.IsTrue(fs.Any(x => FileSystemSearch.PatternMatches(x.Name, "*.txt")));
        Assert.IsFalse(fs.Any(x => FileSystemSearch.PatternMatches(x.Name, "*.DoesNotExist")));
    }
}