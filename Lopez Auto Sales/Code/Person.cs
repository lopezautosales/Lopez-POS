using System.Collections.Generic;

namespace Lopez_Auto_Sales
{
    public class Person
    {
        public int PersonID { get; private set; }
        internal List<PaymentCar> Cars = new List<PaymentCar>();
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZIP { get; private set; }
        public string Full_Address { get { return Address + "," + City + "," + State + "," + ZIP; } }

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        /// <param name="personID">The person identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="full_address">The full address.</param>
        /// <param name="cars">The cars.</param>
        public Person(int personID, string name, string phone, string full_address, List<PaymentCar> cars)
        {
            Name = name;
            Cars = cars;
            Phone = phone;
            try
            {
                Address = full_address.Split(',')[0];
                City = full_address.Split(',')[1];
                State = full_address.Split(',')[2];
                ZIP = full_address.Split(',')[3];
            }
            catch { }
            PersonID = personID;
        }

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        /// <param name="personID">The person identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="address">The address.</param>
        /// <param name="city">The city.</param>
        /// <param name="state">The state.</param>
        /// <param name="zip">The zip.</param>
        /// <param name="cars">The cars.</param>
        public Person(int personID, string name, string phone, string address, string city, string state, string zip, List<PaymentCar> cars)
        {
            Name = name;
            Cars = cars;
            Phone = phone;
            Address = address;
            City = city;
            State = state;
            ZIP = zip;
            PersonID = personID;
        }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}