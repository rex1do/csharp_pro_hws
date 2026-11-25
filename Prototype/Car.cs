namespace Prototype
{
    public class Car : Vehicle, IMyCloneable<Car>
    {
        public List<string> Options { get; }

        public Car(string model, string color) : base(model, color)
        {
            Options = new List<string>();
        }

        protected Car(Car source) : base(source)
        {
            Options = new List<string>(source.Options);
        }

        public override Car MyClone()
        {
            return new Car(this);
        }
    }
}
