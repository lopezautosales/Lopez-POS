using System;
using System.Collections.Generic;

namespace Lopez_POS.JSON
{
    /// <summary>
    /// Class for creating website json file.
    /// </summary>
    public class JSONFile
    {
        /// <summary>
        /// Gets or sets the cars.
        /// </summary>
        /// <value>
        /// The cars.
        /// </value>
        public IList<JSONCar> Cars { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONFile"/> class.
        /// </summary>
        /// <param name="cars">The cars.</param>
        public JSONFile(IList<JSONCar> cars)
        {
            Cars = cars;
            Date = DateTime.Today;
        }
    }
}