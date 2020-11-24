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
            
            if (raceType == "air")
            {
                if (vehicle.Type == "air")
                {
                    raceVehicles.Add(vehicle);
                }
                else
                {
                    throw new Exception("ERROR: Vehicle has type land. Expected: air");
                }
            }
            else if (raceType == "land")
            {
                if (vehicle.Type == "land")
                {
                    raceVehicles.Add(vehicle);
                }
                else
                {
                    throw new Exception("ERROR: Vehicle has type air. Expected: land");
                }
            }
            else if (raceType == "all")
            {
                raceVehicles.Add(vehicle);
            }
            else
            {
                throw new Exception("Unknown race type");
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