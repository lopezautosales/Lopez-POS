using System;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Class for storing sales paper information
    /// </summary>
    public class PaperInfo
    {
        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the buyer.
        /// </summary>
        /// <value>
        /// The buyer.
        /// </value>
        public Person Buyer { get; private set; }

        /// <summary>
        /// Gets the co buyer.
        /// </summary>
        /// <value>
        /// The co buyer.
        /// </value>
        public string CoBuyer { get; private set; }

        /// <summary>
        /// Gets the car.
        /// </summary>
        /// <value>
        /// The car.
        /// </value>
        public Car Car { get; private set; }

        /// <summary>
        /// Gets the trade.
        /// </summary>
        /// <value>
        /// The trade.
        /// </value>
        public Car Trade { get; private set; }

        /// <summary>
        /// Gets down.
        /// </summary>
        /// <value>
        /// Down.
        /// </value>
        public decimal Down { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PaperInfo"/> is tag.
        /// </summary>
        /// <value>
        ///   <c>true</c> if tag; otherwise, <c>false</c>.
        /// </value>
        public bool Tag { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PaperInfo"/> is lien.
        /// </summary>
        /// <value>
        ///   <c>true</c> if lien; otherwise, <c>false</c>.
        /// </value>
        public bool Lien { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [out of state].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [out of state]; otherwise, <c>false</c>.
        /// </value>
        public bool OutOfState { get; private set; }

        /// <summary>
        /// Gets the warranty.
        /// </summary>
        /// <value>
        /// The warranty.
        /// </value>
        public int Warranty { get; private set; }

        /// <summary>
        /// Gets the tax.
        /// </summary>
        /// <value>
        /// The tax.
        /// </value>
        public decimal Tax { get; private set; }

        /// <summary>
        /// Gets the average payment.
        /// </summary>
        /// <value>
        /// The average payment.
        /// </value>
        public decimal AveragePayment { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperInfo" /> class.
        /// </summary>
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