using System;
using System.Collections.Generic;

namespace Shop
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager();

            // create 3 shops
            Shop Speedo = new Shop("Speedo", "Ul. Vosstania, 6");
            manager.shops.Add(Speedo.Id, Speedo);

            Shop MadWave = new Shop("Mad Wave", "Bolshaya Pushkarskaya, 54");
            manager.shops.Add(MadWave.Id, MadWave);
            
            Shop Arena = new Shop("Arena", "Ul. Marata, 44");
            manager.shops.Add(Arena.Id, Arena);

            //create 10 products
            Product MenSportSwimsuit = new Product("Men Sport Swimsuit"); // 1
            manager.guids.Add(MenSportSwimsuit.prodId);
            Product SwimmingTrunks = new Product("Swimming Trunks"); // 2
            manager.guids.Add(SwimmingTrunks.prodId);
            Product KidsSwimsuit = new Product("Kids Swimsuit"); // 3
            manager.guids.Add(KidsSwimsuit.prodId);
            Product KidsGoggles = new Product("Kids Goggles"); // 4
            manager.guids.Add(KidsGoggles.prodId);
            Product CompetitionGoggles = new Product("Competition Goggles"); // 5
            manager.guids.Add(CompetitionGoggles.prodId);
            Product WomenSportSwimsuit = new Product("Women Sport Swimsuit"); // 6
            manager.guids.Add(WomenSportSwimsuit.prodId);
            Product WomenBeachSwimsuit = new Product("Women Beach Swimsuit"); // 7
            manager.guids.Add(WomenBeachSwimsuit.prodId);
            Product AdultPoolSlippers = new Product("Adult Pool Slippers"); // 8
            manager.guids.Add(AdultPoolSlippers.prodId);
            Product KidsPoolSlippers = new Product("Kids Pool Slippers"); // 9
            manager.guids.Add(KidsGoggles.prodId);
            Product Towel = new Product("Towel"); // 10
            manager.guids.Add(Towel.prodId);
            
            
            //fill Speedo shop with goods
            Speedo.AddProduct(MenSportSwimsuit, 2400);
            Speedo.AddProduct(SwimmingTrunks, 1800);
            Speedo.AddProduct(CompetitionGoggles, 1500);
            Speedo.AddProduct(AdultPoolSlippers, 1100);
            Speedo.AddConsignment(Towel.prodId, 400, 100, Towel);
            
            //fill MadWave shop with goods
            MadWave.AddProduct(MenSportSwimsuit, 2000);
            MadWave.AddProduct(SwimmingTrunks, 1500);
            MadWave.AddProduct(KidsSwimsuit, 1200);
            MadWave.AddProduct(KidsGoggles, 1000);
            MadWave.AddProduct(CompetitionGoggles, 1790);
            MadWave.AddConsignment(WomenSportSwimsuit.prodId, 1800, 20, WomenSportSwimsuit);
            MadWave.AddConsignment(WomenBeachSwimsuit.prodId, 1500, 3, WomenBeachSwimsuit);
            MadWave.AddConsignment(AdultPoolSlippers.prodId, 1200, 30, AdultPoolSlippers);
            MadWave.AddConsignment(KidsPoolSlippers.prodId, 1000, 2, KidsPoolSlippers);
            MadWave.AddConsignment(Towel.prodId, 500, 50, Towel);
            
            //fill Arena shop with goods
            Arena.AddProduct(KidsGoggles, 700);
            Arena.AddProduct(SwimmingTrunks, 1100);
            Arena.AddConsignment(KidsPoolSlippers.prodId, 990, 40, KidsPoolSlippers);
            Arena.AddConsignment(Towel.prodId, 600, 10, Towel);
            
            //Manager.cheapprodinfo cheapprodinfo = manager.FindCheapestProd(new Guid("73a303b4-c921-4f91-8c32-3231aa6c689a")); // finding the cheapest prod + filling in info 'bout it
            Manager.cheapprodinfo cheapprodinfo = manager.FindCheapestProd(Towel.prodId); // finding the cheapest prod + filling in info 'bout it
            Console.WriteLine($"\nLowest price of {cheapprodinfo.ChProdName} is in a {cheapprodinfo.ChShopName} shop at {cheapprodinfo.ChShopAddress} and it costs {cheapprodinfo.ChProdPrice}");

            var sum = 3000; // declaring sum on which we wanna buy smth
            var shopForList = MadWave; // choosing a shop
            
            shopForList.ProdListOnSum(sum); // getting the list of prod on exact sum

            manager.ShowAvailabilityOfGoods();
            
            // buying consignment \/
            Dictionary<Guid, int> GoodsForCons = new Dictionary<Guid, int>();
            GoodsForCons.Add(MenSportSwimsuit.prodId, 1);
            GoodsForCons.Add(Towel.prodId, 10);
            
            var shopForCons = Speedo; // choosing a shop
            
            var sumCons = manager.BuyConsignment(GoodsForCons, shopForCons); // buying consignment
            
            Console.WriteLine($"\nThe sum of your purchase: {sumCons}\n");
            foreach (var good in GoodsForCons)
            {
                var id = good.Key;
                var amount = good.Value;
                var goodSum = shopForCons.catalog[id].prodPrice * amount;
                Console.WriteLine($"{shopForCons.catalog[id].prodName}: {shopForCons.catalog[id].prodPrice} x {amount} = {goodSum}");
            }

            manager.ShowAvailabilityOfGoods();
            
            // cheapest way to buy consignment \/
            Dictionary<Guid, int> GoodsForCheapestShop = new Dictionary<Guid, int>();
            GoodsForCheapestShop.Add(MenSportSwimsuit.prodId, 1);
            GoodsForCheapestShop.Add(Towel.prodId, 1);
            var FinalCost = 0;
            
            var CheapestShop = manager.FindCheapestShopToBuyCons(GoodsForCheapestShop);
            
            Console.WriteLine($"\nThe lowest sum of your purchase will be in a {CheapestShop.Name} shop at {CheapestShop.Address}." +
                              "\nYour list of the cheapest products:");
            foreach (var good in GoodsForCheapestShop)
            {
                var id = good.Key;
                var amount = good.Value;
                
                var goodSum = CheapestShop.catalog[id].prodPrice * amount;
                FinalCost += goodSum;
                Console.WriteLine($"{CheapestShop.catalog[id].prodName}: {CheapestShop.catalog[id].prodPrice} x {amount} = {goodSum}");
            }
            Console.WriteLine($"\nFinal cost will be {FinalCost}");
        }
    }
}
