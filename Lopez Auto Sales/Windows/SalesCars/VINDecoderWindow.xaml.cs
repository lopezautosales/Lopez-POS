using Lopez_Auto_Sales.JSON;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for VINDecoderWindow.xaml
    /// </summary>
    public partial class VINDecoderWindow : Window
    {
        public SalesCar Car { get; private set; }

        public VINDecoderWindow(SalesCar salesCar)
        {
            InitializeComponent();
            Car = salesCar;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            JSONCar jsonCar = WebManager.GetInfo(Car);

            foreach (JSONExtra info in jsonCar.Extras.OrderBy(extra => extra.Name))
            {
                TreeView treeView = new TreeView();
                TreeViewItem header = new TreeViewItem() { Header = info.Name };
                TreeViewItem item = new TreeViewItem() { Header = info.Value };
                header.Items.Add(item);
                treeView.Items.Add(header);
                NodeList.Items.Add(treeView);
            }
        }
    }
}