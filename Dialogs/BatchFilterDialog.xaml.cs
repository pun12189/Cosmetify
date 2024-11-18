using BahiKitaab.Model;
using BahiKitaab.Model.Enums;
using BahiKitaab.Repository;
using BahiKitaab.ViewModel;
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
using System.Windows.Shapes;

namespace BahiKitaab.Dialogs
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
            this.cbCust.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbStatus.ItemsSource = Enum.GetValues(typeof(BatchStatus));
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
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
                mfg = "between " + this.dpMfgFrom.Text + " AND " + this.dpMfgTo.Text;
            }
            else if (!string.IsNullOrEmpty(this.dpMfgFrom.Text) && string.IsNullOrEmpty(this.dpMfgTo.Text))
            {
                isSearch = true;
                mfg = " = " + this.dpMfgFrom.Text;
            }
            else if (string.IsNullOrEmpty(this.dpMfgFrom.Text) && !string.IsNullOrEmpty(this.dpMfgTo.Text))
            {
                isSearch = true;
                mfg = "between " + DateOnly.FromDateTime(DateTime.Now) + " AND " + this.dpMfgTo.Text;
            }

            if (!string.IsNullOrEmpty(this.dpExpFrom.Text) && !string.IsNullOrEmpty(this.dpExpTo.Text))
            {
                isSearch = true;
                exp = "between " + this.dpExpFrom.Text + " AND " + this.dpExpTo.Text;
            }
            else if (!string.IsNullOrEmpty(this.dpExpFrom.Text) && string.IsNullOrEmpty(this.dpExpTo.Text))
            {
                isSearch = true;
                exp = " = " + this.dpExpFrom.Text;
            }
            else if (string.IsNullOrEmpty(this.dpExpFrom.Text) && !string.IsNullOrEmpty(this.dpExpTo.Text))
            {
                isSearch = true;
                exp = "between " + DateOnly.FromDateTime(DateTime.Now) + " AND " + this.dpExpTo.Text;
            }

            if (!string.IsNullOrEmpty(this.dpCdFrom.Text) && !string.IsNullOrEmpty(this.dpCdTo.Text))
            {
                isSearch = true;
                cd = "between " + this.dpCdFrom.Text + " AND " + this.dpCdTo.Text;
            }
            else if (!string.IsNullOrEmpty(this.dpCdFrom.Text) && string.IsNullOrEmpty(this.dpCdTo.Text))
            {
                isSearch = true;
                cd = " = " + this.dpCdFrom.Text;
            }
            else if (string.IsNullOrEmpty(this.dpCdFrom.Text) && !string.IsNullOrEmpty(this.dpCdTo.Text))
            {
                isSearch = true;
                cd = "between " + DateOnly.FromDateTime(DateTime.Now) + " AND " + this.dpCdTo.Text;
            }

            if (isSearch)
            {
                BatchModels = BatchOrderRepository.BatchFilters(cstid, oid, ptype, bname, status, mfg, exp, cd);
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
