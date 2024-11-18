using BahiKitaab.Model;
using BahiKitaab.ViewModel;
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

namespace BahiKitaab.Dialogs
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        public AddCustomer()
        {
            InitializeComponent();
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

            var cust = new CustomerModel();
            cust.FirstName = this.tbName.Text;
            cust.PhoneNumber = this.tbContact.Text;
            HomepageViewModel.CommonViewModel.LeadsRepository.InsertLead(cust);
            this.DialogResult = true;
            this.Close();
        }
    }
}
