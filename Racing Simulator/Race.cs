using System;
using System.Collections.Generic;

namespace Racing_Simulator
{
    public class Race
    {
        private List<Vehicle> raceVehicles;

        private string raceType;

        private double distance;

        public Race(int _distance, string _type)
        {
            distance = _distance;
            raceType = _type;
            raceVehicles = new List<Vehicle>();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if (raceVehicles.Contains(vehicle))
            {
                throw new Exception("ERROR: you have already registered this vehicle type for the race");
            }
            if (raceType == vehicle.Type)
            {
                raceVehicles.Add(vehicle);
            }
            else if (raceType == vehicle.Type)
            {
                raceVehicles.Add(vehicle);
            }
            else
            {
                throw new Exception($"ERROR: Vehicle has type {vehicle.Type}. Expected: {raceType}");
            }
        }

        public string Startrace()
        {
            double fastestTime = Double.MaxValue;
            string fastestVehicle = "smone";
            
            foreach (var vehicle in raceVehicles)
            {
                vehicle.CalcTime(distance);
                
                if (vehicle.Time < fastestTime)
                {
                    fastestTime = vehicle.Time;
                    fastestVehicle = vehicle.Name;
                }
            }

            return $"The winner is {fastestVehicle} [time: {fastestTime}]";
        }
    }
}
