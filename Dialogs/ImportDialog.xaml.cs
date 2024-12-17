using Cosmetify.Model;
using Cosmetify.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace Cosmetify.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportDialog.xaml
    /// </summary>
    public partial class ImportDialog : Window
    {
        // Using a DependencyProperty as the backing store for FilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(ImportDialog));

        private CategoryModel _categoryModel;

        private SubCategoryModel _subCategoryModel;

        private SubSubCategoryModel _subSubCategoryModel;

        public ImportDialog()
        {
            InitializeComponent();
            this.cbCateg.ItemsSource = HomepageViewModel.CommonViewModel.CategoryRepository.GetCategories();
        }        

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        private void UploadExcelFile(object sender, RoutedEventArgs e)
        {
            if (this._categoryModel == null)
            {
                MessageBox.Show("Please select category then sub-category & last sub-sub-category to upload the inventory", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (this._subCategoryModel == null)
            {
                MessageBox.Show("Please select sub-category then sub-sub-category to upload the inventory", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (this._subSubCategoryModel == null)
            {
                MessageBox.Show("Please select sub-sub-category to upload the inventory", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Excel files(*.csv, *.xls, *.xlsx) | *.csv; *.xls; *.xlsx | All files(*.*) | *.* ";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = System.IO.Path.GetFileName(dlg.FileName);
                this.fileNameTb.Text = filename;
                this.FilePath = dlg.FileName;
            }
        }

        private void SubmitFile(object sender, RoutedEventArgs e)
        {
            var dataTable = Helper.Helper.ConvertCsvToDataTable(this.FilePath, this._categoryModel.Id, this._subCategoryModel.Id, this._subSubCategoryModel.Id);
            HomepageViewModel.CommonViewModel.ActivesRepository.BulkInsertMySQL(dataTable, "actives");
            this.DialogResult = true;
            this.Close();            
        }

        private void cbCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as System.Windows.Controls.ComboBox;
            if (cb != null && cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem as CategoryModel;
                if (item != null)
                {
                    this.cbSCateg.ItemsSource = item.SubCategories;
                    this._categoryModel = item;
                }
            }
        }

        private void cbSCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as System.Windows.Controls.ComboBox;
            if (cb != null && cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem as SubCategoryModel;
                if (item != null)
                {
                    //var category = HomepageViewModel.CommonViewModel.SubCategoryRepository.GetSubCategory(item.Id);
                    this.cbSSCateg.ItemsSource = item.SubSubCategories;
                    this._subCategoryModel = item;
                }
            }
        }

        private void cbSSCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as System.Windows.Controls.ComboBox;
            if (cb != null && cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem as SubSubCategoryModel;
                if (item != null)
                {
                    this._subSubCategoryModel = item;
                }
            }
        }
    }
}
