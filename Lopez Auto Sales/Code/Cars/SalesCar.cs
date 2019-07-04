using System;

namespace Lopez_Auto_Sales
{
    public class SalesCar : Car
    {
        public decimal Price { get; private set; }
        public decimal LowestPrice { get; private set; }
        public decimal BoughtPrice { get; private set; }
        public bool ExtraKey { get; private set; }
        public DateTime ListDate { get; private set; }
        public bool Salvage { get; private set; }

        public SalesCar(int year, string make, string model, string vin, int? mileage, decimal price, decimal lowestPrice, DateTime listDate, string color, decimal boughtPrice, bool salvage, bool extraKey) : base(year, make, model, color, vin, price, mileage)
        {
            Year = year;
            Make = make;
            Model = model;
            VIN = vin;
            Mileage = mileage;
            Price = price;
            LowestPrice = lowestPrice;
            ListDate = listDate;
            Color = color;
            BoughtPrice = boughtPrice;
            Salvage = salvage;
            ExtraKey = extraKey;
        }

        public override string ToString()
        {
            return VIN;
        }
    }
}