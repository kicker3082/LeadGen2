using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Creative.System.Core
{
    public class PathExtra
    {
        readonly IFileSystem _file;

        public PathExtra(IFileSystem file)
        {
            _file = file;
        }

        public string GetRelativePath(string fromPath, string toPath)
        {
            var fromAttr = GetPathAttribute(fromPath);
            var toAttr = GetPathAttribute(toPath);

            var path = new StringBuilder(260); // MAX_PATH
            if (PathRelativePathTo(
                path,
                fromPath,
                fromAttr,
                toPath,
                toAttr) == 0)
            {
                throw new ArgumentException("Paths must have a common prefix");
            }
            return path.ToString();
        }

        int GetPathAttribute(string path)
        {
            var di = _file.GetDirectoryInfo(path);
            if (di.Exists)
            {
                return file_attribute_directory;
            }

            var fi = _file.GetFileInfo(path);
            if (fi.Exists)
            {
                return file_attribute_normal;
            }

            throw new FileNotFoundException();
        }

        const int file_attribute_directory = 0x10;
        const int file_attribute_normal = 0x80;

        [DllImport("shlwapi.dll", SetLastError = true)]
        static extern int PathRelativePathTo(StringBuilder pszPath,
            string pszFrom, int dwAttrFrom, string pszTo, int dwAttrTo);
    }
}