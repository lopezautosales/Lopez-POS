namespace Lopez_POS.JSON
{
    /// <summary>
    /// Class for holding extra json information.
    /// </summary>
    public class JSONExtra
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONExtra"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public JSONExtra(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}