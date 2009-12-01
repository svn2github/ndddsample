namespace NDDDSample.Infrastructure.Utils
{
    #region Usings

    using System.Collections.Generic;
    using System.IO;

    #endregion

    public static class FileUtils
    {
        /// <summary>
        /// Open an txt file and read lines from it.
        /// </summary>
        /// <param name="fileInfo">File Info of a text file.</param>
        /// <returns>List of string lines.</returns>
        public static IList<string> ReadLines(FileInfo fileInfo)
        {
            IList<string> lines = new List<string>();
            StreamReader streamReader = fileInfo.OpenText();

            try
            {                
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            finally
            {
               streamReader.Close();             
            }

            return lines;
        }

        /// <summary>
        /// Create a new txt file and writes lines to it.
        /// </summary>
        /// <param name="directory">Directory where the new file will be created.</param>
        /// <param name="filename">file name</param>
        /// <param name="lines">Lines to write</param>
        public static void WriteLines(DirectoryInfo directory, string filename, IList<string> lines)
        {
            var filePath = directory.FullName + filename;
            StreamWriter streamWriter = File.CreateText(filePath);

            try
            {
                foreach (string line in lines)
                {
                    streamWriter.WriteLine(line);
                }               
            }
            finally
            {
                streamWriter.Close();
            }
        }
    }
}