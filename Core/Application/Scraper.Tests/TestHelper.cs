namespace Scraper.Tests
{
    public class TestHelper
    {
        internal static string GetFullPathToFile(string pathRelativeUnitTestingFile)
        {
            string folderProjectLevel = GetPathToCurrentUnitTestProject();
            string final = System.IO.Path.Combine(folderProjectLevel, pathRelativeUnitTestingFile);
            return final;
        }
        /// <summary>
        /// Get the path to the current unit testing project.
        /// </summary>
        /// <returns></returns>
        private static string GetPathToCurrentUnitTestProject()
        {
            string pathAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string folderAssembly = System.IO.Path.GetDirectoryName(pathAssembly);
            if (folderAssembly.EndsWith(@"\") == false)
                folderAssembly += @"\";
            string folderProjectLevel = System.IO.Path.GetFullPath(folderAssembly + @"..\..\..\");
            return folderProjectLevel;
        }
    }
}