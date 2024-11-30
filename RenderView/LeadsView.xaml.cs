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
    }
}
