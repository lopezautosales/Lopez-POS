using System;

namespace Lopez_POS
{
    /// <summary>
    /// The Sales Car Class
    /// </summary>
    /// <seealso cref="Lopez_POS.Car" />
    public class SalesCar : Car
    {
        /// <summary>
        /// Gets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; private set; }

        /// <summary>
        /// Gets the lowest price.
        /// </summary>
        /// <value>
        /// The lowest price.
        /// </value>
        public decimal LowestPrice { get; private set; }

        /// <summary>
        /// Gets the bought price.
        /// </summary>
        /// <value>
        /// The bought price.
        /// </value>
        public decimal BoughtPrice { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [extra key].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [extra key]; otherwise, <c>false</c>.
        /// </value>
        public bool ExtraKey { get; private set; }

        /// <summary>
        /// Gets the list date.
        /// </summary>
        /// <value>
        /// The list date.
        /// </value>
        public DateTime ListDate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SalesCar"/> is salvage.
        /// </summary>
        /// <value>
        ///   <c>true</c> if salvage; otherwise, <c>false</c>.
        /// </value>
        public bool Salvage { get; private set; } = false;

        /// <summary>
        /// Gets a value indicating whether [by owner].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [by owner]; otherwise, <c>false</c>.
        /// </value>
        public bool ByOwner { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesCar"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="make">The make.</param>
        /// <param name="model">The model.</param>
        /// <param name="vin">The vin.</param>
        /// <param name="mileage">The mileage.</param>
        /// <param name="price">The price.</param>
        /// <param name="lowestPrice">The lowest price.</param>
        /// <param name="listDate">The list date.</param>
        /// <param name="color">The color.</param>
        /// <param name="boughtPrice">The bought price.</param>
        /// <param name="salvage">if set to <c>true</c> [salvage].</param>
        /// <param name="extraKey">if set to <c>true</c> [extra key].</param>
        /// <param name="byOwner">if set to <c>true</c> [by owner].</param>
        public SalesCar(int year, string make, string model, string vin, int? mileage, decimal price, decimal lowestPrice, DateTime listDate, string color, decimal boughtPrice, bool salvage, bool extraKey, bool byOwner) : base(year, make, model, color, vin, price, mileage)
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
            ByOwner = byOwner;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return VIN;
        }
    }
}