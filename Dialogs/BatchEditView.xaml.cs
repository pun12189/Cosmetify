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
    /// Interaction logic for BatchEditView.xaml
    /// </summary>
    public partial class BatchEditView : Window
    {
        public BatchEditView(BatchModel model)
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            var custList = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbCust.ItemsSource = custList;
            this.BatchModel = model;
            for (int i = 0; i <= custList.Count; i++)
            {
                if (custList[i].Id == this.BatchModel.Customer.Id)
                {
                    this.cbCust.SelectedIndex = i;
                    break;
                }
            }
        }

        public BatchModel BatchModel
        {
            get { return (BatchModel)GetValue(BatchModelProperty); }
            set { SetValue(BatchModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BatchModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BatchModelProperty =
            DependencyProperty.Register("BatchModel", typeof(BatchModel), typeof(BatchEditView), new PropertyMetadata(new BatchModel()));

        private void cbCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                var customer = cb.SelectedItem as CustomerModel;
                if (customer != null)
                {
                    if (this.BatchModel.Customer.Id != customer.Id)
                    {
                        this.BatchModel.Customer = customer;
                    }                    
                }
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
