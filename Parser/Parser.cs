using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab1
{
    internal class Parser
    {
        private Dictionary<string, Dictionary<string, string>> _map = new Dictionary<string, Dictionary<string, string>>();
        private FileStream _file;

        public Dictionary<string, Dictionary<string, string>> GetMap() { return this._map; }
        internal Parser(string path)
        {
            
            if (!path.Contains(".ini"))
            {
                Console.WriteLine("ERROR: Wrong format");
                Environment.Exit(1);

            }
            try
            {
                _file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            ReadWithoutWaste();
        }
        private void PutMap(List<string> _file)
        {
             
             var MainKey = "";

            foreach (var i in _file)
            {
                if (i.Contains("["))
                {
                    MainKey = i;
                    MainKey = i.Replace("[", "").Replace("]", "").Replace(" ", "");

                    _map.Add($"{MainKey}", new Dictionary<string, string>());
                }
                else if (!string.IsNullOrEmpty(i))
                {
                    string name, value;
                    string[] tmp = i.Split('=');
                    name = tmp[0]
                        .Replace(" ", "")
                        .Replace("\t", "")
                        .Replace("\n", "");
                    value = tmp[1]
                        .Replace(" ", "")
                        .Replace("\t", "")
                        .Replace("\n", "")
                        .Replace(".", ",");

                    _map[$"{MainKey}"].Add($"{name}", $"{value}");
                }
                else
                {
                    
                    continue;
                }
            }
            

        }


        private void ReadWithoutWaste()
        {
            var reader = new StreamReader(_file);
            string text = "", tmp;

            while (!reader.EndOfStream)
            {

                tmp = reader.ReadLine();
                int index = tmp.IndexOf(";", StringComparison.Ordinal);

                if (index != -1)
                {
                    tmp = tmp.Remove(index, tmp.Length - index);
                }

                if (!string.IsNullOrEmpty(tmp))
                {
                    text += tmp + "\n";
                }
            }
            reader.Close();
           PutMap(text.Split('\n').ToList());

        }       
    }
}
