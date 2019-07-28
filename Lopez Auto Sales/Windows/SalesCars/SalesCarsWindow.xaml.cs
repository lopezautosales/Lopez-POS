using Lopez_Auto_Sales.Static;
using System.Linq;
using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for SalesCars.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class SalesCars : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SalesCars"/> class.
        /// </summary>
        public SalesCars()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the AddCarButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            CarAdder carAdder = new CarAdder() { Owner = this, Topmost = true };
            if (carAdder.ShowDialog() == true)
            {
                Storage.SalesCars.AddSalesCar(carAdder.Car);

                ReloadVehicles();
            }
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadVehicles();
        }

        /// <summary>
        /// Reloads the vehicles.
        /// </summary>
        private void ReloadVehicles()
        {
            CarGrid.ItemsSource = null;
            CarGrid.ItemsSource = Storage.SalesCarsList.OrderBy(c => c.Make);
        }

        /// <summary>
        /// Handles the Click event of the SellCarButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SellCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarGrid.SelectedItem == null || !(CarGrid.SelectedItem is SalesCar))
            {
                MessageBox.Show("Select a car to sell");
                return;
            }

            SalesCar car = CarGrid.SelectedItem as SalesCar;
            if (car.ByOwner)
            {
                MessageBox.Show("Cannot sell a vehicle for sale by owner.");
                return;
            }

            SellCarWindow carWindow = new SellCarWindow(car) { Owner = Owner };
            carWindow.Show();
            Close();
        }

        /// <summary>
        /// Handles the Click event of the InfoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarGrid.SelectedItem == null)
            {
                MessageBox.Show("Select a car");
                return;
            }

            VINDecoderWindow infoWindow = new VINDecoderWindow((CarGrid.SelectedItem as SalesCar));
            infoWindow.Show();
        }

        /// <summary>
        /// Handles the Click event of the EditCarButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void EditCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarGrid.SelectedItem == null || !(CarGrid.SelectedItem is SalesCar))
            {
                MessageBox.Show("Select a car to edit");
                return;
            }

            SalesCar car = CarGrid.SelectedItem as SalesCar;
            EditSalesCar editCar = new EditSalesCar(car) { Owner = this, Topmost = true };
            if (editCar.ShowDialog() == true)
            {
                if (editCar.Car == null)
                {
                    Storage.SalesCars.RemoveSalesCar(car);
                }
                else
                {
                    Storage.SalesCars.EditSalesCar(car, editCar.Car);
                }

                ReloadVehicles();
            }
        }

        /// <summary>
        /// Handles the Click event of the WarrantyButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void WarrantyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarGrid.SelectedItem == null || !(CarGrid.SelectedItem is SalesCar))
            {
                MessageBox.Show("Select a car");
                return;
            }

            MSEdit.PrintWarranty(CarGrid.SelectedItem as SalesCar, 20);
        }
    }
}