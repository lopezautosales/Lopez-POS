namespace Lopez_Auto_Sales
{
    public class JsonCar
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Series { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public string DriveType { get; set; }
        public string VehicleType { get; set; }
        public bool IsSalvage { get; set; }
        public string FuelType { get; set; }
        public string Displacement { get; set; }
        public int? NumDoors { get; set; }
        public int? NumCylinders { get; set; }
        public int? Mileage { get; set; }
        public decimal Price { get; set; }

        public JsonCar(SalesCar car)
        {
            Color = car.Color;
            VIN = car.VIN;
            IsSalvage = car.Salvage;
            Price = car.Price;
            Mileage = car.Mileage;
        }
    }
}