using System;
using System.Collections.Generic;

namespace Lopez_Auto_Sales.JSON
{
    public class JSONFile
    {
        public IList<JSONCar> Cars { get; set; }
        public DateTime Date { get; set; }

        public JSONFile(IList<JSONCar> cars)
        {
            Cars = cars;
            Date = DateTime.Today;
        }
    }

    public class JSONCar
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public bool IsSalvage { get; set; }
        public int? Mileage { get; set; }
        public decimal Price { get; set; }
        public IList<JSONExtra> Extras { get; set; }

        public JSONCar()
        {
            //needed for deserialization
        }

        public JSONCar(SalesCar car)
        {
            Extras = new List<JSONExtra>();
            Update(car);
        }

        public void Update(SalesCar car)
        {
            Year = car.Year;
            Make = car.Make;
            Model = car.Model;
            Color = car.Color;
            VIN = car.VIN;
            IsSalvage = car.Salvage;
            Price = car.Price;
            Mileage = car.Mileage;
        }
    }

    public class JSONExtra
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public JSONExtra(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}