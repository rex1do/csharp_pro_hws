namespace Prototype
{
    class Program
    {
        static void Main()
        {
            var vehicle = new Vehicle("Скутер", "белый");
            var car = new Car("Универсал", "серый");
            car.Options.Add("люк");
            var electricCar = new ElectricCar("Электрохэтчбек", "синий", 60);
            electricCar.Options.Add("камера заднего вида");

            CheckBothInterfaces(vehicle);
            CheckBothInterfaces(car);
            ElectricCar winterEdition = CheckBothInterfaces(electricCar);
            winterEdition.Color = "красный";
            winterEdition.Options.Add("подогрев сидений");

            ShowConfiguration("Базовая комплектация", electricCar);
            ShowConfiguration("Зимняя комплектация", winterEdition);

            Vehicle prototype = electricCar;
            CheckCopy(prototype, prototype.MyClone());
        }

        static T CheckBothInterfaces<T>(T original) where T : Vehicle, IMyCloneable<T>
        {
            IMyCloneable<T> prototype = original;
            T copy = prototype.MyClone();
            CheckCopy(original, copy);

            ICloneable standard = original;
            CheckCopy(original, (Vehicle)standard.Clone());
            Console.WriteLine($"{typeof(T).Name}: проверены оба интерфейса.");
            return copy;
        }

        static void CheckCopy(Vehicle original, Vehicle copy)
        {
            if (ReferenceEquals(original, copy) || original.GetType() != copy.GetType())
            {
                throw new InvalidOperationException("Ожидался отдельный объект того же типа.");
            }

            if (original.Model != copy.Model || original.Color != copy.Color)
            {
                throw new InvalidOperationException("При копировании изменилась модель или цвет.");
            }

            if (original is Car car && copy is Car carCopy)
            {
                if (ReferenceEquals(car.Options, carCopy.Options) || !car.Options.SequenceEqual(carCopy.Options))
                {
                    throw new InvalidOperationException("У копии должен быть свой список с такими же опциями.");
                }
            }

            if (original is ElectricCar electricCar && copy is ElectricCar electricCopy &&
                electricCar.BatteryCapacityKWh != electricCopy.BatteryCapacityKWh)
            {
                throw new InvalidOperationException("При копировании изменилась ёмкость батареи.");
            }
        }

        static void ShowConfiguration(string title, ElectricCar car)
        {
            Console.WriteLine($"{title}: {car.Model}, {car.Color}, {car.BatteryCapacityKWh} кВт/ч. " +
                $"Опции: {string.Join(", ", car.Options)}.");
        }
    }
}
