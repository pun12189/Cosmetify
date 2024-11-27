using Cosmetify.Dialogs;
using Cosmetify.Model;
using Cosmetify.Model.Enums;
using Cosmetify.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for OrderInvoice.xaml
    /// </summary>
    public partial class OrderInvoice : Page
    {
        public OrderInvoice()
        {
            InitializeComponent();
            this.cbCust.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbBrand.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbProduct.ItemsSource = HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
            this.cbColor.ItemsSource = HomepageViewModel.CommonViewModel.ColoursRepository.GetColors();
            this.cbPerfume.ItemsSource = HomepageViewModel.CommonViewModel.PerfumeRepository.GetAllPerfumes();
            this.cbProd.ItemsSource = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
            this.cbCust.IsEnabled = true;
            this.DBBatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
        }

        public ObservableCollection<BatchOrderModel> BatchOrderCollection
        {
            get { return (ObservableCollection<BatchOrderModel>)GetValue(BatchOrderCollectionProperty); }
            set { SetValue(BatchOrderCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BatchOrderCollectionProperty =
            DependencyProperty.Register("BatchOrderCollection", typeof(ObservableCollection<BatchOrderModel>), typeof(OrderInvoice), new PropertyMetadata(new ObservableCollection<BatchOrderModel>()));


        public ObservableCollection<BatchModel> BatchModelCollection
        {
            get { return (ObservableCollection<BatchModel>)GetValue(BatchModelCollectionProperty); }
            set { SetValue(BatchModelCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BatchModelCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BatchModelCollectionProperty =
            DependencyProperty.Register("BatchModelCollection", typeof(ObservableCollection<BatchModel>), typeof(OrderInvoice), new PropertyMetadata(new ObservableCollection<BatchModel>()));

        public ObservableCollection<BatchModel> DBBatchModelCollection
        {
            get { return (ObservableCollection<BatchModel>)GetValue(DBBatchModelCollectionProperty); }
            set { SetValue(DBBatchModelCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BatchModelCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DBBatchModelCollectionProperty =
            DependencyProperty.Register("DBBatchModelCollection", typeof(ObservableCollection<BatchModel>), typeof(OrderInvoice), new PropertyMetadata(new ObservableCollection<BatchModel>()));

        private void btnAddCust_Click(object sender, RoutedEventArgs e)
        {
            var addCust = new AddCustomer();
            if ((bool)addCust.ShowDialog())
            {
                this.cbCust.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
                MessageBox.Show("List Refreshed, Please select added customer", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnAddBrand_Click(object sender, RoutedEventArgs e)
        {
            if (this.cbCust.SelectedItem != null && this.cbCust.SelectedItem is CustomerModel)
            {
                var cust = this.cbCust.SelectedItem as CustomerModel;
                if (cust != null) 
                {
                    var addCust = new AddCustomer(cust);
                    if ((bool)addCust.ShowDialog())
                    {
                        this.cbBrand.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetBrandsOfCustomer(cust.Id);
                        MessageBox.Show("List Refreshed, Please select added brand", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }                
            }
            else
            {
                MessageBox.Show("Please select the customer first.", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnAddProd_Click(object sender, RoutedEventArgs e)
        {
            var addCust = new AddMasterFormula();
            if ((bool)addCust.ShowDialog())
            {
                this.cbProd.ItemsSource = HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
                MessageBox.Show("List Refreshed, Please select added formula", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "png Files (*.png)|*.png|jpg Files (*.jpg)|*.jpeg";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                var img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.UriSource = new Uri(dlg.FileName);
                img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                img.DecodePixelWidth = 50;
                img.EndInit();
                this.imgPkg.Source = img;
            }
        }

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = this.cbProduct.SelectedItem as MasterFormulaModel;
            if (t != null)
            {
                this.BatchOrderCollection.Clear();
                foreach (var actives in t.Requirements)
                {
                    var batchOrder = new BatchOrderModel();
                    batchOrder.Actives = HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(actives.Id);
                    batchOrder.PercentageRequired = actives.Required;
                    this.BatchOrderCollection.Add(batchOrder);
                }                
            }
        }

        private void cbProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbProd.SelectedItem != null && this.cbProd.SelectedItem is ActivesModel)
            {
                var batchOrder = new BatchOrderModel();
                batchOrder.Actives = this.cbProd.SelectedItem as ActivesModel;
                this.BatchOrderCollection.Add(batchOrder);
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var batchModel = new BatchModel();
            batchModel.BatchOrderNo = "COS-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
            var cust = this.cbCust.SelectedItem as CustomerModel;
            if (cust != null) {
                batchModel.Customer = cust;
            }

            var mfmodel = this.cbProduct.SelectedItem as MasterFormulaModel;
            if (mfmodel != null) 
            {
                batchModel.ProductID = mfmodel.Code;
                batchModel.ProductName = mfmodel.Name;
            }
            
            var bname = this.cbBrand.SelectedItem as string;
            if (bname != null) 
            {
                batchModel.BrandName = bname;
            }

            foreach (var item in this.BatchOrderCollection)
            {
                batchModel.BatchOrderCollection.Add(item);
            }            

            var color = this.cbColor.SelectedItem as ColoursModel;
            if (color != null)
            {
                batchModel.Colour = color.Name;
            }

            var perfume = this.cbPerfume.SelectedItem as PerfumeModel;
            if (perfume != null)
            {
                batchModel.Perfume = perfume.Name + " " + this.tbPfumeVal.Text;
            }

            var claims = this.tbClaims.Text.Split(',');
            if (claims.Length > 0)
            {
                batchModel.Claims = new ObservableCollection<string>(claims);
            }

            batchModel.PkgType = this.tbPkgType.Text;
            batchModel.PackagingTypeImage = this.imgPkg.Source as BitmapImage;
            batchModel.PkgOrderQuantity = this.tbPkgQty.Text;

            this.BatchModelCollection.Add(batchModel);
        }

        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderID = "OD-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
            foreach (var batchModel in this.BatchModelCollection)
            {
                batchModel.OrderId = orderID;
                batchModel.Status = BatchStatus.Created;

                HomepageViewModel.CommonViewModel.BatchOrderRepository.InsertProduct(batchModel);
            }
        }

        private void cbCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbCust.SelectedItem != null && this.cbCust.SelectedItem is CustomerModel)
            {
                var cust = this.cbCust.SelectedItem as CustomerModel;
                if (cust != null)
                {
                     this.cbBrand.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetBrandsOfCustomer(cust.Id);
                }

                this.cbCust.IsEnabled = false;
            }
        }        

        private void DeleteBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    this.BatchModelCollection.Remove(model);
                }
            }
        }

        private void DeleteBatch1(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.DeleteProduct(model.Id);
                    this.DBBatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void btnAddColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddColorPerfume(true);
            if ((bool)dialog.ShowDialog())
            {
                this.cbColor.ItemsSource = HomepageViewModel.CommonViewModel.ColoursRepository.GetColors();
            }
        }

        private void btnAddPerfume_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddColorPerfume(false);
            if ((bool)dialog.ShowDialog())
            {
                this.cbPerfume.ItemsSource = HomepageViewModel.CommonViewModel.PerfumeRepository.GetAllPerfumes();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.DBBatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
        }

        private void dataGrid1_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dg = sender as System.Windows.Controls.DataGrid;
            if (dg != null)
            {
                BatchModel product = dg.SelectedItem as BatchModel;
                if (e.Command == System.Windows.Controls.DataGrid.DeleteCommand && product != null)
                {
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.DeleteProduct(product.Id);
                    this.DBBatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
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
                        if (product.Status == BatchStatus.Processed)
                        {
                            foreach (var item in product.BatchOrderCollection)
                            {
                                HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                            }
                        }

                        HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(product);
                    }
                    else
                    {
                        HomepageViewModel.CommonViewModel.BatchOrderRepository.InsertProduct(product);
                    }
                }
            }
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var searchData = this.tbSearch.Text;
                if (!string.IsNullOrEmpty(searchData))
                {
                    var data = HomepageViewModel.CommonViewModel.BatchOrderRepository.SearchBatch(searchData);
                    this.DBBatchModelCollection = data;
                }
                else
                {
                    this.DBBatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void cbAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.dataGrid2.Items)
            {
                var row = this.dataGrid2.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    row.IsSelected = true;
                }
            }
        }

        private void cbAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.dataGrid2.Items)
            {
                var row = this.dataGrid2.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    row.IsSelected = false;
                }
            }
        }
    }
}
