using Lopez_Auto_Sales.Cars;
using System;

namespace Lopez_Auto_Sales
{
    public class PaperInfo
    {
        public DateTime Date { get; private set; }
        public Person Buyer { get; private set; }
        public string CoBuyer { get; private set; }
        public Car Car { get; private set; }
        public Car Trade { get; private set; }
        public decimal Down { get; private set; }
        public bool Tag { get; private set; }
        public bool Lien { get; private set; }
        public bool OutOfState { get; private set; }
        public int Warranty { get; private set; }
        public decimal Tax { get; private set; }
        public decimal AveragePayment { get; private set; }

        public PaperInfo(DateTime date, Person buyer, string coBuyer, Car car, Car trade, decimal down, bool tag, bool lien, bool outofstate, decimal tax, int warranty, decimal averagePayment)
        {
            Date = date;
            Buyer = buyer;
            CoBuyer = coBuyer;
            Car = car;
            Trade = trade;
            Down = down;
            Tag = tag;
            Lien = lien;
            OutOfState = outofstate;
            Tax = tax;
            AveragePayment = averagePayment;
            Warranty = warranty;
        }
    }
}
