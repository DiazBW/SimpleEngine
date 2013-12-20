using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace LinkParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var dirInfo = new DirectoryInfo("FilesWithLinks");
            var res = ProcessDirsWithInner(dirInfo);

            SaveToFile("Links.txt", res);
        }

        private static void SaveToFile(string linksTxt, List<string> links)
        {
            FileInfo outputFile = new FileInfo(linksTxt);
            try
            {
                using (var fs = outputFile.OpenWrite())
                {
                    using (var writer = new StreamWriter(fs))
                    {
                        links.ForEach(line => writer.WriteLine(line));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error with output file " + e.Message);
            }
        }

        private static List<string> ProcessDirsWithInner(DirectoryInfo dirInfo)
        {
            var files = dirInfo.EnumerateFiles();
            var directories = dirInfo.EnumerateDirectories();

            var res = new List<String>();
            foreach (var fileInfo in files)
            {
                var linksFromFile = GetLinesWithLinks(fileInfo);
                res.AddRange(linksFromFile);
            }

            foreach (var dir in directories)
            {
                res.AddRange(ProcessDirsWithInner(dir));
            }
            return res;
        }


        private static List<string> GetLinesWithLinks(FileInfo fileInfo)
        {
            var res = new List<string>();

            try
            {
                using (FileStream fs = fileInfo.OpenRead())
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        var line = String.Empty;
                        while (line != null)
                        {
                            if (IsStringContainsLink(line))
                                res.Add(line);
                            line = reader.ReadLine();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error with file " + fileInfo.Name );
            }

            return res;
        }

        private static bool IsStringContainsLink(string str)
        {
            if (str.Contains("http"))
                return true;
            if (str.Contains("www"))
                return true;
            return false;
        }
    }
}
