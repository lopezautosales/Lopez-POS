using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for EditPersonWindow.xaml
    /// </summary>
    public partial class EditPersonWindow : Window
    {
        Person person = null;
        public EditPersonWindow(Person p)
        {
            person = p;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StateBox.ItemsSource = Constants.States;

            NameBox.Text = person.Name;
            AddressBox.Text = person.Address;
            PhoneBox.Text = person.Phone;
            StateBox.Text = person.State;
            CityBox.Text = person.City;
            ZipBox.Text = person.ZIP;
        }

        private bool CheckPerson(out Person newPerson)
        {
            newPerson = null;
            if (String.IsNullOrEmpty(NameBox.Text) || String.IsNullOrEmpty(AddressBox.Text) || String.IsNullOrEmpty(CityBox.Text) || String.IsNullOrEmpty(StateBox.Text) || String.IsNullOrEmpty(ZipBox.Text))
            {
                MessageBox.Show("Some buyer info is blank.", "Info Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!String.IsNullOrEmpty(PhoneBox.Text) && PhoneBox.Text.Length != 13)
            {
                MessageBox.Show("Enter a 10 digit phone number or leave blank.", "Phone Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            newPerson = new Person(NameBox.Text.ToCapital(), PhoneBox.Text, AddressBox.Text.ToCapital(), CityBox.Text.ToCapital(), StateBox.Text.ToCapital(), ZipBox.Text, person.Cars, person.PersonID);
            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPerson(out Person newPerson))
            {
                Storage.UpdatePerson(person, newPerson);
                MessageBox.Show("Person Info Updated.");
                DialogResult = true;
                Close();
            }
        }

        private void PhoneBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneBox.Text.Length == 10 && double.TryParse(PhoneBox.Text, out double number))
                PhoneBox.Text = String.Format("{0:(###)###-####}", number);
        }

        private void PhoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
