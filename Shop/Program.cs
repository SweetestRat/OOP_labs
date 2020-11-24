using System;

namespace Shop
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager();
            Product product = new Product();

            // create 3 shops
            Shop shop1 = new Shop("Speedo", "Ul. Vosstania, 6");
            manager.shops.Add(shop1.Id, shop1);

            Shop shop2 = new Shop("Mad Wave", "Bolshaya Pushkarskaya, 54");
            manager.shops.Add(shop2.Id, shop2);
            
            Shop shop3 = new Shop("Arena", "Ul. Marata, 44");
            manager.shops.Add(shop3.Id, shop3);

            // prod moves)
            Guid good1 = product.CreateProduct("SportSwimsuit"); //CREATION
            
            shop1.AddProduct(good1, 1400, product); // adding prod to shop
            shop1.AddConsignment(good1, 1450, 20, product); // adding consignment to shop
            shop2.AddProduct(good1, 1500, product); // adding prod to shop
            shop3.AddProduct(good1, 1400, product); // adding prod to shop
            shop3.AddConsignment(good1, 1450, 50, product); // adding consignment to shop

            Guid good2 = product.CreateProduct("KidsGoggles"); // CREATION
            
            shop1.AddProduct(good2, 1200, product); // adding prod to shop
            shop2.AddConsignment(good2, 1900,10, product); // adding consignment to shop
            shop3.AddConsignment(good2, 1900, 30, product); // adding consignment to shop

            Manager.cheapprodinfo cheapprodinfo = manager.FindCheapestProd("SportSwimsuit"); // finding the cheapest prod + filling in info 'bout it
            Console.WriteLine($"\nLowest price of {cheapprodinfo.ChProdName} is in a {cheapprodinfo.ChShopName} shop at {cheapprodinfo.ChShopAddress} and it costs {cheapprodinfo.ChProdPrice}");

            var sum = 3000; // declaring sum on which we wanna buy smth
            var shopForList = shop1; // choosing a shop
            
            shopForList.ProdListOnSum(sum); // getting the list of prod on exact sum
            
            Console.WriteLine($"\nList of amount of products you can buy in {shopForList.Name} on {sum}:");
            foreach (var prod in shopForList.ProdList)
            {
                var amount = sum / prod.Value;
                Console.WriteLine($"{prod.Key}: {amount} piece(-s) // {prod.Value} x {amount}");
            }

            manager.ShowAvailabilityOfGoods();
            
            Guid[] goodsForCons = {good1, good2}; // listing some of the prods we wanna buy 
            int[] goodsAmountForCons = {15, 1}; // listing their amounts
            var shopForCons = shop1; // choosing a shop
            
            var sumCons = manager.BuyConsignment(goodsForCons, goodsAmountForCons, shopForCons); // buying consignment
            
            Console.WriteLine($"\nThe sum of your purchase: {sumCons}\n");
            for(int i = 0; i < goodsForCons.Length; i++)
            {
                var id = goodsForCons[i];
                var goodSum = shopForCons.catalog[id].prodPrice * goodsAmountForCons[i];
                Console.WriteLine($"{shopForCons.catalog[id].prodName}: {shopForCons.catalog[id].prodPrice} x {goodsAmountForCons[i]} = {goodSum}");
            }

            manager.ShowAvailabilityOfGoods();
            Guid[] goodsForCheapestShop = {good1, good2}; // listing some of the goods we wanna buy at the lowest price
            int[] goodsAmountForCheapestShop = {1, 1}; // listing their amounts
            var FinalCost = 0;
            
            var CheapestShop = manager.FindCheapestShopToBuyCons(goodsForCheapestShop, goodsAmountForCheapestShop);
            
            Console.WriteLine($"\nThe lowest sum of your purchase will be in a {CheapestShop.Name} shop at {CheapestShop.Address}." +
                              "\nYour list of the cheapest products:");
            for (int i = 0; i < goodsForCheapestShop.Length; i++)
            {
                var id = goodsForCheapestShop[i];
                var goodSum = CheapestShop.catalog[id].prodPrice * goodsAmountForCheapestShop[i];
                FinalCost += goodSum;
                Console.WriteLine($"{CheapestShop.catalog[id].prodName}: {CheapestShop.catalog[id].prodPrice} x {goodsAmountForCheapestShop[i]} = {goodSum}");
            }
            Console.WriteLine($"\nFinal cost will be {FinalCost}");
        }
    }
}