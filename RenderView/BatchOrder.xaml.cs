using Cosmetify.Dialogs;
using Cosmetify.Model;
using Cosmetify.Model.Enums;
using Cosmetify.ViewModel;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using MigraDoc.Rendering;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;
using Button = System.Windows.Controls.Button;
using ComboBox = System.Windows.Controls.ComboBox;
using TabControl = System.Windows.Controls.TabControl;

namespace Cosmetify.RenderView
{
    /// <summary>
    /// Interaction logic for BatchOrder.xaml
    /// </summary>
    public partial class BatchOrder : System.Windows.Controls.Page
    {
        Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook workBook;
        Microsoft.Office.Interop.Excel.Worksheet workSheet;
        Microsoft.Office.Interop.Excel.Range cellRange;

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text        

        // Using a DependencyProperty as the backing store for BatchModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BatchModelProperty =
            DependencyProperty.Register("BatchModel", typeof(BatchModel), typeof(BatchOrder), new PropertyMetadata(new BatchModel()));

        // Using a DependencyProperty as the backing store for BatchModelCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BatchModelCollectionProperty =
            DependencyProperty.Register("BatchModelCollection", typeof(ObservableCollection<BatchModel>), typeof(BatchOrder), new PropertyMetadata());

        public BatchOrder()
        {
            InitializeComponent();
            this.Loaded += BatchOrder_Loaded;
            this.cbUnits.ItemsSource = System.Enum.GetValues(typeof(ProductUnits));
            this.dataGrid1.ItemsSource = this.BatchModel.BatchOrderCollection;
            this.stkAct.Visibility = Visibility.Collapsed;
        }

        private async void BatchOrder_Loaded(object sender, RoutedEventArgs e)
        {

            this.cbCust.ItemsSource = await HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbProd.ItemsSource = await HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
            this.cbMF.ItemsSource = await HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();

            if (this.BatchModelCollection != null && this.BatchModelCollection.Count > 0)
            {
                this.BatchModelCollection.Clear();
            }

            this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
        }

