using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace IniParser
{
    public class Parser
    {
        public List<Tuple<string, string, string>> inidata = new List<Tuple<string, string, string>>();
        
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
            else if (line.Contains("="))
            {
                if (Regex.IsMatch(line, grammar.key) &&
                    (Regex.IsMatch(line, grammar.valueInt) ||
                     Regex.IsMatch(line, grammar.valueFloat) ||
                     Regex.IsMatch(line, grammar.valueString)))
                {
                    elements = line.Split('=', 2);

                    if (elements[0].Trim() != String.Empty && elements[1].Trim() != String.Empty)
                    {
                        var tuple = Tuple.Create(sect, elements[0].Trim(), elements[1].Trim());
                        inidata.Add(tuple);
                    }
                    else
                    {
                        throw new Exception("ERROR: Invalid file structure");
                    }
                }
            }
            else
            {
                throw new Exception("ERROR: Invalid file structure");
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
        
        public int TryGetInt(string Section, string Key)
        {
            string value = GetValue(Section, Key);
            
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

        public double TryGetDouble(string Section, string Key)
        {
            string value = GetValue(Section, Key);
            
            if (value == null)
            {
                throw new Exception("ERROR: No such key or section");
            }
            
            bool success = double.TryParse(value, out double number);
            if (!success)
            {
                throw new Exception("ERROR: Failed to parse: double");
            }
            return number;
        }
        
        public string TryGetString(string Section, string Key)
        {
            string value = GetValue(Section, Key);
            
            if (value == null)
            {
                throw new Exception("ERROR: No such key or section");
            }
            
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
