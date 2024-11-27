using Cosmetify.Model;
using Cosmetify.Model.Enums;
using Cosmetify.ViewModel;
using ControlzEx.Standard;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
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
    /// Interaction logic for ActivesView.xaml
    /// </summary>
    public partial class ActivesView : System.Windows.Controls.Page
    {
        Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook workBook;
        Microsoft.Office.Interop.Excel.Worksheet workSheet;
        Microsoft.Office.Interop.Excel.Range cellRange;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Using a DependencyProperty as the backing store for ActivesModelsCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivesModelsCollectionProperty =
            DependencyProperty.Register("ActivesModelsCollection", typeof(ObservableCollection<ActivesModel>), typeof(ActivesView), new PropertyMetadata(HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts()));

        public ActivesView()
        {
            InitializeComponent();
            this.cbUnits.ItemsSource = System.Enum.GetValues(typeof(ProductUnits));
            this.cbStatus.ItemsSource = System.Enum.GetValues(typeof(BatchStatus));
            this.ActivesModelsCollection = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
            this.cbCateg.ItemsSource = HomepageViewModel.CommonViewModel.CategoryRepository.GetCategories();
        }

        public CategoryModel Category { get; set; }

        public SubCategoryModel SubCategory { get; set; }

        public SubSubCategoryModel SubSubCategory { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "csv Files (*.csv)|*.csv";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                var dataTable = Helper.Helper.ConvertCsvToDataTable(dlg.FileName);
                HomepageViewModel.CommonViewModel.ActivesRepository.BulkInsertMySQL(dataTable, "actives");
            }

            this.ActivesModelsCollection = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
        }

        private void btnAddActive_Click(object sender, RoutedEventArgs e)
        {
            var actives = new ActivesModel();
            if (string.IsNullOrEmpty(this.tbName.Text))
            {
                MessageBox.Show("Actives Name cannot be empty");
            }
            else
            {
                actives.ActivesName = this.tbName.Text;
            }

            if (string.IsNullOrEmpty(this.tbCode.Text))
            {
                MessageBox.Show("Actives Code cannot be empty");
            }
            else
            {
                actives.ShortCode = this.tbCode.Text;
            }

            if (string.IsNullOrEmpty(this.tbStock.Text))
            {
                MessageBox.Show("Actives Stock cannot be empty");
            }
            else
            {
                actives.Stocks = Convert.ToDouble(this.tbStock.Text);
            }

            if (this.cbUnits.SelectedIndex < 0)
            {
                MessageBox.Show("Please select the unit.");
            }
            else
            {
                actives.Units = (ProductUnits)System.Enum.Parse(typeof(ProductUnits), this.cbUnits.SelectedValue.ToString());
            }

            if (string.IsNullOrEmpty(this.tbSku.Text))
            {
                MessageBox.Show("Actives SKU cannot be empty");
            }
            else
            {
                actives.SKU = Convert.ToDouble(this.tbSku.Text);
            }

            if (this.Category != null)
            {
                actives.Category = this.Category;
            }

            if (this.SubCategory != null)
            {
                actives.SubCategory = this.SubCategory;
            }

            if (this.SubSubCategory != null)
            {
                actives.SubSubCategory = this.SubSubCategory;
            }

            HomepageViewModel.CommonViewModel.ActivesRepository.InsertProduct(actives);
        }

        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                ActivesModel product = e.Row.DataContext as ActivesModel;
                if (product != null)
                {
                    if (product.Id > 0)
                    {
                        HomepageViewModel.CommonViewModel.ActivesRepository.UpdateProduct(product);
                    }
                    else
                    {
                        HomepageViewModel.CommonViewModel.ActivesRepository.InsertProduct(product);
                    }
                }
            }
        }

        private void dataGrid1_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg != null)
            {
                ActivesModel product = dg.SelectedItem as ActivesModel;
                if (e.Command == DataGrid.DeleteCommand && product != null)
                {
                    HomepageViewModel.CommonViewModel.ActivesRepository.DeleteProduct(product.Id);
                }
            }
        }

        private void btnFormat_Click(object sender, RoutedEventArgs e)
        {
            File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"TxtFile\Sample_Test.csv", System.IO.Path.GetTempPath() + "\\Sample.csv", true);
            Process excel = new Process();
            excel.StartInfo.UseShellExecute = true;
            excel.StartInfo.FileName = System.IO.Path.GetTempPath() + "\\Sample.csv";
            excel.Start();

            // Need to wait for excel to start
            excel.WaitForInputIdle();

            IntPtr p = excel.MainWindowHandle;
            ShowWindow(p, 1);
        }

        private void Tabs1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                       
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1 != null)
            {
                this.ActivesModelsCollection = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
            }

            var batchOrders = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetAllProducts();
            if (batchOrders != null)
            {
                foreach (var batchOrder in batchOrders)
                {
                    if (batchOrder.Status == BatchStatus.Processed)
                    {
                        foreach (var model in batchOrder.BatchOrderCollection)
                        {
                            if (model.Actives != null)
                            {
                                var actives = this.ActivesModelsCollection.SingleOrDefault<ActivesModel>(r => r.Id == model.Actives.Id);
                                if (actives != null)
                                {
                                    actives.TotalRequired += model.StocksRequired;
                                    actives.TotalBatchOrders += 1;
                                    actives.BrandNames += batchOrder.ProductName + Environment.NewLine;
                                }
                            }
                        }
                    }

                    if (batchOrder.Status == BatchStatus.Created)
                    {
                        foreach (var model in batchOrder.BatchOrderCollection)
                        {
                            if (model.Actives != null)
                            {
                                var actives = this.ActivesModelsCollection.SingleOrDefault<ActivesModel>(r => r.Id == model.Actives.Id);
                                if (actives != null)
                                {
                                    actives.TotalCreatedRequired += model.StocksRequired;
                                    actives.TotalCreated += 1;
                                    actives.BrandNames += batchOrder.ProductName + Environment.NewLine;
                                }
                            }
                        }
                    }

                    if (batchOrder.Status == BatchStatus.Hold)
                    {
                        foreach (var model in batchOrder.BatchOrderCollection)
                        {
                            if (model.Actives != null)
                            {
                                var actives = this.ActivesModelsCollection.SingleOrDefault<ActivesModel>(r => r.Id == model.Actives.Id);
                                if (actives != null)
                                {
                                    actives.TotalHoldRequired += model.StocksRequired;
                                    actives.TotalHold += 1;
                                    actives.BrandNames += batchOrder.ProductName + Environment.NewLine;
                                }
                            }
                        }
                    }
                }
            }

        }

        private void ExportActiveReport(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
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

        private void DeleteActives(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button != null)
            {
                var model = button.DataContext as ActivesModel;
                if (model != null)
                {
                    HomepageViewModel.CommonViewModel.ActivesRepository.DeleteProduct(model.Id);
                    this.ActivesModelsCollection = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
                }
            }
        }

        public ObservableCollection<ActivesModel> ActivesModelsCollection
        {
            get { return (ObservableCollection<ActivesModel>)GetValue(ActivesModelsCollectionProperty); }
            set { SetValue(ActivesModelsCollectionProperty, value); }
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var searchData = this.tbSearch.Text;
                if (!string.IsNullOrEmpty(searchData))
                {
                    var data = HomepageViewModel.CommonViewModel.ActivesRepository.SearchActives(searchData);
                    this.ActivesModelsCollection = data;
                }
                else
                {
                    this.ActivesModelsCollection = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
                }
            }
        }

        private void cbAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.dataGrid1.Items)
            {
                var row = this.dataGrid1.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    row.IsSelected = true;
                }
            }
        }

        private void cbAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.dataGrid1.Items)
            {
                var row = this.dataGrid1.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    row.IsSelected = false;
                }
            }
        }

        private void btnBulk_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.SelectedItems != null && this.dataGrid1.SelectedItems.Count > 0)
            {
                foreach (ActivesModel model in this.dataGrid1.SelectedItems)
                {
                    if (model != null)
                    {
                        HomepageViewModel.CommonViewModel.ActivesRepository.DeleteProduct(model.Id);
                    }
                }

                this.ActivesModelsCollection = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
            }
            else
            {
                MessageBox.Show("Please select multiple rows to perform this action.", "Bulk Delete", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            string fromDate = null;
            string toDate = null;
            if (!string.IsNullOrEmpty(this.dtFrom.Text))
            {
                fromDate = this.dtFrom.Text;
            }

            if (!string.IsNullOrEmpty(this.dtTo.Text))
            {
                toDate = this.dtTo.Text;
            }

            if (this.dataGrid1 != null)
            {
                this.ActivesModelsCollection = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
            }

            var batchOrders = HomepageViewModel.CommonViewModel.BatchOrderRepository.BatchFilters(fromDate, toDate);
            if (batchOrders != null)
            {
                foreach (var batchOrder in batchOrders)
                {
                    if (batchOrder.Status == BatchStatus.Processed)
                    {
                        foreach (var model in batchOrder.BatchOrderCollection)
                        {
                            var actives = this.ActivesModelsCollection.SingleOrDefault<ActivesModel>(r => r.Id == model.Actives.Id);
                            if (actives != null)
                            {
                                actives.TotalRequired += model.StocksRequired;
                                actives.TotalBatchOrders += 1;
                                actives.BrandNames += batchOrder.ProductName + Environment.NewLine;
                            }
                        }
                    }

                    if (batchOrder.Status == BatchStatus.Created)
                    {
                        foreach (var model in batchOrder.BatchOrderCollection)
                        {
                            var actives = this.ActivesModelsCollection.SingleOrDefault<ActivesModel>(r => r.Id == model.Actives.Id);
                            if (actives != null)
                            {
                                actives.TotalCreatedRequired += model.StocksRequired;
                                actives.TotalCreated += 1;
                                actives.BrandNames += batchOrder.ProductName + Environment.NewLine;
                            }
                        }
                    }

                    if (batchOrder.Status == BatchStatus.Hold)
                    {
                        foreach (var model in batchOrder.BatchOrderCollection)
                        {
                            var actives = this.ActivesModelsCollection.SingleOrDefault<ActivesModel>(r => r.Id == model.Actives.Id);
                            if (actives != null)
                            {
                                actives.TotalHoldRequired += model.StocksRequired;
                                actives.TotalHold += 1;
                                actives.BrandNames += batchOrder.ProductName + Environment.NewLine;
                            }
                        }
                    }
                }
            }
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
                workSheet.Name = "LearningExcel";
                System.Data.DataTable tempDt = DtIN;
                //dgExcel.ItemsSource = tempDt.DefaultView;
                workSheet.Cells.Font.Size = 11;
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

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            GenerateExcel(Helper.Helper.ToDataTable(this.ActivesModelsCollection.ToList()));
            var dialog = new SaveFileDialog();
            dialog.FileName = Math.Abs(DateTime.Now.GetHashCode()).ToString();
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

        private void cbCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null && cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem as CategoryModel; 
                if (item != null) 
                {
                    this.cbSCateg.ItemsSource = item.SubCategories;
                    this.Category = item;
                }
            }
        }

        private void cbSCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null && cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem as SubCategoryModel;
                if (item != null)
                {
                    var category = HomepageViewModel.CommonViewModel.SubCategoryRepository.GetSubCategory(item.Id);
                    this.cbSSCateg.ItemsSource = category.SubSubCategories;
                    this.SubCategory = category;
                }
            }
        }

        private void cbSSCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null && cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem as SubSubCategoryModel;
                if (item != null)
                {
                    var category = HomepageViewModel.CommonViewModel.SubSubCategoryRepository.GetSubSubCategory(item.Id);
                    this.SubSubCategory = category;
                }
            }
        }
    }
}
