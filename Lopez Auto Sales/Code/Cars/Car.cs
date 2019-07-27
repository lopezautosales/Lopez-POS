namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Generic Inheritable Car Class
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Gets the car year.
        /// </summary>
        /// <value>
        /// The car year.
        /// </value>
        public int Year { get; internal set; }

        /// <summary>
        /// Gets the make.
        /// </summary>
        /// <value>
        /// The make.
        /// </value>
        public string Make { get; internal set; }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public string Model { get; internal set; }

        /// <summary>
        /// Gets the mileage.
        /// </summary>
        /// <value>
        /// The mileage.
        /// </value>
        public int? Mileage { get; internal set; }

        /// <summary>
        /// Gets the vin.
        /// </summary>
        /// <value>
        /// The vin.
        /// </value>
        public string VIN { get; internal set; }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { get; internal set; }

        /// <summary>
        /// Gets the car's selling price/trade-in value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public decimal Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="make">The make.</param>
        /// <param name="model">The model.</param>
        /// <param name="color">The color.</param>
        /// <param name="vin">The vin.</param>
        /// <param name="value">The value.</param>
        /// <param name="mileage">The mileage.</param>
        public Car(int year, string make, string model, string color, string vin, decimal value, int? mileage = null)
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="make">The make.</param>
        /// <param name="model">The model.</param>
        public Car(int year, string make, string model)
        {
            Year = year;
            Make = make;
            Model = model;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return string.Format("{0} {1} {2}", Year, Make, Model); }
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}