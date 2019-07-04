using System;

namespace Lopez_Auto_Sales
{
    /// <summary>Class for storing sales paper information</summary>
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

        /// <summary>Initializes a new instance of the <see cref="PaperInfo"/> class.</summary>
        /// <param name="date">The date.</param>
        /// <param name="buyer">The buyer.</param>
        /// <param name="coBuyer">The co buyer.</param>
        /// <param name="car">The car.</param>
        /// <param name="trade">The trade.</param>
        /// <param name="down">The down payment.</param>
        /// <param name="tag">if set to <c>true</c> [tag].</param>
        /// <param name="lien">if set to <c>true</c> [lien].</param>
        /// <param name="outofstate">if set to <c>true</c> [outofstate].</param>
        /// <param name="tax">The tax.</param>
        /// <param name="warranty">The warranty.</param>
        /// <param name="averagePayment">The average payment.</param>
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