// Program.cs
// 
// CECS 342
//
// Group 6: Kevin Patel, Kevin Vu, Harry Tran, Tymee Kong, Jessica Wei, Aaron Ramirez
// 
// This Program is designed to implement lazy loading by using enumerables and yield return statements

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;


namespace iterators_and_generators
{
    class GeneratorProgram
    {
        // File name for the output HTML file 
        public static string reportFileName; 

        // Path for the output HTML file 
        public static string reportPath;

        // Path of the current project directory  
        public static string projectDirectory = Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            // Store the given path that will be examined  
            string examinDirectoryPath = args[0];

            // Store the name of the report file 
            reportFileName = args[1];

            // Initialize the path for the report document 
            reportPath = Path.Combine(projectDirectory, reportFileName);
             
            // Enumerate the files using a generator 
            IEnumerable<string> files = EnumerateFilesRecursively(examinDirectoryPath); 

            // Create a report and invoke the generator for grouping files 
            // based on their extension types
            XDocument dataReport = CreateReport(files);

            // Save the report to the project directory 
            dataReport.Save(reportPath); 

        }

        // Function that takes a path for a given directory and yields after returning 
        // each file in the directory 
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

        // Function that returns an XML document in the format of a table 
        static XDocument CreateReport(IEnumerable<string> files)
        {
            return new XDocument(new XElement("table",                                  // table element 
                                 new XElement("thead",                                  // table header
                                 new XElement("tr",                                     // table row 
                                 new XElement("th", "Type"),                            // table column
                                 new XElement("th", "Count"),                           // table column
                                 new XElement("th", "Size"),                            // table column
                                 new XElement(AddReportData(files))))));                // Construct table body by invoking AddReportData()
        }

        // Function that takes in files from EnumerateFilesRecursively() function 
        // and groups each of the files based on file extension. After query results, 
        // the function will add the results to the table body element and return the 
        // the new element to the caller 
        static XElement AddReportData(IEnumerable<string> files)
        {
            // Iterate the given files and group each file based on file extension
            var groupedFiles = files.GroupBy(file => Path.GetExtension(file))
                                    .Select(Group => new // Construct and anonymous object 
                                    {
                                        Extension = Group.Key, // Specifies the extension type for a group of files 
                                        Count = Group.Count(), // Specifies the total count for a single group of files 
                                        FormattedSize = FormatByteSize(Group.Select(file => new FileInfo(file).Length).Sum()), // Formats the sum of file sizes for a single group of files 
                                        TotalSizeInBytes = Group.Select(file => new FileInfo(file).Length).Sum() // Calculates the sum of file sizes for a single group of files 
                                    })
                                    .OrderByDescending(groupedFile => groupedFile.TotalSizeInBytes); // Order groups of files based on total size in descending order 

            // Wrapper for appending child row elements consisting of data for the report 
            XElement dataElementWrapper = new XElement("tbody");


            foreach(var groupedFile in groupedFiles)
            {
                dataElementWrapper.Add(new XElement("tr", new XAttribute("style", "width: 40%"), // Add a width attrbute for the rows to create space between the data 
                                         new XElement("td", groupedFile.Extension),              // Table data: file extension
                                         new XElement("td", groupedFile.Count),                  // Table data: group file count 
                                         new XElement("td", groupedFile.FormattedSize)));        // Table data: formatted size in bytes of group file data 
            }
            
            // return the wrapper element back to the caller 
            // to finish constructing the report 
            return dataElementWrapper; 
        }

        // Takes the given bytesize for a group of files 
        // and returns a string formatted byte size conversion 
        static string FormatByteSize(long byteSize)
        {

            const int scale = 1000; // 1kB = 1000 Bytes
            string[] sizes = new string[] { "ZB", "EB", "PB", "TB", "GB", "MB", "KB", "Bytes" }; // {1 Byte, 1kb == 1000 Bytes, 1mb == 1,000,000 Bytes, ... }
            decimal maxSize = (decimal)Math.Pow(scale, sizes.Length - 1); // used for storing the maxsize of byte conversion, i.e. 1 TB = (1000)^4

            // Loop through each of the sizes from the array starting from the hightest value (1 ZB)
            // and find where the given byte size lands
            // e.g., if the given byte size is greater than 1 ZB, the returned string should be formatted as a ZB
            foreach (string size in sizes)
            {
                if (byteSize >= maxSize)
                    return string.Format("{0:##.##} {1}", Decimal.Divide(byteSize, maxSize), size); // formatted bytesize according to the curerent max size 

                maxSize /= scale; // reduce the max in case the given bytesize is smaller then the max size 
            }

            return byteSize + " Bytes"; // the value of the given byte size is < 1000 

        }
    }
}
