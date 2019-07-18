using System.Linq;
using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for SalesCars.xaml
    /// </summary>
    public partial class SalesCars : Window
    {
        public SalesCars()
        {
            InitializeComponent();
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            CarAdder carAdder = new CarAdder() { Owner = this, Topmost = true };
            if (carAdder.ShowDialog() == true)
            {
                Storage.AddSalesCar(carAdder.Car);

                ReloadVehicles();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadVehicles();
        }

        private void ReloadVehicles()
        {
            CarGrid.ItemsSource = null;
            CarGrid.ItemsSource = Storage.SalesCars.OrderBy(c => c.Make);
        }

        private void SellCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarGrid.SelectedItem == null || !(CarGrid.SelectedItem is SalesCar))
            {
                MessageBox.Show("Select a car to sell");
                return;
            }

            SellCarWindow carWindow = new SellCarWindow(CarGrid.SelectedItem as SalesCar) { Owner = Owner };
            carWindow.Show();
            Close();
        }

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
                    Storage.RemoveSalesCar(car);
                }
                else
                {
                    Storage.EditSalesCar(car, editCar.Car);
                }

                ReloadVehicles();
            }
        }

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