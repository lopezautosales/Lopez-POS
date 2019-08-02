namespace Lopez_POS.JSON
{
    /// <summary>
    /// The vin decoder json result class.
    /// </summary>
    public class JSONResult
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the value identifier.
        /// </summary>
        /// <value>
        /// The value identifier.
        /// </value>
        public string ValueId { get; set; }

        /// <summary>
        /// Gets or sets the variable.
        /// </summary>
        /// <value>
        /// The variable.
        /// </value>
        public string Variable { get; set; }

        /// <summary>
        /// Gets or sets the variable identifier.
        /// </summary>
        /// <value>
        /// The variable identifier.
        /// </value>
        public int VariableId { get; set; }
    }
}