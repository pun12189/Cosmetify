using Cosmetify.Model;
using Cosmetify.ViewModel;
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

namespace Cosmetify.Dialogs
{
    /// <summary>
    /// Interaction logic for OrderViewPage.xaml
    /// </summary>
    public partial class OrderViewPage : Window
    {
        public OrderViewPage()
        {
            InitializeComponent();
        }

        private async void dataGrid1_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dg = sender as System.Windows.Controls.DataGrid;
            if (dg != null)
            {
                BatchModel product = dg.SelectedItem as BatchModel;
                if (e.Command == System.Windows.Controls.DataGrid.DeleteCommand && product != null)
                {
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.DeleteProduct(product.Id);
                    // this.DBBatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void dataGrid2_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                BatchModel product = e.Row.DataContext as BatchModel;
                if (product != null)
                {
                    if (product.Id > 0)
                    {
                        /*if (product.Status == BatchStatus.Processed)
                        {
                            foreach (var item in product.BatchOrderCollection)
                            {
                                HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                            }
                        }*/

                        HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(product);
                    }
                    else
                    {
                        HomepageViewModel.CommonViewModel.BatchOrderRepository.InsertProduct(product);
                    }
                }
            }
        }
    }
}
