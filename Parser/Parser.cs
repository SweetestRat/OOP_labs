using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace IniParser
{
    public class Parser
    {
        private List<Tuple<string, string, string>> inidata = new List<Tuple<string, string, string>>();
        
        Grammar grammar = new Grammar();
        
        public string path = "/Users/vladislavagilde/RiderProjects/INI_parser/IniParser/inifile.ini";

        private int position = 0;
        private string sect;
        private string[] elements;
        
        public void Parsing(string line)
        {
            if (Regex.IsMatch(line, grammar.section))
            {
                line = line.Remove(0, 1);
                line = line.Remove(line.Length - 1, 1);
                sect = line;
            }
            else if (Regex.IsMatch(line, grammar.key) &&
                    (Regex.IsMatch(line, grammar.valueInt) ||
                    Regex.IsMatch(line, grammar.valueFloat) ||
                    Regex.IsMatch(line, grammar.valueString)))
            {
                elements = line.Split('=');

                var tuple = Tuple.Create(sect, elements[0].Remove(elements[0].Length - 1, 1), elements[1].Remove(0, 1));
                inidata.Add(tuple);
                
            }
        }
        private bool CommentExist(string line)
        {
            position = line.IndexOf(';', 0);
            
            return position != -1;
        }
        
        public string DeleteComments(string line)
        {
            if (CommentExist(line)) 
            { 
                line = line.Remove(position);
            }
            return line;
        }

        public string GetValue(string Section, string Key)
        {
            foreach (var item in inidata)
            {
                if (item.Item1 == Section)
                {
                    if (item.Item2 == Key)
                    {
                        return item.Item3;
                    }
                }
            }
            return null;
        }
        
        public int TryGetInt(string value)
        {
            if (value == null)
            {
                throw new Exception("ERROR: No such key or section");
            }
            
            bool success = Int32.TryParse(value, out int number);
            
            if (!success)
            {
                throw new Exception("ERROR: Failed to parse: int");
            }
            return number;
        }

        public double TryGetDouble(string value)
        {
            
            bool success = double.TryParse(value, out double number);
            if (!success)
            {
                throw new Exception("ERROR: Failed to parse: double");
            }
            return number;
        }
        
        public string TryGetString(string value)
        {
            if (Int32.TryParse(value, out int numberInt))
            {
                throw new Exception("ERROR: Failed to parse: string");
            }
            // ReSharper disable once RedundantIfElseBlock
            else if (Double.TryParse(value, out double numberDouble))
            {
                throw new Exception("ERROR: Failed to parse: string");
            }
            return value;
        }
    }
}
