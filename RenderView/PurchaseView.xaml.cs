using Cosmetify.Dialogs;
using Cosmetify.Helper;
using Cosmetify.Model;
using Cosmetify.ViewModel;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    /// Interaction logic for PurchaseView.xaml
    /// </summary>
    public partial class PurchaseView : Page
    {
        public PurchaseView()
        {
            InitializeComponent();
            this.DataContext = HomepageViewModel.CommonViewModel;
        }

        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                ProductModel product = e.Row.DataContext as ProductModel;
                if (product != null)
                {
                    if (product.Id > 0)
                    {
                        HomepageViewModel.CommonViewModel.ProductRepository.UpdateProduct(product);
                    }
                    else
                    {
                        HomepageViewModel.CommonViewModel.ProductRepository.InsertProduct(product);
                    }
                }
            }
        }

        private void dataGrid1_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg !=null)
            {
                ProductModel product = dg.SelectedItem as ProductModel;
                if (e.Command == DataGrid.DeleteCommand && product != null)
                {
                    HomepageViewModel.CommonViewModel.ProductRepository.DeleteProduct(product.Id);
                }
            }            
        }

        

        //private void ImportBulk(object sender, RoutedEventArgs e)
        //{
        //    var dialog = new ImportDialog();
        //    dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        //    dialog.ResizeMode = ResizeMode.NoResize;
        //    bool? result = dialog.ShowDialog();
        //    if (result == true)
        //    {
        //        var fp = dialog.FilePath;
        //        using (var reader = new StreamReader(fp.FullName))
        //        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //        {
        //            var records = csv.GetRecords<dynamic>();
        //            this.dataDb.ItemsSource = records;
        //        }
        //    }
        //}
    }
}
