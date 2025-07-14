using Cosmetify.Dialogs;
using Cosmetify.Model;
using Cosmetify.Model.Enums;
using Cosmetify.ViewModel;
using Microsoft.Win32;
using MigraDoc.Rendering;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Cosmetify.RenderView
{
    /// <summary>
    /// Interaction logic for OrderInvoice.xaml
    /// </summary>
    public partial class OrderInvoice : Page
    {
        public OrderInvoice()
        {
            this.Loaded += OrderInvoice_Loaded;
            InitializeComponent();            
        }

        private async void OrderInvoice_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbCust.ItemsSource = await HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbBrand.ItemsSource = await HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbProduct.ItemsSource = await HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
            this.cbColor.ItemsSource = HomepageViewModel.CommonViewModel.ColoursRepository.GetColors();
            this.cbPerfume.ItemsSource = HomepageViewModel.CommonViewModel.PerfumeRepository.GetAllPerfumes();
            this.cbProd.ItemsSource = await HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
            this.cbCust.IsEnabled = true;
            this.DBBatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllDistinctOrders();
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

        public ObservableCollection<CustomOrderModel> DBBatchModelCollection
        {
            get { return (ObservableCollection<CustomOrderModel>)GetValue(DBBatchModelCollectionProperty); }
            set { SetValue(DBBatchModelCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BatchModelCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DBBatchModelCollectionProperty =
            DependencyProperty.Register("DBBatchModelCollection", typeof(ObservableCollection<CustomOrderModel>), typeof(OrderInvoice), new PropertyMetadata(new ObservableCollection<CustomOrderModel>()));

        private async void btnAddCust_Click(object sender, RoutedEventArgs e)
        {
            var addCust = new AddCustomer();
            if ((bool)addCust.ShowDialog())
            {
                this.cbCust.ItemsSource = await HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
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

        private async void btnAddProd_Click(object sender, RoutedEventArgs e)
        {
            var addCust = new AddMasterFormula();
            addCust.ActivesList.Clear();
            if ((bool)addCust.ShowDialog())
            {
                if (!string.IsNullOrEmpty(addCust.FormulaName) || !string.IsNullOrEmpty(addCust.FormulaCode) || addCust.ActivesList.Count > 0)
                {
                    var model = new MasterFormulaModel();
                    model.Name = addCust.FormulaName;
                    model.Code = addCust.FormulaCode;
                    model.Requirements = new ObservableCollection<MasterProductModel>();
                    foreach (var item in addCust.ActivesList)
                    {
                        model.Requirements.Add(item);
                    }

                    model.RemainingWater = addCust.RemainingWater;
                    HomepageViewModel.CommonViewModel.MasterFormulaRepository.InsertFormula(model); 
                    this.cbProd.ItemsSource = await HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
                    MessageBox.Show("List Refreshed, Please select added formula", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }                
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

        private async void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = this.cbProduct.SelectedItem as MasterFormulaModel;
            if (t != null)
            {
                this.BatchOrderCollection.Clear();
                foreach (var actives in t.Requirements)
                {
                    var batchOrder = new BatchOrderModel();
                    batchOrder.Actives = await HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(actives.Id);
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
                batchOrder.Units = batchOrder.Actives.Units;
                this.BatchOrderCollection.Add(batchOrder);
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var batchModel = new BatchModel();
            if (this.chCust != null && this.chCust.IsChecked == true)
            {
                if (this.tbPname != null && string.IsNullOrEmpty(this.tbPname.Text))
                {
                    MessageBox.Show("Enter Product Name", "Custom Product", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return;
                }
                else
                {
                    batchModel.ProductName = this.tbPname.Text;
                    batchModel.ProductID = "PRD" + Math.Abs(DateTime.Now.GetHashCode());
                }
            }
            else
            {
                var mfmodel = this.cbProduct.SelectedItem as MasterFormulaModel;
                if (mfmodel != null)
                {
                    batchModel.ProductID = mfmodel.Code;
                    if (string.IsNullOrEmpty(mfmodel.Name))
                    {
                        batchModel.ProductName = mfmodel.Code;
                    }
                    else
                    {
                        batchModel.ProductName = mfmodel.Name;
                    }

                }
            }

            batchModel.BatchOrderNo = "COS-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
            var cust = this.cbCust.SelectedItem as CustomerModel;
            if (cust != null) {
                batchModel.Customer = cust;
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

            var claims = this.tbClaims.Text.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (claims.Length > 0)
            {
                batchModel.Claims = new ObservableCollection<string>(claims);
            }

            if (!string.IsNullOrEmpty(this.tbMProd.Text))
            {
                batchModel.AdditionalInfo = this.tbMProd.Text;
            }

            batchModel.PkgType = this.tbPkgType.Text;
            batchModel.PackagingTypeImage = this.imgPkg.Source as BitmapImage;
            batchModel.PkgOrderQuantity = this.tbPkgQty.Text;

            this.BatchModelCollection.Add(batchModel);

            this.cbCust.IsEnabled = false;
            this.cbBrand.SelectedIndex = -1;
            this.cbProduct.SelectedIndex = -1;
            this.tbMProd.Text = string.Empty;
            this.tbPkgQty.Text = string.Empty;
            this.cbColor.SelectedIndex = -1;
            this.cbPerfume.SelectedIndex = -1;
            this.tbPfumeVal.Text = string.Empty;
            this.tbClaims.Text = string.Empty;
            this.tbPkgType.Text = string.Empty;
            this.imgPkg.ClearValue(Image.SourceProperty);
            this.cbProd.SelectedIndex = -1;
            this.BatchOrderCollection.Clear();
        }

        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderID = "OD-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
            foreach (var batchModel in this.BatchModelCollection)
            {
                batchModel.OrderId = orderID;
                batchModel.Status = BatchStatus.Created;
                batchModel.BatchDate = DateTime.Now;

                HomepageViewModel.CommonViewModel.BatchOrderRepository.InsertProduct(batchModel);
            }

            this.BatchModelCollection.Clear();
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

        private async void DeleteBatch1(object sender, RoutedEventArgs e)
        {
            //var button = sender as Button;
            //if (button != null)
            //{
            //    var model = button.DataContext as BatchModel;
            //    if (model != null)
            //    {
            //        HomepageViewModel.CommonViewModel.BatchOrderRepository.DeleteProduct(model.Id);
            //        //this.DBBatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
            //    }
            //
            MessageBox.Show("This button is Disabled because in this order there are multiple products, after delete complete sale order will deleted including batch orders which is not recoverable. If you want to enable it then please contact your development team, it will be enabled in next build.", "Functionality Disabled", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.DBBatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllDistinctOrders();
        }

        

        private async void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var searchData = this.tbSearch.Text;
                if (!string.IsNullOrEmpty(searchData))
                {
                    var data = await HomepageViewModel.CommonViewModel.BatchOrderRepository.SearchDistinctBatch(searchData);
                    this.DBBatchModelCollection = data;
                }
                else
                {
                    this.DBBatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllDistinctOrders();
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

        private async void ReorderBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    model.BatchOrderNo = "COS-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
                    model.OrderId = "OD-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
                    model.Status = BatchStatus.Created;
                    model.BatchDate = DateTime.Now;
                    model.PlannedDate = DateTime.MinValue;
                    model.PlanningDate = DateTime.MinValue;
                    model.MfgDate = DateTime.MinValue;
                    model.Expiry = DateTime.MinValue;
                    model.CompletionDate = DateTime.MinValue;
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.InsertProduct(model);
                    //this.DBBatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private async void ExportBatch(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button != null)
                {
                    var model = button.DataContext as CustomOrderModel;
                    if (model != null)
                    {
                        var batchCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProductsWithOrderId(model.OrderId);
                        var batch = new PdfCore.PdfForm();
                        var document = batch.CreateOrder(batchCollection[0], batchCollection);
                        document.UseCmykColor = true;
                        var pdfRenderer = new PdfDocumentRenderer(true);

                        // Set the MigraDoc document.
                        pdfRenderer.Document = document;

                        // Create the PDF document.
                        pdfRenderer.RenderDocument();

                        // Save the PDF document...
                        var filename = "SaleOrder-" + model.CustomerName.FirstName + "_" + model.BrandName + ".pdf";

                        var dialog = new SaveFileDialog();
                        dialog.FileName = filename;
                        dialog.AddExtension = true;
                        dialog.DefaultExt = ".pdf";
                        if ((bool)dialog.ShowDialog())
                        {
                            pdfRenderer.Save(dialog.FileName);
                            // ...and start a viewer.
                            //Process.Start(dialog.FileName);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Either the file is open or access by another process or app. " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Helper.Helper.LogError(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Either the file is open or access by another process or app. " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Helper.Helper.LogError(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Helper.Helper.LogError(ex);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectCategory();
            
            if ((bool)dialog.ShowDialog())
            {
                this.cbProd.ItemsSource = dialog.ItemsCollection;
            }
        }

        private async void OpenOrder(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as CustomOrderModel;
                if (model != null && model.OrderId != null)
                {
                    var orderView = new OrderViewPage();
                    orderView.DbModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProductsWithOrderId(model.OrderId);
                    orderView.ShowDialog();
                }
            }
        }

        private void EditOrder(object sender, RoutedEventArgs e)
        {

        }

        private async void btnBulkDload_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid2.SelectedItems.Count > 1)
            {
                var result = MessageBox.Show("Do you want to download Pdf of All Orders?", "Bulk Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (CustomOrderModel model in this.dataGrid2.SelectedItems)
                    {
                        if (model != null)
                        {
                            var batchCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProductsWithOrderId(model.OrderId);
                            var batch = new PdfCore.PdfForm();
                            var document = batch.CreateOrder(batchCollection[0], batchCollection);
                            document.UseCmykColor = true;
                            var pdfRenderer = new PdfDocumentRenderer(true);

                            // Set the MigraDoc document.
                            pdfRenderer.Document = document;

                            // Create the PDF document.
                            pdfRenderer.RenderDocument();

                            // Save the PDF document...
                            var filename = "SaleOrder-" + model.CustomerName.FirstName + "_" + model.BrandName + ".pdf";
                            var dektopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);                            
                            pdfRenderer.Save(dektopPath + "//" + filename);                        }
                    }

                    
                    MessageBox.Show("Selected Items successfully downloaded on your Desktop.", "Bulk Action", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnBulkDel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
