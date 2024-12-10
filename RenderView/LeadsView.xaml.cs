using Cosmetify.Dialogs;
using Cosmetify.Helper;
using Cosmetify.Model;
using Cosmetify.Model.Enums;
using Cosmetify.RenderView.UserControls;
using Cosmetify.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for LeadsView.xaml
    /// </summary>
    public partial class LeadsView : Page
    {        
        public LeadsView()
        {
            InitializeComponent();
            this.DataContext = HomepageViewModel.CommonViewModel;
        }

        private void ImportFile(object sender, RoutedEventArgs e)
        {
            /*var dialog = new ImportDialog();
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.ResizeMode = ResizeMode.NoResize;
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var fp = dialog.FilePath;
                using (var reader = new StreamReader(fp.FullName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<dynamic>();
                    this.dataGrid1.ItemsSource = records;
                    this.dataGrid1.DataContext = records;
                }
            }*/
        }

        private void dataGrid1_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dg = sender as System.Windows.Controls.DataGrid;
            if (dg != null)
            {
                CustomerModel product = dg.SelectedItem as CustomerModel;
                if (e.Command == System.Windows.Controls.DataGrid.DeleteCommand && product != null)
                {
                    HomepageViewModel.CommonViewModel.LeadsRepository.DeleteLead(product.Id);
                    this.addLead.CustomerLeads = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
                }
            }
        }

        private void dataGrid2_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                CustomerModel product = e.Row.DataContext as CustomerModel;
                if (product != null)
                {
                    if (product.Id > 0)
                    {
                        HomepageViewModel.CommonViewModel.LeadsRepository.UpdateLead(product);
                    }
                    else
                    {
                        HomepageViewModel.CommonViewModel.LeadsRepository.InsertLead(product);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.addLead.CustomerLeads = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
        }

        private void DeleteBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as CustomerModel;
                if (model != null)
                {
                    HomepageViewModel.CommonViewModel.LeadsRepository.DeleteLead(model.Id);
                    this.addLead.CustomerLeads = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
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

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var searchData = this.tbSearch.Text;
                if (!string.IsNullOrEmpty(searchData))
                {
                    var data = HomepageViewModel.CommonViewModel.LeadsRepository.SearchLeads(searchData);
                    this.addLead.CustomerLeads = data;
                }
                else
                {
                    this.addLead.CustomerLeads = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
                }
            }
        }

        private void btnBulk_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.SelectedItems != null && this.dataGrid1.SelectedItems.Count > 0)
            {
                foreach (var model in this.dataGrid1.SelectedItems)
                {                    
                    if (model != null)
                    {
                        var cModel = model as CustomerModel;
                        if (cModel != null)
                        {
                            HomepageViewModel.CommonViewModel.LeadsRepository.DeleteLead(cModel.Id);
                        }                        
                    }
                }

                this.addLead.CustomerLeads = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            }
            else
            {
                MessageBox.Show("Please select multiple rows to perform this action.", "Bulk Delete", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