        private async void btnAddCust_Click(object sender, RoutedEventArgs e)
        {
            var addCust = new AddCustomer();
            if ((bool)addCust.ShowDialog())
            {
                this.cbCust.ItemsSource = await HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
                MessageBox.Show("List Refreshed, Please select added customer", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public BatchModel BatchModel
        {
            get { return (BatchModel)GetValue(BatchModelProperty); }
            set { SetValue(BatchModelProperty, value); }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbProdName.Text))
            {
                MessageBox.Show("Please enter brand name.", "Alert", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }

            this.BatchModel.BatchDate = DateTime.Now;
            this.BatchModel.ProductName = this.tbProdName.Text;
            this.BatchModel.BatchOrderNo = "COS-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
            this.BatchModel.Customer = this.cbCust.SelectedItem as CustomerModel;
            if (!string.IsNullOrEmpty(this.dpExpiry.Text))
            {
                this.BatchModel.Expiry = DateTime.Parse(this.dpExpiry.Text);
            }
            else
            {
                this.BatchModel.Expiry = DateTime.MinValue;
            }

            if (!string.IsNullOrEmpty(this.dpMfg.Text))
            {
                this.BatchModel.MfgDate = DateTime.Parse(this.dpMfg.Text);
            }
            else
            {
                this.BatchModel.MfgDate = DateTime.MinValue;
            }

            if (!string.IsNullOrEmpty(this.dpPlan.Text))
            {
                this.BatchModel.PlannedDate = DateTime.Now;
                this.BatchModel.PlanningDate = DateTime.Parse(this.dpPlan.Text);
            }
            else
            {
                this.BatchModel.PlannedDate = DateTime.MinValue;
                this.BatchModel.PlanningDate = DateTime.MinValue;
            }
            
            this.BatchModel.PkgType = this.tbPkg.Text;
            this.BatchModel.PkgOrderQuantity = this.tbPkgQuantity.Text;
            this.BatchModel.Remarks = this.tbRemarks.Text;
            this.BatchModel.Description = this.tbDecrp.Text;
            HomepageViewModel.CommonViewModel.BatchOrderRepository.InsertProduct(this.BatchModel);
        }

        public ObservableCollection<BatchModel> BatchModelCollection
        {
            get { return (ObservableCollection<BatchModel>)GetValue(BatchModelCollectionProperty); }
            set { SetValue(BatchModelCollectionProperty, value); }
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
        }

        private async void dataGrid1_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dg = sender as System.Windows.Controls.DataGrid;
            if (dg != null)
            {
                BatchModel product = dg.SelectedItem as BatchModel;
                if (e.Command == System.Windows.Controls.DataGrid.DeleteCommand && product != null)
                {
                    if (product.Status == BatchStatus.Processed || product.Status == BatchStatus.Completed)
                    {
                        var result1 = MessageBox.Show("Do you want to add inventory of this batch?", "Inventory Update", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result1 == MessageBoxResult.Yes)
                        {
                            var actives = product.BatchOrderCollection;
                            if (actives != null)
                            {
                                foreach (var active in actives)
                                {
                                    if (active.Actives != null)
                                    {
                                        var act = await HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(active.Actives.Id);
                                        act.Stocks = act.Stocks + active.StocksRequired;
                                        HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(act);
                                    }                                    
                                }

                                await Helper.Helper.UpdateBatchOrders();
                            }
                        }
                    }

                    HomepageViewModel.CommonViewModel.BatchOrderRepository.DeleteProduct(product.Id);
                    this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();                    
                }                
            }
        }

        private void ExportBatch(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button != null)
                {
                    var model = button.DataContext as BatchModel;
                    if (model != null)
                    {
                        var batch = new PdfCore.PdfForm();
                        var document = batch.CreateDocument(model);
                        document.UseCmykColor = true;
                        var pdfRenderer = new PdfDocumentRenderer(true);

                        // Set the MigraDoc document.
                        pdfRenderer.Document = document;

                        // Create the PDF document.
                        pdfRenderer.RenderDocument();

                        // Save the PDF document...
                        var filename = "Batch-" + model.Customer.FirstName + "_" + model.BrandName + ".pdf";

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

        private async void DeleteBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var result = MessageBox.Show("Do you want to delete the batch?", "Delete Batch", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) 
                {
                    var model = button.DataContext as BatchModel;
                    if (model != null)
                    {
                        if (model.Status == BatchStatus.Processed || model.Status == BatchStatus.Completed)
                        {
                            var result1 = MessageBox.Show("Do you want to add inventory of this batch?", "Inventory Update", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result1 == MessageBoxResult.Yes)
                            {
                                var actives = model.BatchOrderCollection;
                                if (actives != null) 
                                {
                                    foreach (var active in actives)
                                    {
                                        if (active.Actives != null)
                                        {
                                            var act = await HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(active.Actives.Id);
                                            act.Stocks = act.Stocks + active.StocksRequired;
                                            HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(act);
                                        }                                        
                                    }

                                    await Helper.Helper.UpdateBatchOrders();
                                }
                            }
                        }                       

                        HomepageViewModel.CommonViewModel.BatchOrderRepository.DeleteProduct(model.Id);
                        this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                    }
                }                
            }
        }

        private async void ProceedBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    model.Status = BatchStatus.Processed;
                    model.PlanningDate = DateTime.Now;
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                    if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                    {
                        foreach (var item in model.BatchOrderCollection)
                        {
                            if (item.Actives != null)
                            {
                                var active = await HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(item.Actives.Id);
                                active.Stocks = active.Stocks - item.StocksRequired;
                                HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(active);
                            }                            
                        }

                        await Helper.Helper.UpdateBatchOrders();
                    }

                    this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private async void HoldBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    model.Status = BatchStatus.Hold;
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                    /*if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                    {
                        foreach (var item in model.BatchOrderCollection)
                        {
                            HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                        }
                    }*/

                    this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private async void CompleteBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    model.Status = BatchStatus.Completed;
                    model.CompletionDate = DateTime.Now;
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                    /*if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                    {
                        foreach (var item in model.BatchOrderCollection)
                        {
                            HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                        }
                    }*/

                    this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
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
                    var newmodel = new BatchModel();
                    newmodel.BatchOrderNo = "COS-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
                    newmodel.OrderId = "OD-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
                    newmodel.Status = BatchStatus.Created;
                    newmodel.BatchDate = DateTime.Now;
                    newmodel.PlannedDate = DateTime.MinValue;
                    newmodel.PlanningDate = DateTime.MinValue;
                    newmodel.MfgDate = DateTime.MinValue;
                    newmodel.Expiry = DateTime.MinValue;
                    newmodel.CompletionDate = DateTime.MinValue;
                    newmodel.AdditionalInfo = model.AdditionalInfo;
                    newmodel.Description = model.Description;
                    newmodel.ProductName = model.ProductName;
                    newmodel.Customer = model.Customer;
                    newmodel.Claims = model.Claims;
                    newmodel.Colour = model.Colour;
                    foreach (var bo in model.BatchOrderCollection)
                    {
                        newmodel.BatchOrderCollection.Add(bo);
                    }
                    
                    newmodel.Perfume = model.Perfume;
                    newmodel.RemainingWater = model.RemainingWater;
                    newmodel.BrandName = model.BrandName;
                    newmodel.PkgOrderQuantity = model.PkgOrderQuantity;
                    newmodel.PackagingTypeImage = model.PackagingTypeImage;
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.InsertProduct(newmodel);
                    this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private async void cbMF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbMF.SelectedItem != null && this.cbMF.SelectedItem is MasterFormulaModel)
            {
                if (this.cbUnits.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select correct unit.", "Alert", MessageBoxButton.OK, MessageBoxImage.Error); return;
                }

                //if (string.IsNullOrEmpty(this.tbProdName.Text))
                //{
                //    MessageBox.Show("Please enter brand name.", "Alert", MessageBoxButton.OK, MessageBoxImage.Error); return;
                //}

                var t = this.cbMF.SelectedItem as MasterFormulaModel;
                if (t != null)
                {
                    this.BatchModel.BatchOrderCollection.Clear();
                    foreach (var actives in t.Requirements)
                    {
                        var batchOrder = new BatchOrderModel();
                        batchOrder.BatchSize = Convert.ToInt64(this.tbSize.Text);
                        batchOrder.Units = (ProductUnits)System.Enum.Parse(typeof(ProductUnits), this.cbUnits.SelectedItem.ToString());
                        batchOrder.Actives = await HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(actives.Id);
                        batchOrder.PercentageRequired = actives.Required;
                        this.BatchModel.BatchOrderCollection.Add(batchOrder);
                    }

                    this.BatchModel.BatchOrderCollection.Add(
                        new BatchOrderModel
                        {
                            BatchSize = Convert.ToInt64(this.tbSize.Text),
                            Units = ProductUnits.ltrs,
                            Actives = new ActivesModel
                            {
                                ActivesName = "Water",
                                ShortCode = "H2O"
                            },
                            PercentageRequired = t.RemainingWater
                        });

                    this.stkAct.Visibility = Visibility.Visible;
                }
            }
        }

