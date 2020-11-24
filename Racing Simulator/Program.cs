using System;
using System.Collections.Generic;

namespace Racing_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Vehicles creation
            
            LandVehicle BactrianCamel = new LandVehicle("BactrianCamel",10, 30, new double[] {5, 8});
            LandVehicle FastCamel = new LandVehicle("FastCamel", 40, 10, new double[] {5, 6.5, 8});
            LandVehicle Kentaur = new LandVehicle("Kentaur", 15, 8, new double[] {2});
            LandVehicle AllTerrainBoots = new LandVehicle("AllTerrainBoots", 6, 60, new double[] {10, 5});
            
            Dictionary<string, double> MagicCarpetRestDuration = new Dictionary<string, double>();
            MagicCarpetRestDuration.Add("1000", 0);
            MagicCarpetRestDuration.Add("5000", 0.03);
            MagicCarpetRestDuration.Add("10000", 0.1);
            MagicCarpetRestDuration.Add("more", 0.5);
            AirVehicle MagicCarpet = new AirVehicle("MagicCarpet", 10, MagicCarpetRestDuration);
            
            Dictionary<string, double> MortarRestDuration = new Dictionary<string, double>();
            MortarRestDuration.Add("always", 0.6);
            AirVehicle Mortar = new AirVehicle("Mortar", 8, MortarRestDuration);
            
            Dictionary<string, double> BroomRestDuration = new Dictionary<string, double>();
            BroomRestDuration.Add("evenly", 0.01);
            AirVehicle Broom = new AirVehicle("Broom", 20, BroomRestDuration);
            
            // Race creation
            Race race1 = new Race(1000, "all");
            
            // Adding vehicles to race
            race1.AddVehicle(BactrianCamel);
            race1.AddVehicle(FastCamel);
            race1.AddVehicle(Kentaur);
            race1.AddVehicle(AllTerrainBoots);
            race1.AddVehicle(MagicCarpet);
            race1.AddVehicle(Mortar);
            race1.AddVehicle(Broom);
            
            // Start race
            Console.WriteLine(race1.Startrace());
        }
    }
}