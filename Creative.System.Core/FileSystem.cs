using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Creative.System.Core
{
    public class FileSystem : IFileSystem
    {
        /// <summary>
        ///     Returns the directory information for the specified path string.
        /// </summary>
        /// <returns>
        ///     Directory information for <paramref name="path" />, or null if <paramref name="path" /> denotes a root directory or
        ///     is null. Returns <see cref="F:System.String.Empty" /> if <paramref name="path" /> does not contain directory
        ///     information.
        /// </returns>
        /// <param name="path">The path of a file or directory. </param>
        /// <exception cref="T:System.ArgumentException">
        ///     The <paramref name="path" /> parameter contains invalid characters, is
        ///     empty, or contains only white spaces.
        /// </exception>
        /// <exception cref="T:System.IO.PathTooLongException">
        ///     The <paramref name="path" /> parameter is longer than the
        ///     system-defined maximum length.
        /// </exception>
        string IFileSystem.GetDirectoryName(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Opens an existing file or creates a new file for writing.
        /// </summary>
        /// <param name="path">The file to be opened for writing.</param>
        /// <returns>An unshared <see cref="FileStream" /> object on the specified path with Write access.</returns>
        FileStream IFileSystem.OpenWrite(string path)
        {
            return File.OpenWrite(path);
        }

        /// <summary>
        ///     Opens an existing UTF-8 encoded text file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A <see cref="UnauthorizedAccessException" /> on the specified path.</returns>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission. -or-
        ///     <paramref name="path" /> specified a file that is read-only.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path" /> is a zero-length string, contains only white space, or
        ///     contains one or more invalid characters as defined by <see cref="Path" />.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is a null reference.</exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path, file name, or both exceed the system-defined maximum
        ///     length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be
        ///     less than 260 characters.
        /// </exception>
        /// <exception cref="IOException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException">An I/O error occurred while creating the file.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path" /> is in an invalid format.</exception>
        StreamReader IFileSystemOpenText(string path)
        {
            return File.OpenText(path);
        }

        /// <summary>
        ///     Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist,
        ///     this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="path">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        void IFileSystemAppendAllText(string path, string contents)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Creates or overwrites a file in the specified path.
        /// </summary>
        /// <param name="path">The path and name of the file to create.</param>
        /// <returns>A <see cref="FileStream" /> that provides read/write access to the file specified in <paramref name="path" />.</returns>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission. -or-
        ///     <paramref name="path" /> specified a file that is read-only.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path" /> is a zero-length string, contains only white space, or
        ///     contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars" />.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is a null reference.</exception>
        /// <exception cref="PathTooLongException">
        ///     The specified path, file name, or both exceed the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than
        ///     260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while creating the file.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path" /> is in an invalid format.</exception>
        FileStream IFileSystemCreate(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns a <see cref="FileInfo" /> object for the specified file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>A <see cref="FileInfo" /> object for the specified file.</returns>
        FileInfo IFileSystemGetFileInfo(string path)
        {
            return new FileInfo(path);
        }

        /// <summary>
        ///     Deletes a specified directory and all of the files contained within.
        /// </summary>
        /// <param name="path">The name of the empty directory to remove. This directory must be writable or empty.</param>
        /// <param name="deleteAllFiles">Delete all of the files in the directory before deleting the directory.</param>
        void IFileSystemDeleteDirectory(string path, bool deleteAllFiles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Deletes a specified directory.
        /// </summary>
        /// <param name="path">The name of the empty directory to remove. This directory must be writable or empty.</param>
        void IFileSystemDeleteDirectory(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file
        ///     already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="bytes">The bytes to write to the file.</param>
        void IFileSystemWriteAllBytes(string path, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A byte array containing the contents of the file.</returns>
        byte[] IFileSystemReadAllBytes(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Moves a file to a new location optionally specifying a new filename
        /// </summary>
        /// <param name="source">The name of the file to move</param>
        /// <param name="destination">New path for the file</param>
        /// <param name="overwrite">Indicates whether to overwrite a file with the same name in the destination.</param>
        void IFileSystemMoveFile(string source, string destination, bool overwrite)
        {
            if (overwrite && File.Exists(destination))
                File.Delete(destination);
            File.Move(source, destination);
        }

        /// <summary>
        ///     Returns a list of files in the specified directory.
        /// </summary>
        /// <param name="path">The path to search</param>
        /// <returns>An enumeration of the complete paths to all files in the <paramref name="path" />.</returns>
        IEnumerable<string> IFileSystemGetFiles(string path)
        {
            return Directory.EnumerateFiles(path);
        }

        /// <summary>
        ///     Returns a list of files in the specified directory.
        /// </summary>
        /// <param name="path">The path to search</param>
        /// <param name="searchPattern">The search string, such as "*.txt"</param>
        /// <returns>An enumeration of the complete paths to files that match the <paramref name="searchPattern" />.</returns>
        IEnumerable<string> IFileSystemGetFiles(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns a list of files in the specified directory.
        /// </summary>
        /// <param name="path">The path to search</param>
        /// <param name="searchPattern">The search string, such as "*.txt"</param>
        /// <param name="searchOption">
        ///     Specifies whether to search only in the top-level path, or whether to recursively search the
        ///     top-level path and all subdirectories.
        /// </param>
        /// <returns>An enumeration of the complete paths to files that match the <paramref name="searchPattern" />.</returns>
        IEnumerable<string> IFileSystemGetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Deletes the specified file. An exception is thrown if the specified file does not exist.
        /// </summary>
        /// <param name="path">The name of the file to be deleted.</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path" /> is a zero-length string, contains only white space, or
        ///     contains one or more invalid characters as defined by <seealso cref="Path.GetInvalidPathChars" />.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is null reference.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">The specified file is in use.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path" /> is in an invalid format.</exception>
        /// <exception cref="PathTooLongException">
        ///     The specified path, file name, or both exceed the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than
        ///     260 characters.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.<br />-or- <br />
        ///     <paramref name="path" /> is a directory.<br />-or- <br /><paramref name="path" /> specified a read-only file.
        /// </exception>
        /// <remarks>
        ///     The path parameter is permitted to specify relative or absolute path information. Relative path information is
        ///     interpreted as relative to the current working directory. To obtain the current working directory, see
        ///     <seealso cref="Directory.GetCurrentDirectory">.<br />For a list of common I/O tasks, see Common I/O Tasks.</seealso>
        /// </remarks>
        void IFileSystemDelete(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        ///     Opens an existing file for reading
        /// </summary>
        /// <param name="path">The file to be opened for reading</param>
        /// <returns>A <see cref="Stream" /> containing the contents of the file.</returns>
        Stream IFileSystemOpenRead(string path)
        {
            return File.OpenRead(path);
        }

        /// <summary>
        ///     Opens a text file, reads all lines of the file, then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading</param>
        /// <returns>A string array containing all lines of the file.</returns>
        string[] IFileSystemReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        /// <summary>
        ///     Opens a text file, reads all of the contents of the file, then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading</param>
        /// <returns>A string containing all lines of the file.</returns>
        string IFileSystemReadAllText(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Determines whether a file already exists
        /// </summary>
        /// <param name="path">The file to check</param>
        /// <returns>True if the file is present. False otherwise.</returns>
        bool IFileSystemExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        ///     Creates a new file, writes the specified string to the file, then closes the file. If the file already exists, it
        ///     is overwritten.
        /// </summary>
        /// <param name="path">The file to write to</param>
        /// <param name="contents">The string to write to the file</param>
        void IFileSystemWriteAllText(string path, string contents)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Creates a new file, writes the specified string to the file, then closes the file. If the file already exists, it
        ///     is overwritten.
        /// </summary>
        /// <param name="path">The file to write to</param>
        /// <param name="contents">The string to write to the file</param>
        /// <param name="encoding">An <see cref="Encoding" /> object that represents the encoding to apply to the string</param>
        void IFileSystemWriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
        }

        /// <summary>
        ///     Creates a new file, writes a collection of strings to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The lines to write to the file.</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path" /> is a zero-length string, contains only white space, or
        ///     contains one or more invalid characters defined by the GetInvalidPathChars method.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Either <paramref name="path" /> or <paramref name="contents" /> is
        ///     <value>null</value>
        ///     .
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     <paramref name="path" /> is invalid (for example, it is on an unmapped
        ///     drive).
        /// </exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="PathTooLongException">
        ///     <paramref name="path" /> exceeds the system-defined maximum length. For example,
        ///     on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="NotSupportedException"><paramref name="path" /> is in an invalid format.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     <paramref name="path" /> specifies a file that is read-only.
        ///     -or-
        ///     This operation is not supported on the current platform.
        ///     -or-
        ///     <paramref name="path" /> is a directory.
        ///     -or-
        ///     The caller does not have the required permission.
        /// </exception>
        /// <remarks>
        ///     The default behavior of the WriteAllLines(String, IEnumerable{String}) method is to write out data by using UTF-8
        ///     encoding without a byte order mark (BOM). If it is necessary to include a UTF-8 identifier, such as a byte order
        ///     mark, at the beginning of a file, use the WriteAllLines(String, IEnumerable{String}, Encoding) method overload with
        ///     UTF8 encoding.
        ///     If the target file already exists, it is overwritten.
        ///     You can use this method to create the contents for a collection class that takes an IEnumerable{T} in its
        ///     constructor, such as a List{T}, HashSet{T}, or a SortedSet{T} class.
        /// </remarks>
        void IFileSystemWriteAllLines(string path, IEnumerable<string> contents)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Creates or opens a file for writing UTF-8 encoded text
        /// </summary>
        /// <param name="path">The file to be opened for writing</param>
        /// <returns>A <see cref="StreamWriter" /> that writes to the specified file using UTF-8 encoding.</returns>
        StreamWriter IFileSystemCreateText(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>true if path refers to an existing directory; otherwise, false.</returns>
        bool IFileSystemDirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Creates all the directories in a specified path.
        /// </summary>
        /// <param name="path">The directory path to create. </param>
        /// <returns>A <see cref="DirectoryInfo" /> as specified by path.</returns>
        DirectoryInfo IFileSystemCreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Creates a new <see cref="DirectoryInfo" /> from an existing directory
        /// </summary>
        /// <param name="path">The directory to create the <see cref="DirectoryInfo" /> value. </param>
        /// <returns>A <see cref="DirectoryInfo" /> as specified by path.</returns>
        DirectoryInfo IFileSystemGetDirectoryInfo(string path)
        {
            return new DirectoryInfo(path);
        }

        /// <summary>
        ///     Moves a file or a directory and its contents to a new location.
        /// </summary>
        /// <param name="sourceDirName">The path of the file or directory to move.</param>
        /// <param name="destDirName">
        ///     The path to the new location for sourceDirName. If sourceDirName is a file, then destDirName
        ///     must also be a file name.
        /// </param>
        void IFileSystemMoveDirectory(string sourceDirName, string destDirName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
        /// </summary>
        /// <param name="sourceFileName">The file to copy.</param>
        /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
        void IFileSystemCopy(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
        }

        /// <summary>
        /// Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.
        /// </summary>
        /// <returns>The full path of the temporary file.</returns>
        /// <exception cref="IOException">An I/O error occurs, such as no unique temporary file name is available.
        ///
        /// - or -
        ///
        /// This method was unable to create a temporary file.
        ///  </exception>
        /// <remarks>
        /// This method creates a temporary file with a .TMP file extension. The temporary file is created within the user’s temporary folder, which is the path returned by the GetTempPath method.
        ///
        /// The GetTempFileName method will raise an IOException if it is used to create more than 65535 files without deleting previous temporary files.
        ///
        ///    The GetTempFileName method will raise an IOException if no unique temporary file name is available.To resolve this error, delete all unneeded temporary files.
        ///
        ///    For a list of common I/O tasks, see Common I/O Tasks.
        /// </remarks>
        string IFileSystemGetTempFileName()
        {
            return Path.GetTempFileName();
        }

        /// <summary>
        /// Returns a random folder name or file name.
        /// </summary>
        /// <returns>A random folder name or file name.</returns>
        /// <remarks>The GetRandomFileName method returns a cryptographically strong, random string that can be used as either a folder name or a file name.
        /// Unlike GetTempFileName, GetRandomFileName does not create a file. When the security of your file system is paramount,
        /// this method should be used instead of GetTempFileName.</remarks>
        string IFileSystemGetRandomFileName()
        {
            return Path.GetRandomFileName();
        }

        Task IFileSystem.WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        StreamReader IFileSystem.OpenText(string path)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.AppendAllText(string path, string contents)
        {
            throw new NotImplementedException();
        }

        FileStream IFileSystem.Create(string path)
        {
            throw new NotImplementedException();
        }

        FileInfo IFileSystem.GetFileInfo(string path)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.DeleteDirectory(string path, bool deleteAllFiles)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.DeleteDirectory(string path)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.WriteAllBytes(string path, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        byte[] IFileSystem.ReadAllBytes(string path)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.MoveFile(string source, string destination, bool overwrite)
        {
            throw new NotImplementedException();
        }

        IEnumerable<string> IFileSystem.GetFiles(string path)
        {
            throw new NotImplementedException();
        }

        IEnumerable<string> IFileSystem.GetFiles(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        IEnumerable<string> IFileSystem.GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.Delete(string path)
        {
            throw new NotImplementedException();
        }

        Stream IFileSystem.OpenRead(string path)
        {
            throw new NotImplementedException();
        }

        string[] IFileSystem.ReadAllLines(string path)
        {
            throw new NotImplementedException();
        }

        string IFileSystem.ReadAllText(string path)
        {
            throw new NotImplementedException();
        }

        bool IFileSystem.Exists(string path)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.WriteAllText(string path, string contents)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.WriteAllText(string path, string contents, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.WriteAllLines(string path, IEnumerable<string> contents)
        {
            throw new NotImplementedException();
        }

        StreamWriter IFileSystem.CreateText(string path)
        {
            throw new NotImplementedException();
        }

        bool IFileSystem.DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        DirectoryInfo IFileSystem.CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        DirectoryInfo IFileSystem.GetDirectoryInfo(string path)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.MoveDirectory(string sourceDirName, string destDirName)
        {
            throw new NotImplementedException();
        }

        void IFileSystem.Copy(string sourceFileName, string destFileName)
        {
            throw new NotImplementedException();
        }

        string IFileSystem.GetTempFileName()
        {
            throw new NotImplementedException();
        }

        string IFileSystem.GetRandomFileName()
        {
            throw new NotImplementedException();
        }
    }
}
