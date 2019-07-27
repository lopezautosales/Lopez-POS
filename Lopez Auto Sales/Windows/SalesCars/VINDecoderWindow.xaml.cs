using Lopez_Auto_Sales.JSON;
using Lopez_Auto_Sales.Web;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for VINDecoderWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class VINDecoderWindow : Window
    {
        /// <summary>
        /// Gets the car.
        /// </summary>
        /// <value>
        /// The car.
        /// </value>
        public SalesCar Car { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VINDecoderWindow"/> class.
        /// </summary>
        /// <param name="salesCar">The sales car.</param>
        public VINDecoderWindow(SalesCar salesCar)
        {
            InitializeComponent();
            Car = salesCar;
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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