using System;

namespace IniParser
{
    public class IniData
    {
        public int TryGetInt(string value)
        {
            bool success = Int32.TryParse(value, out int number);
            
            if (!success)
            {
                throw new Exception("ERROR: Failed to parse: int");
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                return number;
            }
        }

        public double TryGetDouble(string value)
        {
            
            bool success = double.TryParse(value, out double number);
            if (!success)
            {
                throw new Exception("ERROR: Failed to parse: double");
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                return number;
            }
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
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                return value;
            }
        }
    }
}