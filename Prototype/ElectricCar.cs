namespace Prototype
{
    public sealed class ElectricCar : Car, IMyCloneable<ElectricCar>
    {
        public int BatteryCapacityKWh { get; }

        public ElectricCar(string model, string color, int batteryCapacityKWh) : base(model, color)
        {
            BatteryCapacityKWh = batteryCapacityKWh;
        }

        private ElectricCar(ElectricCar source) : base(source)
        {
            BatteryCapacityKWh = source.BatteryCapacityKWh;
        }

        public override ElectricCar MyClone()
        {
            return new ElectricCar(this);
        }
    }
}
