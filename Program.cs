using System;
using System.Collections.Generic;
using System.IO; 

namespace iterators_and_generators
{
    class Program
    {
        static void Main(string[] args)
        {
            //string pathOfDirectory = args[0];

            ////EnumerateFilesRecursively(pathOfDirectory);
            long byteSize = long.Parse(args[0]);
            Console.WriteLine("Enterted Byte Size: " + byteSize);
            Console.WriteLine("Formatted Byte Size: " + FormatByteSize(byteSize)); 
        }

        static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            var allFiles = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories); 
            
            foreach (string currentFile in allFiles)
            {
                Console.WriteLine(currentFile.ToLower()); 
            }

            return null;
        }

        static string FormatByteSize(long byteSize)
        {

            const int scale = 1000; // 1kB = 1000 Bytes
            string[] sizes = new string[] { "ZB", "EB", "PB", "TB", "GB", "MB", "kB", "Bytes" }; // {1 Byte, 1kb == 1000 Bytes, 1mb == 1,000,000 Bytes, ... }
            decimal maxSize = (decimal)Math.Pow(scale, sizes.Length - 1); 

            foreach(string size in sizes)
            {
                if (byteSize >= maxSize)

                    return string.Format("{0:##.##} {1}", Decimal.Divide(byteSize, maxSize), size);

                maxSize /= scale; // reduce the max
            }

            return "0 Bytes"; // no max
        }
    }
}
