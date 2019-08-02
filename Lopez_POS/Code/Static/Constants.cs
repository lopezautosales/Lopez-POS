namespace Lopez_POS.Static
{
    /// <summary>
    /// Class for holding any project constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The tax rate
        /// </summary>
        public const decimal TAX_RATE = 8.5m;

        /// <summary>
        /// The lien cost
        /// </summary>
        public const decimal LIEN_COST = 20;

        /// <summary>
        /// The tag cost
        /// </summary>
        public const decimal TAG_COST = 10;

        /// <summary>
        /// The monthly payment
        /// </summary>
        public const decimal MONTHLY_PAYMENT = 300;

        /// <summary>
        /// The warranty
        /// </summary>
        public const decimal WARRANTY = 20;

        /// <summary>
        ///
        /// </summary>
        public struct DealerInfo
        {
            /// <summary>
            /// The name
            /// </summary>
            public const string NAME = "Lopez Auto Sales";

            /// <summary>
            /// The owner
            /// </summary>
            public const string OWNER = "Gabriel Lopez";

            /// <summary>
            /// The number
            /// </summary>
            public const int NUMBER = 2518;

            /// <summary>
            /// The email
            /// </summary>
            public const string EMAIL = "lopezauto@outlook.com";
        }

        /// <summary>
        /// The states
        /// </summary>
        public static string[] States = new string[] {
         "Alabama",
         "Alaska",
        "Arizona",
        "Arkansas",
        "California",
        "Colorado",
        "Connecticut",
        "Delaware",
        "District of Columbia",
        "Florida",
        "Georgia",
        "Hawaii",
        "Idaho",
        "Illinois",
        "Indiana",
        "Iowa",
        "Kansas",
        "Kentucky",
        "Louisiana",
        "Maine",
        "Maryland",
        "Massachusetts",
        "Michigan",
         "Minnesota",
        "Mississippi",
        "Missouri",
        "Montana",
        "Nebraska",
        "Nevada",
        "New Hampshire",
        "New Jersey",
        "New Mexico",
        "New York",
        "North Carolina",
        "North Dakota",
        "Ohio",
       "Oklahoma",
        "Oregon",
        "Pennsylvania",
        "Rhode Island",
        "South Carolina",
        "South Dakota",
        "Tennessee",
        "Texas",
        "Utah",
        "Vermont",
        "Virginia",
        "Washington",
        "West Virginia",
        "Wisconsin",
        "Wyoming", };
    }
}