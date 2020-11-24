using System;
using System.Collections.Generic;

namespace Shop
{
    public class Manager
    {
        public Dictionary<Guid, Shop> shops = new Dictionary<Guid, Shop>();

        public struct cheapprodinfo
        {
            private string chshopname;
            public string ChShopName 
            {
                get => chshopname;
                set => chshopname = value;
            }
            
            private string chshopaddress;
            public string ChShopAddress
            {
                get => chshopaddress;
                set => chshopaddress = value;
            }

            private string chprodname;
            public string ChProdName
            {
                get => chprodname;
                set => chprodname = value;
            }
            
            private int chprodprice;
            public int ChProdPrice
            {
                get => chprodprice;
                set => chprodprice = value;
            }
        }
        
        public cheapprodinfo FindCheapestProd(string prodName)
        {
            cheapprodinfo cheapprodinfo = default;
            int price = Int32.MaxValue;

            foreach (var curShop in shops)
            {
                foreach (var value in curShop.Value.catalog.Values)
                {
                    if (value.prodName == prodName)
                    {
                        if (value.prodPrice < price)
                        {
                            cheapprodinfo.ChShopName = curShop.Value.Name;
                            cheapprodinfo.ChShopAddress = curShop.Value.Address;
                            cheapprodinfo.ChProdName = value.prodName;
                            cheapprodinfo.ChProdPrice = value.prodPrice;
                            price = value.prodPrice;
                        }
                    }
                }
            }

            if (cheapprodinfo.ChProdPrice == 0)
            {
                throw new Exception("ERROR: Such product doesn't exist");
            }
            return cheapprodinfo;
        }
        
        public int BuyConsignment(Guid[] goods, int[] amount, Shop shop)
        {
            int sum = 0;

            for (int i = 0; i < goods.Length; i++)
            {
                if (shop.catalog.ContainsKey(goods[i]))
                {
                    var id = goods[i];
                    if (amount[i] <= shop.catalog[id].prodAmount)
                    {
                        shop.catalog[id].prodAmount -= amount[i];
                        sum += amount[i] * shop.catalog[id].prodPrice;
                    }
                    else
                    {
                        throw new Exception("Not enough goods you wanna buy, sorry :(");
                    }
                }
                else
                {
                    throw new Exception("ERROR: No such product exist");
                }
            }

            return sum;
        }

        private Shop CheapestShop;
        
        public Shop FindCheapestShopToBuyCons(Guid[] goods, int[] amount)
        {
            var minPrice = Int32.MaxValue;
            
            foreach (var curShop in shops)
            {
                for (int i = 0; i < goods.Length; i++)
                {
                    if (curShop.Value.catalog.ContainsKey(goods[i]))
                    {
                        var id = goods[i];
                        var PriceInShop = curShop.Value.catalog[id].prodPrice * amount[i];

                        if (PriceInShop < minPrice)
                        {
                            minPrice = PriceInShop;
                            CheapestShop = curShop.Value;
                        }
                    }
                    else
                    {
                        throw new Exception("ERROR: No such product exist");
                    }
                }
            }
            
            return CheapestShop;
        }
        
        public void ShowAvailabilityOfGoods()
        {
            Console.WriteLine("\n-----AVALIABILITY OF GOODS-----");
            foreach (var curShop in shops)
            {
                Console.WriteLine($"\nShop Name: {curShop.Value.Name}");

                foreach (var good in curShop.Value.catalog)
                {
                    Console.WriteLine($"{good.Value.prodName}: {good.Value.prodAmount}");
                }
            }
            Console.WriteLine("\n---------------------------------");
        }
    }
}