        private void cbProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbProd.SelectedItem != null && this.cbProd.SelectedItem is ActivesModel)
            {
                var batchOrder = new BatchOrderModel();
                batchOrder.BatchSize = Convert.ToInt64(this.tbSize.Text);
                batchOrder.Units = (ProductUnits)System.Enum.Parse(typeof(ProductUnits), this.cbUnits.SelectedItem.ToString());
                batchOrder.Actives = this.cbProd.SelectedItem as ActivesModel;
                this.BatchModel.BatchOrderCollection.Add(batchOrder);
            }
        }

        private async void Tabs1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (tabControl.SelectedIndex == 1)
                {
                    this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void btnFilters_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BatchFilterDialog();
            if ((bool)dialog.ShowDialog())
            {
                this.BatchModelCollection = dialog.BatchModels;
            }            
        }

        private async void dataGrid2_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
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
                                if (item.Actives != null)
                                {
                                    var active = await HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(item.Actives.Id);
                                    active.Stocks = active.Stocks - item.StocksRequired;
                                    HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(active);
                                }                                
                            }

                            await Helper.Helper.UpdateBatchOrders();
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

        private async void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var searchData = this.tbSearch.Text;
                if (!string.IsNullOrEmpty(searchData))
                {
                    var data = await HomepageViewModel.CommonViewModel.BatchOrderRepository.SearchBatch(searchData);
                    this.BatchModelCollection = data;
                }
                else
                {
                    this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void cbAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach(var item in this.dataGrid2.Items)
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

        private async void cbBulk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                if (cb.SelectedIndex >= 0)
                {
                    switch (cb.SelectedIndex)
                    {
                        case 0:
                            if (this.dataGrid2.SelectedItems.Count > 1)
                            {
                                var result = MessageBox.Show("Do you want perform bulk action on selected items?", "Bulk Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    foreach (BatchModel model in this.dataGrid2.SelectedItems)
                                    {
                                        if (model != null)
                                        {
                                            model.Status = BatchStatus.Processed;
                                            // model.PlanningDate = DateTime.Now;
                                            HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                                            if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                                            {
                                                foreach (var item in model.BatchOrderCollection)
                                                {
                                                    if (item.Actives != null)
                                                    {
                                                        var active = await HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(item.Actives.Id);
                                                        active.Stocks = active.Stocks - item.StocksRequired;
                                                        HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(active);
                                                    }                                                    
                                                }
                                            }
                                        }
                                    }

                                    await Helper.Helper.UpdateBatchOrders();
                                    MessageBox.Show("Selected Items successfully processed.", "Bulk Action", MessageBoxButton.OK, MessageBoxImage.Information);
                                }                                
                            }

                            break;

                        case 1:
                            if (this.dataGrid2.SelectedItems.Count > 1)
                            {
                                var result = MessageBox.Show("Do you want perform bulk action on selected items?", "Bulk Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    foreach (BatchModel model in this.dataGrid2.SelectedItems)
                                    {
                                        if (model != null)
                                        {
                                            model.Status = BatchStatus.Planned;
                                            model.PlanningDate = DateTime.Now;
                                            HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                                            /*if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                                            {
                                                foreach (var item in model.BatchOrderCollection)
                                                {
                                                    HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                                                }
                                            }*/
                                        }
                                    }

                                    MessageBox.Show("Selected Items successfully planned.", "Bulk Action", MessageBoxButton.OK, MessageBoxImage.Information);
                                }                                    
                            }

                            break;

                        case 2:
                            if (this.dataGrid2.SelectedItems.Count > 1)
                            {
                                var result = MessageBox.Show("Do you want perform bulk action on selected items?", "Bulk Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    foreach (BatchModel model in this.dataGrid2.SelectedItems)
                                    {
                                        if (model != null)
                                        {
                                            model.Status = BatchStatus.Hold;
                                            HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                                            /*if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                                            {
                                                foreach (var item in model.BatchOrderCollection)
                                                {
                                                    HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                                                }
                                            }*/
                                        }
                                    }

                                    MessageBox.Show("Selected Items successfully hold.", "Bulk Action", MessageBoxButton.OK, MessageBoxImage.Information);
                                }                                    
                            }

                            break;
                        case 3:
                            if (this.dataGrid2.SelectedItems.Count > 1)
                            {
                                var result = MessageBox.Show("Do you want perform bulk action on selected items?", "Bulk Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    foreach (BatchModel model in this.dataGrid2.SelectedItems)
                                    {
                                        if (model != null)
                                        {
                                            model.Status = BatchStatus.Completed;
                                            model.CompletionDate = DateTime.Now;
                                            HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                                            /*if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                                            {
                                                foreach (var item in model.BatchOrderCollection)
                                                {
                                                    HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                                                }
                                            }*/
                                        }
                                    }

                                    MessageBox.Show("Selected Items successfully completed.", "Bulk Action", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }

                            break;

                        case 4:
                            if (this.dataGrid2.SelectedItems.Count > 1)
                            {
                                var result = MessageBox.Show("Do you want perform bulk action on selected items?", "Bulk Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    foreach (BatchModel item in this.dataGrid2.SelectedItems)
                                    {
                                        var model = item as BatchModel;
                                        if (model != null)
                                        {
                                            HomepageViewModel.CommonViewModel.BatchOrderRepository.DeleteProduct(model.Id);
                                        }
                                    }
                                }

                                MessageBox.Show("Selected Items successfully deleted.", "Bulk Action", MessageBoxButton.OK, MessageBoxImage.Information);
                            }

                            break;
                    }

                    this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private async void BatchView_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var dc = btn.DataContext as BatchModel;
                if (dc != null) 
                {
                    var dialog = new BatchEditView(dc);
                    if ((bool)dialog.ShowDialog())
                    {
                        if (dialog.BatchModel.Status == BatchStatus.Processed)
                        {
                            foreach (var item in dialog.BatchModel.BatchOrderCollection)
                            {
                                if (item.Actives != null)
                                {
                                    var active = await HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(item.Actives.Id);
                                    active.Stocks = active.Stocks - item.StocksRequired;
                                    HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(active);
                                }                                
                            }

                            await Helper.Helper.UpdateBatchOrders();
                        }

                        HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(dialog.BatchModel);
                    }
                }
            }

            this.BatchModelCollection = await HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
        }

        private void BatchSizeUpdate(object sender, TextChangedEventArgs e)
        {
            var tb = sender as System.Windows.Controls.TextBox;
            if (tb != null && !string.IsNullOrEmpty(tb.Text))
            {
                var bsize = long.Parse(tb.Text);
                if (bsize >= 0)
                {
                    if (this.BatchModel != null && this.BatchModel.BatchOrderCollection != null && this.BatchModel.BatchOrderCollection.Count > 0)
                    {
                        foreach (var item in this.BatchModel.BatchOrderCollection)
                        {
                            item.BatchSize = bsize;
                        }
                    }
                }
            }
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void GenerateExcel(System.Data.DataTable DtIN)
        {
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.DisplayAlerts = false;
                excel.Visible = false;
                workBook = excel.Workbooks.Add(Type.Missing);
                workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;
                workSheet.Name = "Filtered Data Sheet";
                System.Data.DataTable tempDt = DtIN;
                //dgExcel.ItemsSource = tempDt.DefaultView;
                workSheet.Cells.Font.Size = 11;
                workSheet.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                // workSheet.Columns.AutoFit();
                // workSheet.Rows.AutoFit();
                int rowcount = 1;
                for (int i = 1; i <= tempDt.Columns.Count; i++) //taking care of Headers.  
                {
                    workSheet.Cells[1, i] = tempDt.Columns[i - 1].ColumnName;
                }
                foreach (System.Data.DataRow row in tempDt.Rows) //taking care of each Row  
                {
                    rowcount += 1;
                    for (int i = 0; i < tempDt.Columns.Count; i++) //taking care of each column  
                    {
                        workSheet.Cells[rowcount, i + 1] = row[i].ToString();
                    }
                }
                cellRange = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[rowcount, tempDt.Columns.Count]];
                cellRange.EntireColumn.AutoFit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnCompletion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.BatchModelCollection != null)
                {
                    var tempCollection = new ObservableCollection<ActivesModel>();
                    foreach (var item in this.BatchModelCollection)
                    {
                        foreach (var act in item.BatchOrderCollection)
                        {
                            tempCollection.Add(act.Actives);
                        }
                    }

                    var activesCollection = tempCollection.DistinctBy(p => p.Id);

                    foreach (var batchOrder in this.BatchModelCollection)
                    {
                        //if (batchOrder.Status == BatchStatus.Planned)
                        //{
                        foreach (var model in batchOrder.BatchOrderCollection)
                        {
                            var actives = activesCollection.SingleOrDefault<ActivesModel>(r => r.Id == model.Actives.Id);
                            if (actives != null)
                            {
                                if (!string.IsNullOrEmpty(batchOrder.BrandName))
                                {
                                    actives.BrandNames += batchOrder.BrandName + "," + Environment.NewLine;
                                }

                                if (!string.IsNullOrEmpty(batchOrder.ProductName))
                                {
                                    actives.ProductNames += batchOrder.ProductName + "(" + batchOrder.AdditionalInfo + ")" + "," + Environment.NewLine;
                                }

                                actives.TotalRequired += model.StocksRequired;
                            }
                        }
                        //}
                    }

                    GenerateExcel(Helper.Helper.ToBatchDataTable(activesCollection.ToList()));
                    var dialog = new SaveFileDialog();
                    dialog.FileName = "BatchFilterReport-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
                    dialog.AddExtension = true;
                    dialog.DefaultExt = ".xlsx";
                    if ((bool)dialog.ShowDialog())
                    {
                        workBook.SaveAs(dialog.FileName);
                        // ...and start a viewer.
                        //Process.Start(dialog.FileName);
                    }

                    workBook.Close();
                    excel.Quit();
                }
            }
            catch (Exception ex)
            {
                Helper.Helper.BugReport(ex);
            }
        }
    }
}
