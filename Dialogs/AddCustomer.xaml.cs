using Cosmetify.Model;
using Cosmetify.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        public bool IsBrand { get; set; } = false;

        public CustomerModel CustomerModel { get; set; }

        public AddCustomer()
        {
            InitializeComponent();
            this.tbBNames.Visibility = Visibility.Collapsed;
            this.IsBrand = false;
        }

        public AddCustomer(CustomerModel cust)
        {
            InitializeComponent();
            this.tbName.Text = cust.FirstName;
            this.tbContact.Text = cust.PhoneNumber;
            this.CustomerModel = cust;
            this.tbBNames.Visibility = Visibility.Visible;
            this.IsBrand = true;
        }

        private void SaveCustomer(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbName.Text))
            {
                MessageBox.Show("Please enter name", "Alert", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (string.IsNullOrEmpty(this.tbContact.Text))
            {
                MessageBox.Show("Please enter contact", "Alert", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            } 

            if (this.IsBrand)
            {                
                if (!string.IsNullOrEmpty(this.tbBNames.Text))
                {
                    var str = this.tbBNames.Text.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    this.CustomerModel.BrandName ??= new System.Collections.ObjectModel.ObservableCollection<string>();
                    foreach (var b in str)
                    {
                        this.CustomerModel.BrandName.Add(b);
                    }
                }

                HomepageViewModel.CommonViewModel.LeadsRepository.UpdateLead(this.CustomerModel);
            }
            else
            {
                var cust = new CustomerModel();
                cust.FirstName = this.tbName.Text;
                cust.PhoneNumber = this.tbContact.Text;
                HomepageViewModel.CommonViewModel.LeadsRepository.InsertLead(cust);
            }
            
            this.DialogResult = true;
            this.Close();
        }
    }
}
