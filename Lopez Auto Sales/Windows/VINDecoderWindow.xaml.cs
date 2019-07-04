using System;
using System.Windows;
using System.Windows.Controls;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for VINDecoderWindow.xaml
    /// </summary>
    public partial class VINDecoderWindow : Window
    {
        public string VIN { get; private set; }

        public VINDecoderWindow(string vin)
        {
            InitializeComponent();
            VIN = vin;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            JSONClass jsonClass = await VINDecoder.DecodeVINAsync(VIN);

            try
            {
                foreach (JSONResult result in jsonClass.Results)
                {
                    if (String.IsNullOrWhiteSpace(result.Value))
                        continue;
                    TreeView treeView = new TreeView();
                    TreeViewItem header = new TreeViewItem() { Header = result.Variable };
                    TreeViewItem item = new TreeViewItem() { Header = result.Value };
                    header.Items.Add(item);
                    treeView.Items.Add(header);
                    NodeList.Items.Add(treeView);
                }
            }
            catch
            {
                MessageBox.Show("VIN could not be decoded.");
                Close();
            }
        }
    }
}