using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using Argus.Extensions;

namespace Argus.IO
{

    /// <summary>
    /// Search the file system for all directories or files in a given path.
    /// </summary>
    /// <remarks>
    /// The programmer can search by using the contains method on the arraylist desired.  They can
    /// then also create a DirectoryInfo or FileInfo class as needed there instead of here.
    /// 
    /// TODO:  Add a property to only look at a single directory and not recurse through all of the child directories.
    /// </remarks>
    public class FileSystemSearch
    {
        //*********************************************************************************************************************
        //
        //             Class:  FileSystemSearch
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  12/10/2006
        //      Last Updated:  09/05/2013
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        private List<string> _directoryList = new List<string>();
        private List<string> _fileList = new List<string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <remarks></remarks>
        public FileSystemSearch(string baseDirectory)
        {
            this.BaseDirectory = baseDirectory;
        }

        /// <summary>
        /// Processes and returns a string list containing values that have the full path to the directories in them.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The entries are cached between runs.  The cache is only cleared if the Clear procedure is called or the base 
        /// directory path changes.  I.e. If you run the same base directory again, it will not reprocess unless you manually
        /// clear it first.
        /// </remarks>
        public List<string> GetAllDirectories()
        {
            // If already populated, no need to reget them.  If the base directory changes the list will clear
            if (_directoryList.Count == 0)
            {
                GetDirectories(_baseDirectory);
            }

            return _directoryList;
        }

        /// <summary>
        /// Returns the System.IO.DirectoryInfo objects for all found entries.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The entries are cached between runs.  The cache is only cleared if the Clear procedure is called or the base 
        /// directory path changes.  I.e. If you run the same base directory again, it will not reprocess unless you manually
        /// clear it first.
        /// </remarks>
        public List<DirectoryInfo> GetAllDirectoriesDirectoryInfo()
        {

            List<string> dirs = GetAllDirectories();
            List<DirectoryInfo> dirsFi = new List<DirectoryInfo>();

            foreach (string buf in dirs)
            {
                DirectoryInfo di = new DirectoryInfo(buf);
                dirsFi.Add(di);
            }

            return dirsFi;

        }

        /// <summary>
        /// Returns the System.IO.FileInfo objects for all found entries.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The entries are cached between runs.  The cache is only cleared if the Clear procedure is called or the base 
        /// directory path changes.  I.e. If you run the same base directory again, it will not reprocess unless you manually
        /// clear it first.
        /// </remarks>
        public List<FileInfo> GetAllFilesFileInfo()
        {

            List<string> files = GetAllFiles();
            List<FileInfo> filesFi = new List<FileInfo>();

            foreach (string buf in files)
            {
                FileInfo fi = new FileInfo(buf);
                filesFi.Add(fi);
            }

            return filesFi;

        }

        /// <summary>
        /// Returns a list of strings with all of the full paths to the files in them.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The entries are cached between runs.  The cache is only cleared if the Clear procedure is called or the base 
        /// directory path changes.  I.e. If you run the same base directory again, it will not reprocess unless you manually
        /// clear it first.
        /// </remarks>
        public List<string> GetAllFiles()
        {
            if (_directoryList.Count == 0)
            {
                GetDirectories(_baseDirectory);
            }

            if (_fileList.Count > 0)
            {
                return _fileList;
            }

            foreach (string dirName in _directoryList)
            {
                DirectoryInfo di = new DirectoryInfo(dirName);

                try
                {
                    foreach (FileInfo Fi in di.GetFiles())
                    {
                        _fileList.Add(Fi.FullName);
                    }
                }
                catch (System.UnauthorizedAccessException ex)
                {
                    // This is expensive but necceary if we don't want this to bomb everytime we come upon a folder
                    // that we don't have permission to.

                }
                catch
                {

                }

            }

            return _fileList;

        }

        /// <summary>
        /// Internal procedure used for recursing through directories.
        /// </summary>
        /// <param name="path"></param>
        /// <remarks></remarks>
        private void GetDirectories(string path)
        {
            try
            {
                string[] arrDir = System.IO.Directory.GetDirectories(path);

                if (_directoryList.Contains(path) == false)
                {
                    _directoryList.Add(path);
                }

                foreach (string subDir in arrDir)
                {
                    if (_directoryList.Contains(subDir) == false)
                    {
                        _directoryList.Add(subDir);
                    }
                    GetDirectories(subDir);
                }
            }
            catch (Exception ex)
            {
                // The user probably doesn't have access to this directory hence the error, eat it, the error I mean.
                // Figure out a way to catch this error though so it doesn't take all the time creating the exception.
            }

        }

        /// <summary>
        /// Clears the directory and file lists.
        /// </summary>
        /// <remarks>
        /// This can be called if you want to clear the cached results and re-run the code for the same
        /// base directory that was previously run.
        /// </remarks>
        public void Clear()
        {
            _directoryList.Clear();
            _fileList.Clear();
        }

        /// <summary>
        /// The number of directorys that were found in the search.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int DirectoryCount
        {
            get { return _directoryList.Count; }
        }

        /// <summary>
        /// The number of files that were found in the search.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int FileCount
        {
            get { return _fileList.Count; }
        }

        private string _baseDirectory;
        /// <summary>
        /// The base directory that you want to start the search at.  The searcher will recurse through all sub directories
        /// of this folder.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string BaseDirectory
        {
            get { return _baseDirectory; }

            set
            {
                bool uncPath = false;

                if (value.Length >= 2)
                {
                    if (value.Left(2) == "\\\\")
                    {
                        uncPath = true;
                    }
                }

                // Cleanup
                value = value.Replace("\\\\", "\\");

                // Replace UNC path if it was one
                if (uncPath == true)
                {
                    value = "\\" + value;
                }

                Clear();
                DirectoryInfo di = new DirectoryInfo(value);
                // will throw exception if the directory doesn't exist            
                _baseDirectory = value;
            }
        }

    }

}