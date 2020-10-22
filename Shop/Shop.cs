using System;
using System.Collections;
using System.Collections.Generic;

namespace Shop
{
    public class Shop
    {
        private string name;
        private int uniqueCode;
        
        public Shop(string _name, int _uniqueCode)
        { 
            name = _name; 
            uniqueCode = _uniqueCode;
            Dictionary<int, Tuple<string, int, int>> product = new Dictionary<int, Tuple<string, int, int>>();
        }

        void Add()
        {
            
        }
    }
}