using System.Collections.Generic;

namespace Lopez_POS.JSON
{
    /// <summary>
    /// Class for holding json car information.
    /// </summary>
    public class JSONCar
    {
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the make.
        /// </summary>
        /// <value>
        /// The make.
        /// </value>
        public string Make { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the vin.
        /// </summary>
        /// <value>
        /// The vin.
        /// </value>
        public string VIN { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is salvage.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is salvage; otherwise, <c>false</c>.
        /// </value>
        public bool IsSalvage { get; set; }

        /// <summary>
        /// Gets or sets the mileage.
        /// </summary>
        /// <value>
        /// The mileage.
        /// </value>
        public int? Mileage { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the extras.
        /// </summary>
        /// <value>
        /// The extras.
        /// </value>
        public IList<JSONExtra> Extras { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONCar"/> class.
        /// </summary>
        public JSONCar()
        {
            //needed for deserialization
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONCar"/> class.
        /// </summary>
        /// <param name="car">The car.</param>
        public JSONCar(SalesCar car)
        {
            Extras = new List<JSONExtra>();
            Update(car);
        }

        /// <summary>
        /// Updates the specified car.
        /// </summary>
        /// <param name="car">The car.</param>
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
}