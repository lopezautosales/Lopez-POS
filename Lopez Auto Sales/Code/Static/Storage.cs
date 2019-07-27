using Lopez_Auto_Sales.LopezDataDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Lopez_Auto_Sales.Static
{
    internal static class Storage
    {
        internal static LopezDataDataSet lopezData = new LopezDataDataSet();
        private static PeopleTableTableAdapter peopleAdapter = new PeopleTableTableAdapter();
        private static PaymentCarTableTableAdapter paymentCarAdapter = new PaymentCarTableTableAdapter();
        private static SalesCarTableTableAdapter salesCarAdapter = new SalesCarTableTableAdapter();
        private static PaymentTableTableAdapter paymentAdapter = new PaymentTableTableAdapter();
        private static PaperInfoTableTableAdapter paperInfoAdapter = new PaperInfoTableTableAdapter();
        private static RemovedPaymentTableTableAdapter removedPaymentAdapter = new RemovedPaymentTableTableAdapter();
        private static SalesTableTableAdapter salesAdapter = new SalesTableTableAdapter();
        internal static List<Person> People = new List<Person>();
        internal static List<SalesCar> SalesCars = new List<SalesCar>();
        internal static List<PaperInfo> Papers = new List<PaperInfo>();
        public const string CONNECTION = "Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\LopezData.mdf;Integrated Security = True; Connect Timeout = 30; MultipleActiveResultSets=True ";

        /// <summary>Initializes this instance.</summary>
        internal static void Init()
        {
            paymentCarAdapter.Fill(lopezData.PaymentCarTable);
            paymentAdapter.Fill(lopezData.PaymentTable);
            peopleAdapter.Fill(lopezData.PeopleTable);
            salesCarAdapter.Fill(lopezData.SalesCarTable);
            paperInfoAdapter.Fill(lopezData.PaperInfoTable);
            salesAdapter.Fill(lopezData.SalesTable);

            Thread thread = new Thread(new ThreadStart(() =>
            {
                SalesCars.Populate();
                Papers.Populate();
            }));
            thread.Start();

            People.Populate();
            thread.Join();
        }

        #region POPULATE

        /// <summary>Populates the specified cars.</summary>
        /// <param name="cars">The cars.</param>
        private static void Populate(this List<SalesCar> cars)
        {
            foreach (DataRow car in lopezData.SalesCarTable)
            {
                int year = (int)car["Year"];
                string make = (string)car["Make"];
                string model = (string)car["Model"];
                string vin = (string)car["VIN"];
                int? mileage = car["Mileage"] is DBNull ? null : (int?)car["Mileage"];
                decimal price = (decimal)car["Price"];
                decimal lowestPrice = (decimal)car["LowestPrice"];
                DateTime date = (DateTime)car["ListDate"];
                string color = (string)car["Color"];
                decimal boughtPrice = (decimal)car["BoughtPrice"];
                bool salvage = (bool)car["Salvage"];
                bool extraKey = (bool)car["ExtraKey"];
                bool byOwner = (bool)car["ByOwner"];

                cars.Add(new SalesCar(year, make, model, vin, mileage, price, lowestPrice, date, color, boughtPrice, salvage, extraKey, byOwner));
            }
        }

        /// <summary>Populates the specified people.</summary>
        /// <param name="people">The people.</param>
        private static void Populate(this List<Person> people)
        {
            foreach (DataRow person in lopezData.PeopleTable)
            {
                string name = (string)person["Name"];
                string phone = (string)person["Phone"];
                string address = (string)person["Address"];
                int personId = (int)person["PersonId"];

                List<PaymentCar> cars = new List<PaymentCar>();

                foreach (DataRow car in lopezData.PaymentCarTable.Select("PersonId=" + personId))
                {
                    int year = (int)car["Year"];
                    string make = (string)car["Make"];
                    string model = (string)car["Model"];
                    string vin = (string)car["VIN"];
                    decimal due = (decimal)car["Due"];
                    decimal expected = (decimal)car["Expected"];
                    int? mileage = car["Mileage"] is DBNull ? null : (int?)car["Mileage"];
                    string color = (string)car["Color"];
                    DateTime boughtdate = (DateTime)car["BoughtDate"];
                    int carID = (int)car["CarId"];
                    List<Payment> payments = new List<Payment>();

                    foreach (DataRow payment in lopezData.PaymentTable.Select("CarId=" + carID))
                    {
                        DateTime date = (DateTime)payment["Date"];
                        decimal amount = (decimal)payment["Amount"];
                        bool down = (bool)payment["Down"];
                        int paymentId = (int)payment["PaymentId"];

                        payments.Add(new Payment(paymentId, carID, date, amount, down));
                    }

                    cars.Add(new PaymentCar(name, year, make, model, mileage, vin, due, expected, color, boughtdate, payments, personId, carID));
                }
                people.Add(new Person(personId, name, phone, address, cars));
            }
        }

        /// <summary>Populates the specified papers.</summary>
        /// <param name="papers">The papers.</param>
        private static void Populate(this List<PaperInfo> papers)
        {
            foreach (DataRow row in lopezData.PaperInfoTable)
            {
                Car trade = null;
                if (!(row["TradeVIN"] is DBNull))
                {
                    int? trademileage = row["TradeMileage"] is DBNull ? null : (int?)row["TradeMileage"];
                    trade = new Car((int)row["TradeYear"], (string)row["TradeMake"], (string)row["TradeModel"], (string)row["TradeColor"], (string)row["TradeVIN"], (decimal)row["TradeValue"], trademileage);
                }
                int? mileage = row["Mileage"] is DBNull ? null : (int?)row["Mileage"];
                Person person = new Person(0, (string)row["Buyer"], (string)row["Phone"], (string)row["Address"], null);
                Car car = new Car((int)row["Year"], (string)row["Make"], (string)row["Model"], (string)row["Color"], (string)row["VIN"], (decimal)row["SellingPrice"], mileage);
                papers.Add(new PaperInfo((DateTime)row["Date"], person, (string)row["CoBuyer"], car, trade, (decimal)row["Down"], (bool)row["Tag"], (bool)row["Lien"], (bool)row["OutOfState"], (decimal)row["Tax"], (int)row["Warranty"], (decimal)row["AveragePayment"]));
            }
        }

        #endregion POPULATE

        #region PAPERINFO

        /// <summary>Adds the paper information.</summary>
        /// <param name="paperInfo">The paper information.</param>
        internal static void AddPaperInfo(PaperInfo paperInfo)
        {
            if (paperInfo.Trade == null)
            {
                paperInfoAdapter.Insert(paperInfo.Date, paperInfo.Buyer.Name, paperInfo.CoBuyer, paperInfo.Buyer.Full_Address, paperInfo.Buyer.Phone, paperInfo.Car.VIN, paperInfo.Car.Year,
                    paperInfo.Car.Make, paperInfo.Car.Model, paperInfo.Car.Color, paperInfo.Car.Mileage, null, null, null, null, null, null, null, paperInfo.Car.Value, paperInfo.Down,
                    paperInfo.Tag, paperInfo.Lien, paperInfo.OutOfState, paperInfo.Tax, paperInfo.Warranty, paperInfo.AveragePayment);
            }
            else
            {
                paperInfoAdapter.Insert(paperInfo.Date, paperInfo.Buyer.Name, paperInfo.CoBuyer, paperInfo.Buyer.Full_Address, paperInfo.Buyer.Phone, paperInfo.Car.VIN, paperInfo.Car.Year,
                    paperInfo.Car.Make, paperInfo.Car.Model, paperInfo.Car.Color, paperInfo.Car.Mileage, paperInfo.Trade.VIN, paperInfo.Trade.Year,
                    paperInfo.Trade.Make, paperInfo.Trade.Model, paperInfo.Trade.Color, paperInfo.Trade.Mileage, paperInfo.Trade.Value, paperInfo.Car.Value, paperInfo.Down,
                    paperInfo.Tag, paperInfo.Lien, paperInfo.OutOfState, paperInfo.Tax, paperInfo.Warranty, paperInfo.AveragePayment);
            }
        }

        #endregion PAPERINFO

        #region PERSON

        /// <summary>Adds the person.</summary>
        /// <param name="name">The name.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="full_Address">The full address.</param>
        /// <returns>
        ///   <para>The table ID</para>
        /// </returns>
        internal static int AddPerson(string name, string phone, string full_Address)
        {
            return (int)peopleAdapter.InsertQuery(name, full_Address, phone);
        }

        /// <summary>Updates the person.</summary>
        /// <param name="person">The person.</param>
        /// <param name="newPerson">The new person.</param>
        internal static void UpdatePerson(Person person, Person newPerson)
        {
            People[People.IndexOf(person)] = newPerson;
            peopleAdapter.Update(newPerson.Name, newPerson.Full_Address, newPerson.Phone, person.PersonID, person.Name, person.Full_Address, person.Phone);
        }

        /// <summary>Removes the person.</summary>
        /// <param name="person">The person.</param>
        internal static void RemovePerson(Person person)
        {
            People.Remove(person);
            peopleAdapter.Delete(person.PersonID, person.Name, person.Full_Address, person.Phone);
        }

        #endregion PERSON

        #region SALESCARS

        /// <summary>Edits the sales car.</summary>
        /// <param name="car">The car.</param>
        /// <param name="newCar">The new car.</param>
        internal static void EditSalesCar(SalesCar car, SalesCar newCar)
        {
            salesCarAdapter.Update(newCar.VIN, newCar.Year, newCar.Make, newCar.Model, newCar.Color, newCar.Mileage, newCar.Price,
                newCar.LowestPrice, newCar.ListDate, newCar.BoughtPrice, newCar.ExtraKey, newCar.Salvage, newCar.ByOwner,
                car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Price,
                car.LowestPrice, car.ListDate, car.BoughtPrice, car.ExtraKey, car.Salvage, car.ByOwner);
            SalesCars[SalesCars.IndexOf(car)] = newCar;
        }

        /// <summary>Removes the sales car.</summary>
        /// <param name="car">The car.</param>
        internal static void RemoveSalesCar(SalesCar car)
        {
            Logger.LogInventory(false, car);
            salesCarAdapter.Delete(car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Price, car.LowestPrice, car.ListDate, car.BoughtPrice, car.ExtraKey, car.Salvage, car.ByOwner);
            SalesCars.Remove(car);
        }

        /// <summary>Adds the sales car.</summary>
        /// <param name="car">The car.</param>
        internal static void AddSalesCar(SalesCar car)
        {
            Logger.LogInventory(true, car);
            salesCarAdapter.Insert(car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Price, car.LowestPrice, car.ListDate, car.BoughtPrice, car.ExtraKey, car.Salvage, car.ByOwner);
            SalesCars.Add(car);
        }

        #endregion SALESCARS

        #region PAYMENTS

        /// <summary>Removes the payment.</summary>
        /// <param name="payment">The payment.</param>
        /// <param name="reason">The reason.</param>
        internal static void RemovePayment(string person, string car, Payment payment, string reason)
        {
            Logger.LogRemovePayment(person, car, payment, reason);
            paymentAdapter.Delete(payment.PaymentID, payment.CarID, payment.Date, payment.Amount, payment.IsDownPayment);
            removedPaymentAdapter.Insert(payment.CarID, payment.Date, payment.Amount, payment.IsDownPayment, reason);
        }

        /// <summary>Edits the payment.</summary>
        /// <param name="payment">The payment.</param>
        /// <param name="date">The date.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="reason">The reason.</param>
        internal static void EditPayment(string person, string car, Payment payment, DateTime date, decimal amount, string reason)
        {
            Logger.LogEditPayment(person, car, payment, date, amount, reason);
            paymentAdapter.Update(payment.CarID, date, amount, payment.IsDownPayment, payment.PaymentID, payment.CarID, payment.Date, payment.Amount, payment.IsDownPayment);
        }

        /// <summary>Adds the payment.</summary>
        /// <param name="carID">The car identifier.</param>
        /// <param name="person">The person.</param>
        /// <param name="car">The car.</param>
        /// <param name="date">The date.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="down">if set to <c>true</c> [down].</param>
        /// <returns>
        ///   <para>The table ID</para>
        /// </returns>
        internal static int AddPayment(int carID, string person, string car, DateTime date, decimal amount, bool down)
        {
            Logger.LogAddPayment(person, car, amount, down);
            return (int)paymentAdapter.InsertQuery(carID, date, amount, down);
        }

        #endregion PAYMENTS

        #region PAYMENTCAR

        /// <summary>Adds the payment car.</summary>
        /// <param name="personID">The person identifier.</param>
        /// <param name="person">The person.</param>
        /// <param name="year">The year.</param>
        /// <param name="make">The make.</param>
        /// <param name="model">The model.</param>
        /// <param name="mileage">The mileage.</param>
        /// <param name="VIN">The vin.</param>
        /// <param name="due">The due.</param>
        /// <param name="average">The average payment.</param>
        /// <param name="now">The current datetime.</param>
        /// <param name="color">The color.</param>
        /// <returns>
        ///   <para>The table ID</para>
        /// </returns>
        internal static int AddPaymentCar(int personID, int year, string make, string model, int? mileage, string VIN, decimal due, decimal average, DateTime now, string color)
        {
            return (int)paymentCarAdapter.InsertQuery(personID, year, make, model, VIN, due, color, average, now, mileage);
        }

        /// <summary>Removes the payment car.</summary>
        /// <param name="person">The person.</param>
        /// <param name="car">The car.</param>
        internal static void RemovePaymentCar(Person person, PaymentCar car)
        {
            People[People.IndexOf(person)].Cars.Remove(car);
            foreach (Payment p in car.Payments)
            {
                paymentAdapter.Delete(p.PaymentID, car.CarID, p.Date, p.Amount, p.IsDownPayment);
            }
            paymentCarAdapter.Delete(car.CarID, car.PersonID, car.Year, car.Make, car.Model, car.VIN, car.Due, car.Color, car.Expected, car.BoughtDate, car.Mileage);
        }

        #endregion PAYMENTCAR

        #region Sales

        /// <summary>
        /// Adds the sale to storage.
        /// </summary>
        /// <param name="person">The name.</param>
        /// <param name="car">The car.</param>
        /// <param name="boughtPrice">The bought price.</param>
        /// <param name="listDate">The list date.</param>
        /// <param name="boughtDate">The bought date.</param>
        /// <param name="salvage">if set to <c>true</c> [salvage].</param>
        internal static void AddSales(string person, Car car, decimal boughtPrice, DateTime listDate, DateTime boughtDate, bool salvage)
        {
            Logger.LogSale(person, car);
            salesAdapter.Insert(person, car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Value, boughtPrice, listDate, boughtDate, salvage);
        }

        #endregion Sales
    }
}