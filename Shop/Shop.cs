using System;
using System.Collections.Generic;

namespace Shop
{
    public class Shop
    {
        private string name;
        public string Name => name;

        private string address;
        public string Address => address;

        private Guid id;
        public Guid Id => id;

        public Dictionary<Guid, Product> catalog;

        public Shop(string _name, string _address)
        { 
            name = _name;
            address = _address;
            id = Guid.NewGuid();
            catalog = new Dictionary<Guid, Product>();
        }

        public void AddProduct(Product _product, int _prodPrice)
        {
            if (catalog.ContainsKey(_product.prodId))
            {
                throw new Exception("Sorry, this prod with unique id is already exist");
            }

            _product.prodPrice = _prodPrice;
            _product.prodAmount += 1;

            catalog.Add(_product.prodId, _product);
        }

        public void AddConsignment(Guid _prodid, int _prodPrice, int _prodAmount, Product _product)
        {
            if (catalog.ContainsKey(_prodid))
            {
                _product.prodAmount += _prodAmount;
                _product.prodPrice = _prodPrice;
                catalog[_prodid] = _product;
            }
            else
            {
                _product.prodAmount += _prodAmount;
                _product.prodPrice = _prodPrice;
                catalog.Add(_prodid, _product);
            }
        }

        public void ProdListOnSum(int sum)
        {
            Console.WriteLine($"\nList of amount of products you can buy in {Name} on {sum}:");
            foreach (var prod in catalog)
            {
                var amount = sum / prod.Value.prodPrice;
                Console.WriteLine($"{prod.Value.prodName}: {amount} piece(-s) // {prod.Value.prodPrice} x {amount}");
            }
        }
    }
}
