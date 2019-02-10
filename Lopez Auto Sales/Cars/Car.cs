namespace Lopez_Auto_Sales.Cars
{
    public class Car
    {
        public int Year { get; internal set; }
        public string Make { get; internal set; }
        public string Model { get; internal set; }
        public int? Mileage { get; internal set; }
        public string VIN { get; internal set; }
        public string Color { get; internal set; }
        public decimal Value { get; private set; }

        public Car(int year, string make, string model, string color, string vin, decimal value, int? mileage= null)
        {
            Year = year;
            Make = make;
            Model = model;
            Mileage = mileage;
            VIN = vin;
            Value = value;
            Color = color;
            Value = value;
        }

        public string Name
        {
            get { return Year.ToString() + ' ' + Make + ' ' + Model; }
        }

        public override string ToString()
        {
            return Year.ToString() + ' ' + Make + ' ' + Model;
        }
    }
}
