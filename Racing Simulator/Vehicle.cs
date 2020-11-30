namespace Racing_Simulator
{
    public abstract class Vehicle
    {
        public string Name;
        
        private int speed;

        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        private string type;

        public string Type
        {
            get => type;
            set => type = value;
        }

        private double time;

        public double Time
        {
            get => time;
            set => time = value;
        }

        public abstract double CalcTime(double dist);
    }
}
