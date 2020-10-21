using System;
using System.Collections.Generic;
using System.IO;


namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = @"\Users\Рашид\Desktop\с#\lab1\lab1\test.ini";

            Data data = new Data(path);

            
            
            foreach (KeyValuePair<string, Dictionary<string, string>> keyValue in data.GetMap())
            {
                foreach (KeyValuePair<string, string> keyValuePair in keyValue.Value)
                {
                    Console.WriteLine(keyValue.Key + " " + keyValuePair.Key + " " + keyValuePair.Value);
                }
            }
            

            string command = "";
            while (command != "stop")
            {
                Console.Write("Enter the command: ");
                command = Console.ReadLine();

                if (command == "GetInt")
                {
                    int result = data.TryGetInt(Console.ReadLine(), Console.ReadLine());

                    Console.WriteLine($"Result: {result}");
                }
                else if (command == "GetDb")
                {
                    double result = data.TryGetDouble(Console.ReadLine(), Console.ReadLine());

                    Console.WriteLine($"Result: {result}");
                }
                else if (command == "GetStr")
                {
                    string result = data.TryGetString(Console.ReadLine(), Console.ReadLine());

                    Console.WriteLine($"Result: {result}");
                }
            }
        }
    }
    
}
