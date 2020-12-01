using System;
using System.Collections.Generic;

namespace Shop
{
    public class Manager
    {
        public Dictionary<Guid, Shop> shops = new Dictionary<Guid, Shop>();
        public List<Guid> guids = new List<Guid>();

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
        
        public cheapprodinfo FindCheapestProd(Guid _prodId)
        {
            if (!guids.Contains(_prodId))
            {
                throw new Exception("ERROR: Such product doesn't exist");
            }
            cheapprodinfo cheapprodinfo = default;
            int price = Int32.MaxValue;

            foreach (var curShop in shops)
            {
                foreach (var value in curShop.Value.catalog.Values)
                {
                    if (value.prodId == _prodId)
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

            // if (cheapprodinfo.ChProdPrice == 0)
            // {
            //     throw new Exception("ERROR: Such product doesn't exist");
            // }
            return cheapprodinfo;
        }
        
        public int BuyConsignment(Dictionary<Guid, int> GoodsForCons, Shop shop)
        {
            int sum = 0;

            foreach (var good in GoodsForCons)
            {
                if (shop.catalog.ContainsKey(good.Key))
                {
                    var id = good.Key;
                    var amount = good.Value;
                    if (amount <= shop.catalog[id].prodAmount)
                    {
                        shop.catalog[id].prodAmount -= amount;
                        sum += amount * shop.catalog[id].prodPrice;
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
        
        public Shop FindCheapestShopToBuyCons(Dictionary<Guid, int> GoodsForCheapestShop)
        {
            var minPrice = Int32.MaxValue;
            
            foreach (var curShop in shops)
            {
                foreach (var good in GoodsForCheapestShop)
                {
                    var id = good.Key;
                    var amount = good.Value;
                    
                    if (curShop.Value.catalog.ContainsKey(id))
                    {
                        var PriceInShop = curShop.Value.catalog[id].prodPrice * amount;

                        if (PriceInShop < minPrice)
                        {
                            minPrice = PriceInShop;
                            CheapestShop = curShop.Value;
                        }
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
