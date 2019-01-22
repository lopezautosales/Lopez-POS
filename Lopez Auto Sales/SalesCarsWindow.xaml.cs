using Lopez_Auto_Sales.Cars;
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
            CarAdder carAdder = new CarAdder() { Owner = this, Topmost = true } ;
            if(carAdder.ShowDialog() == true)
            {
                Storage.AddSalesCar(carAdder.Car);

                CarGrid.ItemsSource = null;
                CarGrid.ItemsSource = Storage.SalesCars;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarGrid.ItemsSource = null;
            CarGrid.ItemsSource = Storage.SalesCars;
        }

        private void SellCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarGrid.SelectedItem == null)
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

            InfoWindow infoWindow = new InfoWindow((CarGrid.SelectedItem as SalesCar).VIN);
            infoWindow.Show();
        }

        private void EditCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarGrid.SelectedItem == null)
            {
                MessageBox.Show("Select a car to edit");
                return;
            }

            SalesCar car = CarGrid.SelectedItem as SalesCar;
            EditSalesCar editCar = new EditSalesCar(car) { Owner = this, Topmost = true };
            if(editCar.ShowDialog() == true)
            {
                if(editCar.Car == null)
                {
                    Storage.RemoveSalesCar(car);
                }
                else
                {
                    Storage.EditSalesCar(car, editCar.Car);
                }

                CarGrid.ItemsSource = null;
                CarGrid.ItemsSource = Storage.SalesCars;
            }
        }
    }
}
