using Lopez_Auto_Sales.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
             DisplayResults(NameText.Text, CarText.Text);
        }

        internal void DisplayResults(string nameQuery, string carQuery)
        {
            List<Person> people = new List<Person>();

            //Search in parallel
            Parallel.ForEach(Storage.People, (Person person) =>
            {
                if (!String.IsNullOrEmpty(nameQuery))
                    if (!person.Name.ToLower().Contains(nameQuery.ToLower()))
                        return;

                if (!String.IsNullOrEmpty(carQuery))
                    if (person.Cars.FirstOrDefault(car => car.ToString().ToLower().Contains(carQuery.ToLower())) == null)
                        return;

                lock (people)
                {
                    people.Add(person);
                }
            });

            AccountsBox.Items.Clear();
            foreach (Person person in people)
            {
                TreeView treeView = new TreeView();
                TreeViewItem header = new TreeViewItem() { Header = person };
                foreach (PaymentCar car in person.Cars)
                {
                    TreeViewItem item = new TreeViewItem() { Header = car };
                    item.MouseDoubleClick += Item_MouseDoubleClick;
                    header.Items.Add(item);
                }
                treeView.Items.Add(header);
                AccountsBox.Items.Add(treeView);
            }
        }

        private void Item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            Person person = (Person)((TreeViewItem)item.Parent).Header;
            PaymentCar paymentCar = (PaymentCar)item.Header;
            //Owner property must be set
            EntryWindow entryWindow = new EntryWindow(person, paymentCar) { Owner = this, Title = person.ToString() + ": " + paymentCar.ToString() };
            entryWindow.Show();
        }

        private void CarsButton_Click(object sender, RoutedEventArgs e)
        {
            SalesCars salesCars = new SalesCars();
            salesCars.Show();
        }

        private bool IsConnectedInternet()
        {
            try
            {
                PingReply reply = new Ping().Send("google.com");
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }
            return false;
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsConnectedInternet())
                MessageBox.Show(this, "You are not connected to the internet.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            Storage.Init();
            Search_TextChanged(null, null);
        }

        private void SellCarButton_Click(object sender, RoutedEventArgs e)
        {
            SellCarWindow carWindow = new SellCarWindow();
            carWindow.Show();
        }

        private void InformationButton_Click(object sender, RoutedEventArgs e)
        {
            InformationWindow infoWindow = new InformationWindow();
            infoWindow.Show();
        }

        private void EditPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(AccountsBox.SelectedItem is TreeView tree))
            {
                MessageBox.Show(this, "Select a person.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Person person = (tree.Items[0] as TreeViewItem).Header as Person;
            EditPersonWindow editWindow = new EditPersonWindow(person) { Topmost= true };
            if(editWindow.ShowDialog() == true)
            {
                Search_TextChanged(null, null);
            }
        }
    }

    public static class Extensions
    {
        public static string ToCapital(this string message)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(message.ToLower());
        }
    }
}
