﻿using System;
using System.IO;

namespace IniParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            IniData data = new IniData();

            if (!File.Exists(parser.path))
            {
                throw new Exception("ERROR: File does not exist");
            }

            int index = parser.path.IndexOf('.');
            string extencion = parser.path.Substring(index, 4);
            if (extencion != ".ini")
            {
                throw new Exception("ERROR: Invalid file extencion");
            }
            
            string[] lines = File.ReadAllLines(parser.path);
            foreach (var line in lines)
            {
                string editline = parser.DeleteComments(line);
                parser.Parsing(editline);
            }
            
            Console.WriteLine(data.TryGetInt(parser.GetValue("SECTION1", "nameI")));
            Console.WriteLine(data.TryGetInt(parser.GetValue("SECTION1", "nameD"))); //ERROR: Failed to parse: int
            Console.WriteLine(data.TryGetString(parser.GetValue("SECTION2", "nameS")));
        }
    }
}
