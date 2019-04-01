using Lopez_Auto_Sales.Cars;
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

        public override string ToString()
        {
            return Name;
        }
    }
}