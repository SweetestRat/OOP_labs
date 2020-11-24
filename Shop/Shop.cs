using System;
using System.Collections.Generic;

namespace Shop
{
    public class Shop
    {
        // List<Product> consignment = new List<Product>();

        private string name;
        public string Name
        {
            get => name;
            set => name = value;
        }
        
        private string address;
        public string Address
        {
            get => address;
            set => address = value;
        }

        private Guid id;
        public Guid Id
        {
            get => id;
            set => id = value;
        }
        
        public Dictionary<Guid, Product> catalog;

        Manager mngr = new Manager();
        public Shop(string _name, string _address)
        { 
            name = _name;
            address = _address;
            id = Guid.NewGuid();
            catalog = new Dictionary<Guid, Product>();
        }

        public void AddProduct(Guid _prodid, int _prodPrice, Product _product)
        {
            if (catalog.ContainsKey(_prodid))
            {
                throw new Exception("Sorry, this prod with unique id is already exist");
            }
            
            Product prod = new Product();
            
            prod.prodPrice = _prodPrice;
            prod.prodAmount = 1;
            prod.prodName = _product.prodName;
            // product.prodid = _prodid;
            
            catalog.Add(_prodid, prod);
        }

        public void AddConsignment(Guid _prodid, int _prodPrice, int _prodAmount, Product _product)
        {
            if (catalog.ContainsKey(_prodid))
            {
                Product prod = new Product();
                
                prod.prodAmount += _prodAmount;
                prod.prodPrice = _prodPrice;
                prod.prodName = _product.prodName;
                catalog[_prodid] = prod;
            }
            else
            {
                Product prod = new Product();
                
                prod.prodAmount += _prodAmount;
                prod.prodPrice = _prodPrice;
                prod.prodName = _product.prodName;
                catalog.Add(_prodid, prod);
            }
        }

        public List<KeyValuePair<string, int>> ProdList = new List<KeyValuePair<string, int>>();
        public List<KeyValuePair<string, int>> ProdListOnSum(int _sum)
        {
            foreach (var prod in catalog.Values)
            {
                int amount = _sum / prod.prodPrice;
                
                ProdList.Add(new KeyValuePair<string, int>(prod.prodName, prod.prodPrice));
            }

            return ProdList;
        }
    }
}