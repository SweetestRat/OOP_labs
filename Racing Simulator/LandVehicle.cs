namespace Racing_Simulator
{
    public class LandVehicle : Vehicle
    {
        private int restInterval;

        private double[] restDuration;

        public LandVehicle(string _name, int _speed, int _restInterval, double[] _restDuration)
        {
            Name = _name;
            Type = "land";
            Speed = _speed;
            restInterval = _restInterval;
            restDuration = _restDuration;
        }

        public override double CalcTime(double dist)
        {
            var numOfRestIntervals = dist / restInterval;

            for (var i = 0; i < numOfRestIntervals; i++)
            {
                if (dist > 0)
                {
                    Time += restInterval;
                    dist -= Speed * restInterval;
                    if (i >= restDuration.Length)
                    {
                        Time += restDuration[^1]; // preposterous index
                    }
                    else
                    {
                        Time += restDuration[i];
                    }
                }
            }

            return Time;
        }
    }
}
