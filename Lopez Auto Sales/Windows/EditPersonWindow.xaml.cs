using Lopez_Auto_Sales.Static;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for EditPersonWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class EditPersonWindow : Window
    {
        /// <summary>
        /// The person
        /// </summary>
        private Person person = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPersonWindow"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public EditPersonWindow(Person p)
        {
            person = p;
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Checks the person.
        /// </summary>
        /// <param name="newPerson">The new person.</param>
        /// <returns></returns>
        private bool CheckPerson(out Person newPerson)
        {
            newPerson = null;
            if (String.IsNullOrEmpty(NameBox.Text) || String.IsNullOrEmpty(AddressBox.Text) || String.IsNullOrEmpty(CityBox.Text) || String.IsNullOrEmpty(StateBox.Text) || String.IsNullOrEmpty(ZipBox.Text))
            {
                MessageBox.Show("Some buyer info is blank.");
                return false;
            }
            if (!String.IsNullOrEmpty(PhoneBox.Text) && PhoneBox.Text.Length != 12)
            {
                MessageBox.Show("Enter a 10 digit phone number or leave blank. ###-###-####");
                return false;
            }
            newPerson = new Person(person.PersonID, NameBox.Text.ToCapital(), PhoneBox.Text, AddressBox.Text.ToCapital(), CityBox.Text.ToCapital(), StateBox.Text.ToCapital(), ZipBox.Text, person.Cars);
            return true;
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the TextChanged event of the PhoneBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void PhoneBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneBox.Text.Length == 10 && double.TryParse(PhoneBox.Text, out double number))
                PhoneBox.Text = String.Format("{0:###-###-####}", number);
        }

        /// <summary>
        /// Handles the PreviewTextInput event of the PhoneBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        private void PhoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}