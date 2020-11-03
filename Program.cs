using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;


// This Program is designed to implement lazy loading. 

namespace iterators_and_generators
{
    class Program
    {

        public static string reportFileName;
        public static string reportPath;
        public static string projectDirectory = Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            // Store the given path from the command line 
            string pathOfDirectory = args[0];
            // Store the name of the report file 
            reportFileName = args[1];
            // Create a path for the report document 
            reportPath = Path.Combine(projectDirectory, reportFileName); 
            // Call CreateReport() 
            CreateReport(EnumerateFilesRecursively(pathOfDirectory));

        }

        // Method that takes a path for a given directory and returns each file in the directory in lower case 
        static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            // Enumerator object for iterating through the files in a given path  
            var allFiles = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories); 
            
            // Iterate through all the files 
            foreach (string currentFile in allFiles)
            {
                yield return currentFile.ToLower(); // Return each file, and yield to the caller method 
            }

       
        }

        static void CreateReport(IEnumerable<string> files)
        {

            //XDocument dataReport = new XDocument(new XElement("body", 
            //                                         new XElement("table", 
            //                                             new XElement("tr", 
            //                                                 new XElement("th", "Type"), 
            //                                                 new XElement("th", "Count"), 
            //                                                 new XElement("th", "Size")))));


            //foreach (string file in files)
            //{
            //    FileInfo info = new FileInfo(file);
            //    long fileSize = info.Length;
            //}

            IEnumerable<IGrouping<string, string>> query = files.GroupBy(file => Path.GetExtension(file)); 

            foreach (IGrouping<string, string> grouping in query)
            {
                Console.WriteLine("Extension: " + grouping.Key); 

                foreach (string filename in grouping)
                {
                    Console.WriteLine("    - " + filename); 
                }
            }

            //dataReport.Save(reportPath); 


        }

        static string FormatByteSize(long byteSize)
        {

            const int scale = 1000; // 1kB = 1000 Bytes
            string[] sizes = new string[] { "ZB", "EB", "PB", "TB", "GB", "MB", "kB", "Bytes" }; // {1 Byte, 1kb == 1000 Bytes, 1mb == 1,000,000 Bytes, ... }
            decimal maxSize = (decimal)Math.Pow(scale, sizes.Length - 1);

            foreach (string size in sizes)
            {
                if (byteSize >= maxSize)

                    return string.Format("{0:##.##} {1}", Decimal.Divide(byteSize, maxSize), size);

                maxSize /= scale; // reduce the max
            }

            return byteSize + " Bytes"; // no max


        }
    }
}
