using Lopez_POS.Static;
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

namespace Lopez_POS.Windows.Information
{
    /// <summary>
    /// Interaction logic for SalesWindow.xaml
    /// </summary>
    public partial class SalesWindow : Window
    {
        public SalesWindow()
        {
            InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SalesGrid.ItemsSource = Storage.lopezData.SalesTable.Where(c => c.Name.ToLower().Contains(SearchBox.Text.ToLower()));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SalesGrid.ItemsSource = Storage.lopezData.SalesTable;
        }
    }
}
