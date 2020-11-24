using System.Collections.Generic;
using System;

namespace Racing_Simulator
{
    public class AirVehicle : Vehicle
    {
        private Dictionary<String, double> distanceReducer;

        public AirVehicle(string _name, int _speed, Dictionary<String, double> _distanceReducer)
        {
            Name = _name;
            Type = "air";
            Speed = _speed;
            distanceReducer = _distanceReducer;
        }

        public override void CalcTime(double dist)
        {
            foreach (var pair in distanceReducer)
            {
                if (pair.Key == "1000")
                {
                    // dist -= 1000;
                    Time += 1000 / Speed;
                    
                }
                else if (pair.Key == "5000")
                {
                    if (dist > 1000)
                    {
                        dist -= 1000;
                        Time += dist * distanceReducer["5000"] / Speed;
                    }
                }
                else if (pair.Key == "10000")
                {
                    if (dist > 5000)
                    {
                         dist -= 4000;
                        Time += dist * distanceReducer["10000"] / Speed;
                    }
                }
                else if (pair.Key == "more")
                {
                    if (dist > 10000)
                    {
                        Time += dist * distanceReducer["more"] / Speed;
                        dist = 0;
                    }
                }
                else if (pair.Key == "always")
                {
                    Time += dist * distanceReducer["always"] / Speed;
                    dist = 0;
                }
                else if (pair.Key == "evenly")
                {
                    var r = dist / 1000;

                    while (dist > 0)
                    {
                        Time += 1000 * distanceReducer["evenly"] / Speed;
                        dist -= 1000;
                    }
                }
                else
                {
                    throw new Exception("ERROR: incorrect data for time calculations");
                }
            }
        }
        
    }
}