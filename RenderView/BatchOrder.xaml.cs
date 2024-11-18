using BahiKitaab.Dialogs;
using BahiKitaab.Model;
using BahiKitaab.Model.Enums;
using BahiKitaab.ViewModel;
using Microsoft.Win32;
using MigraDoc.Rendering;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static MaterialDesignThemes.Wpf.Theme;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;
using Button = System.Windows.Controls.Button;
using ComboBox = System.Windows.Controls.ComboBox;
using TabControl = System.Windows.Controls.TabControl;

namespace BahiKitaab.RenderView
{
    /// <summary>
    /// Interaction logic for BatchOrder.xaml
    /// </summary>
    public partial class BatchOrder : Page
    {
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
            this.cbUnits.ItemsSource = Enum.GetValues(typeof(ProductUnits));
            this.cbCust.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbProd.ItemsSource = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
            this.cbMF.ItemsSource = HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
            this.dataGrid1.ItemsSource = this.BatchModel.BatchOrderCollection;
            this.stkAct.Visibility = Visibility.Collapsed;
        }

        private void BatchOrder_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.BatchModelCollection != null && this.BatchModelCollection.Count > 0)
            {
                this.BatchModelCollection.Clear();
            }

            this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
        }

        private void btnAddCust_Click(object sender, RoutedEventArgs e)
        {
            var addCust = new AddCustomer();
            if ((bool)addCust.ShowDialog())
            {
                this.cbCust.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
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
                this.BatchModel.PlannedDate = DateTime.Parse(this.dpPlan.Text);
            }
            else
            {
                this.BatchModel.PlannedDate = DateTime.MinValue;
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

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
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
                    this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void ExportBatch(object sender, RoutedEventArgs e)
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
                    var filename = model.Customer.FirstName + "_" + model.Customer.LastName + "_" + model.ProductName + ".pdf";

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

        private void DeleteBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.DeleteProduct(model.Id);
                    this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void ProceedBatch(object sender, RoutedEventArgs e)
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
                            HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                        }
                    }

                    this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void HoldBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    model.Status = BatchStatus.Hold;
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                    if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                    {
                        foreach (var item in model.BatchOrderCollection)
                        {
                            HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                        }
                    }

                    this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void CompleteBatch(object sender, RoutedEventArgs e)
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
                    if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                    {
                        foreach (var item in model.BatchOrderCollection)
                        {
                            HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                        }
                    }

                    this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void ReorderBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    model.BatchOrderNo = "COS-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
                    model.Status = BatchStatus.Created;
                    model.BatchDate = DateTime.Now;
                    model.PlannedDate = DateTime.MinValue;
                    model.PlanningDate = DateTime.MinValue;
                    model.MfgDate = DateTime.MinValue;
                    model.Expiry = DateTime.MinValue;
                    model.CompletionDate = DateTime.MinValue;
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.InsertProduct(model);
                    this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void cbMF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbMF.SelectedItem != null && this.cbMF.SelectedItem is MasterFormulaModel)
            {
                if (string.IsNullOrEmpty(this.tbSize.Text))
                {
                    MessageBox.Show("Please enter batch size", "Alert", MessageBoxButton.OK, MessageBoxImage.Error); return;
                }

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
                        batchOrder.Units = (ProductUnits)Enum.Parse(typeof(ProductUnits), this.cbUnits.SelectedItem.ToString());
                        batchOrder.Actives = HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(actives.Id);
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
                batchOrder.Units = (ProductUnits)Enum.Parse(typeof(ProductUnits), this.cbUnits.SelectedItem.ToString());
                batchOrder.Actives = this.cbProd.SelectedItem as ActivesModel;
                this.BatchModel.BatchOrderCollection.Add(batchOrder);
            }
        }

        private void Tabs1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (tabControl.SelectedIndex == 1)
                {
                    this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
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
                    this.BatchModelCollection = data;
                }
                else
                {
                    this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
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

        private void cbBulk_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                                            model.PlanningDate = DateTime.Now;
                                            HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                                            if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                                            {
                                                foreach (var item in model.BatchOrderCollection)
                                                {
                                                    HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                                                }
                                            }
                                        }
                                    }

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
                                            model.Status = BatchStatus.Hold;
                                            // model.PlanningDate = DateTime.Now;
                                            HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                                            if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                                            {
                                                foreach (var item in model.BatchOrderCollection)
                                                {
                                                    HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                                                }
                                            }
                                        }
                                    }

                                    MessageBox.Show("Selected Items successfully hold.", "Bulk Action", MessageBoxButton.OK, MessageBoxImage.Information);
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
                                            model.Status = BatchStatus.Completed;
                                            model.CompletionDate = DateTime.Now;
                                            HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(model);
                                            if (model.BatchOrderCollection != null && model.BatchOrderCollection.Count > 0)
                                            {
                                                foreach (var item in model.BatchOrderCollection)
                                                {
                                                    HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(item.Actives);
                                                }
                                            }
                                        }
                                    }

                                    MessageBox.Show("Selected Items successfully completed.", "Bulk Action", MessageBoxButton.OK, MessageBoxImage.Information);
                                }                                    
                            }

                            break;

                        case 3:
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

                    this.BatchModelCollection = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
                }
            }
        }

        private void BatchView_Click(object sender, RoutedEventArgs e)
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
                        HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(dialog.BatchModel);
                    }
                }
            }            
        }
    }
}
