using Cosmetify.Model;
using Cosmetify.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for BatchEditView.xaml
    /// </summary>
    public partial class BatchEditView : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text        

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

            this.dtBatch.SelectedDateTime = this.BatchModel.BatchDate;
            this.dtPlan.SelectedDate = this.BatchModel.PlanningDate;
            this.dtMfg.SelectedDate = this.BatchModel.MfgDate;
            this.dtExp.SelectedDate = this.BatchModel.Expiry;
            this.cbProd.ItemsSource = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();
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
            this.BatchModel.BatchDate = (DateTime)this.dtBatch.SelectedDateTime;
            this.BatchModel.PlanningDate = (DateTime)this.dtPlan.SelectedDate;
            this.BatchModel.PlannedDate = DateTime.Now;
            this.BatchModel.MfgDate = (DateTime)this.dtMfg.SelectedDate;
            this.BatchModel.Expiry = (DateTime)this.dtExp.SelectedDate;
            this.DialogResult = true;
            this.Close();
        }

        private void BatchSizeUpdate(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && !string.IsNullOrEmpty(tb.Text))
            {
                var bsize = long.Parse(tb.Text);
                if (bsize >= 0) {
                    if (this.BatchModel != null && this.BatchModel.BatchOrderCollection != null && this.BatchModel.BatchOrderCollection.Count > 0)
                    {
                        foreach (var item in this.BatchModel.BatchOrderCollection)
                        {
                            item.BatchSize = bsize;
                        }
                    }
                }
            }
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void cbProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbProd.SelectedItem != null && this.cbProd.SelectedItem is ActivesModel)
            {
                var batchOrder = new BatchOrderModel();
                batchOrder.Actives = this.cbProd.SelectedItem as ActivesModel;
                this.BatchModel.BatchOrderCollection.Add(batchOrder);
            }
        }
    }
}
