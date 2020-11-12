﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Xml.Linq;


// This Program is designed to implement lazy loading. 

namespace iterators_and_generators
{
    class Program
    {
        // File name for the output HTML file 
        public static string reportFileName; 
        // Path for the output HTML file 
        public static string reportPath;
        // Current project directory path 
        public static string projectDirectory = Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            // Store the given path from the command line 
            string examinDirectoryPath = args[0];
            // Store the name of the report file 
            reportFileName = args[1];
            // Initialize the path for the report document 
            reportPath = Path.Combine(projectDirectory, reportFileName);
            // Create the report  
            CreateReport(EnumerateFilesRecursively(examinDirectoryPath));

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

            XElement dataReport = new XElement("table",
                new XElement("tr",
                    new XElement("th", "Type"),
                    new XElement("th", "Count"),
                    new XElement("th", "Size")));

            var groupedFiles = files.GroupBy(file => Path.GetExtension(file))
                                    .Select(Group => new
                                    {
                                        Extension = Group.Key,
                                        Count = Group.Count(),
                                        Size = FormatByteSize(Group.Select(file => new FileInfo(file).Length).Sum())
                                    })
                                    .OrderByDescending(groupedFile => groupedFile.Size);
  
            foreach (var groupedFile in groupedFiles) {

                dataReport.Add(new XElement("tr", new XAttribute("style", "width: 40%"),
                                    new XElement("th", groupedFile.Extension),
                                    new XElement("th", groupedFile.Count),
                                    new XElement("th", groupedFile.Size))); 
            }

            dataReport.Save(reportPath);

        }



        static string FormatByteSize(long byteSize)
        {

            const int scale = 1000; // 1kB = 1000 Bytes
            string[] sizes = new string[] { "ZB", "EB", "PB", "TB", "GB", "MB", "KB", "Bytes" }; // {1 Byte, 1kb == 1000 Bytes, 1mb == 1,000,000 Bytes, ... }
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
