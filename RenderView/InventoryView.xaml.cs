using Cosmetify.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cosmetify.RenderView
{
    /// <summary>
    /// Interaction logic for InventoryView.xaml
    /// </summary>
    public partial class InventoryView : Page
    {
        public InventoryView()
        {
            InitializeComponent();
        }

        private void Bulk_Import(object sender, RoutedEventArgs e)
        {
            var dialog = new ImportDialog();
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog();
        }
    }
}
