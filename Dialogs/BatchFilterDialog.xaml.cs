using Cosmetify.Model;
using Cosmetify.Model.Enums;
using Cosmetify.Repository;
using Cosmetify.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for BatchFilterDialog.xaml
    /// </summary>
    public partial class BatchFilterDialog : Window
    {
        private BatchOrderRepository BatchOrderRepository { get; set; } = new();

        public ObservableCollection<BatchModel> BatchModels { get; set; }

        public BatchFilterDialog()
        {
            InitializeComponent();
            this.LoadComponents();
            this.cbStatus.ItemsSource = Enum.GetValues(typeof(BatchStatus));
        }

        private async void LoadComponents()
        {
            this.cbCust.ItemsSource = await HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            int? cstid = null;
            string? oid = null;
            string? ptype = null;
            string? bname = null;
            string? status = null;
            string? mfg = null;
            string? exp = null;
            string? cd = null;
            bool isSearch = false;
            if (this.cbCust.SelectedIndex > 0)
            {
                var cust = this.cbCust.SelectedItem as CustomerModel;
                if (cust != null)
                {
                    isSearch = true;
                    cstid = cust.Id;
                }
            }

            if (this.cbStatus.SelectedIndex > 0)
            {
                isSearch = true;
                status = this.cbStatus.SelectedValue.ToString();
            }

            if (!string.IsNullOrEmpty(this.tbOdrId.Text))
            {
                isSearch = true;
                oid = this.tbOdrId.Text;
            }

            if (!string.IsNullOrEmpty(this.tbPkgType.Text))
            {
                isSearch = true;
                ptype = this.tbPkgType.Text;
            }

            if (!string.IsNullOrEmpty(this.tbBName.Text))
            {
                isSearch = true;
                bname = this.tbBName.Text;
            }

            if (!string.IsNullOrEmpty(this.dpMfgFrom.Text) && !string.IsNullOrEmpty(this.dpMfgTo.Text))
            {
                isSearch = true;
                var m = DateTime.ParseExact(this.dpMfgFrom.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var t = DateTime.ParseExact(this.dpMfgTo.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                mfg = "between " + "'" + m.ToString("yyyy-MM-dd") + "'" + " AND " + "'" + t.ToString("yyyy-MM-dd") + "'";
            }
            else if (!string.IsNullOrEmpty(this.dpMfgFrom.Text) && string.IsNullOrEmpty(this.dpMfgTo.Text))
            {
                isSearch = true;
                var m = DateTime.ParseExact(this.dpMfgFrom.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                mfg = "between " + "'" + m.ToString("yyyy-MM-dd") + "'" + " AND " + "'" + DateTime.Now.AddDays(30).Date.ToString("yyyy-MM-dd") + "'";
            }
            else if (string.IsNullOrEmpty(this.dpMfgFrom.Text) && !string.IsNullOrEmpty(this.dpMfgTo.Text))
            {
                isSearch = true;
                var t = DateTime.ParseExact(this.dpMfgTo.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                mfg = "between " + "'" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'" + " AND " + "'" + t.ToString("yyyy-MM-dd") + "'";
            }

            if (!string.IsNullOrEmpty(this.dpExpFrom.Text) && !string.IsNullOrEmpty(this.dpExpTo.Text))
            {
                isSearch = true;
                var x = DateTime.ParseExact(this.dpExpFrom.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var y = DateTime.ParseExact(this.dpExpTo.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                exp = "between " + "'" + x.ToString("yyyy-MM-dd") + "'" + " AND " + "'" + y.ToString("yyyy-MM-dd") + "'";
            }
            else if (!string.IsNullOrEmpty(this.dpExpFrom.Text) && string.IsNullOrEmpty(this.dpExpTo.Text))
            {
                isSearch = true;
                var x = DateTime.ParseExact(this.dpExpFrom.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                exp = "between " + "'" + x.ToString("yyyy-MM-dd") + "'" + " AND " + "'" + DateTime.Now.AddDays(30).Date.ToString("yyyy-MM-dd") + "'";
            }
            else if (string.IsNullOrEmpty(this.dpExpFrom.Text) && !string.IsNullOrEmpty(this.dpExpTo.Text))
            {
                isSearch = true;
                var y = DateTime.ParseExact(this.dpExpTo.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                exp = "between " + "'" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'" + " AND " + "'" + y.ToString("yyyy-MM-dd") + "'";
            }

            if (!string.IsNullOrEmpty(this.dpCdFrom.Text) && !string.IsNullOrEmpty(this.dpCdTo.Text))
            {
                isSearch = true;
                var c = DateTime.ParseExact(this.dpCdFrom.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var d = DateTime.ParseExact(this.dpCdTo.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                cd = "between " + "'" + c.ToString("yyyy-MM-dd") + "'" + " AND " + "'" + d.ToString("yyyy-MM-dd") + "'";
            }
            else if (!string.IsNullOrEmpty(this.dpCdFrom.Text) && string.IsNullOrEmpty(this.dpCdTo.Text))
            {
                isSearch = true;
                var c = DateTime.ParseExact(this.dpCdFrom.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                cd = "between " + "'" + c.ToString("yyyy-MM-dd") + "'" + " AND " + "'" + DateTime.Now.AddDays(30).Date.ToString("yyyy-MM-dd") + "'";
            }
            else if (string.IsNullOrEmpty(this.dpCdFrom.Text) && !string.IsNullOrEmpty(this.dpCdTo.Text))
            {
                isSearch = true;
                var d = DateTime.ParseExact(this.dpCdTo.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                cd = "between " + "'" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'" + " AND " + "'" + d.ToString("yyyy-MM-dd") + "'";
            }

            if (isSearch)
            {
                BatchModels = await BatchOrderRepository.BatchFilters(cstid, oid, ptype, bname, status, mfg, exp, cd);
            }
            else
            {
                MessageBox.Show("Please select atleast one filter for search, otherwise close this dialog.", "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }
            
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
