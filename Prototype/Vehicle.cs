namespace Prototype
{
    public class Vehicle : IMyCloneable<Vehicle>, ICloneable
    {
        public string Model { get; }
        public string Color { get; set; }

        public Vehicle(string model, string color)
        {
            Model = model;
            Color = color;
        }

        protected Vehicle(Vehicle source) : this(source.Model, source.Color)
        {
        }

        public virtual Vehicle MyClone()
        {
            return new Vehicle(this);
        }

        object ICloneable.Clone()
        {
            return MyClone();
        }
    }
}
