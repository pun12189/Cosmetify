using BahiKitaab.Model;
using BahiKitaab.ViewModel;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BahiKitaab.RenderView.UserControls
{
    /// <summary>
    /// Interaction logic for CreateOrder.xaml
    /// </summary>
    public partial class CreateOrder : UserControl
    {
        private readonly ObservableCollection<ProductModel> products = new ObservableCollection<ProductModel>();

        private readonly ObservableCollection<OrderedProduct> cartproducts = new ObservableCollection<OrderedProduct>();

        // Using a DependencyProperty as the backing store for NetTotal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NetTotalProperty =
            DependencyProperty.Register("NetTotal", typeof(double), typeof(CreateOrder), new PropertyMetadata(0.0));

        // Using a DependencyProperty as the backing store for SubTotal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubTotalProperty =
            DependencyProperty.Register("SubTotal", typeof(double), typeof(CreateOrder), new PropertyMetadata(0.0));

        public CreateOrder()
        {
            InitializeComponent();
            this.cbCust.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbProd.ItemsSource = HomepageViewModel.CommonViewModel.ProductRepository.GetAllProducts();
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            var product = this.cbProd.SelectedItem as ProductModel;
            var orderedProduct = new OrderedProduct();
            if (product != null)
            {
                product.Rate = Convert.ToDouble(this.tbRate.Text);
                orderedProduct.Product = product;
                orderedProduct.Quantity = Convert.ToInt32(this.updStock.Value);
                orderedProduct.SubTotal = this.SubTotal;
                orderedProduct.Total = this.SubTotal + (this.SubTotal * Convert.ToDouble(Convert.ToDouble(this.tbGst.Text) / 100));
                orderedProduct.Gst = Convert.ToDouble(this.tbGst.Text);
                this.cartproducts.Add(orderedProduct);
                this.dtCart.ItemsSource = this.cartproducts;
                this.NetTotal = this.NetTotal + orderedProduct.Total;
            }
        }

        private void cbProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var product = comboBox.SelectedItem as ProductModel;
                if (product != null)
                {
                    products.Clear();
                    products.Add(product);
                    this.prodGrid.ItemsSource = products;
                    this.updStock.Maximum = product.Stock;
                    this.tbMaxSP.Text = product.MaxSellingPrice.ToString();
                    this.tbGst.Text = product.Category.GstRate.ToString();
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[-+]?[0-9]*(\\.[0-9]+)");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tbrate = sender as TextBox;
            if (tbrate != null)
            {
                if (string.IsNullOrEmpty(tbrate.Text))
                {
                    this.tbSubTotal.Text = "0";
                    this.SubTotal = 0;
                }
                else
                {
                    this.tbSubTotal.Text = (Convert.ToDouble(this.updStock.Value) * Convert.ToDouble(tbrate.Text)).ToString();
                    this.SubTotal = Convert.ToDouble(this.updStock.Value) * Convert.ToDouble(tbrate.Text);
                }                
            }

            
        }

        public double SubTotal
        {
            get { return (double)GetValue(SubTotalProperty); }
            set { SetValue(SubTotalProperty, value); }
        }

        public double NetTotal
        {
            get { return (double)GetValue(NetTotalProperty); }
            set { SetValue(NetTotalProperty, value); }
        }

        private void NumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            var disc = sender as NumericUpDown; 
            if (disc != null)
            {
                this.tbValDisc.Text = (this.NetTotal - (this.NetTotal * (disc.Value/100))).ToString();
            }            
        }

        private void cbCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var customer = comboBox.SelectedItem as CustomerModel;
                if (customer != null)
                {
                    this.tbAddress.Text = customer.Address + Environment.NewLine +
                        customer.District + Environment.NewLine +
                        customer.City + Environment.NewLine +
                        customer.State + Environment.NewLine + customer.Country + "-" + customer.PinCode + Environment.NewLine +
                        "Contact: " + customer.PhoneNumber;
                }
            }
        }

        private void GenerateOrder(object sender, RoutedEventArgs e)
        {
            var order = new OrderModel();
            order.OrderId = "BK" + Encoding.Unicode.GetBytes(DateTime.Now.ToString());
            order.Amount = this.NetTotal;
            order.AdvanceAmount = Convert.ToDouble(this.tbAmt.Text);
            if (order.AdvanceAmount == order.Amount)
            {
                order.PaymentStatus = Model.Enums.PaymentStatus.Paid;
            }
            else if (order.AdvanceAmount == 0)
            {
                order.PaymentStatus = Model.Enums.PaymentStatus.Unpaid;
            }
            else
            {
                order.PaymentStatus = Model.Enums.PaymentStatus.PartialPaid;
            }

            if (this.udVal.IsEnabled)
            {
                order.Discount = Convert.ToDouble(this.udVal.Value);
                order.AmountAfterDiscount = Convert.ToDouble(this.tbValDisc.Text);
                order.Balance = order.AmountAfterDiscount - order.AdvanceAmount;
            }
            else
            {
                order.Balance = order.Amount - order.AdvanceAmount;
            }
            
            order.OrderStatus = Model.Enums.OrderStatus.Awaiting;
            order.PaymentType = Model.Enums.PaymentType.Cash;
            order.OrderedProducts = this.cartproducts;
            order.NextFollowup = (DateTime)this.nextDate.SelectedDateTime;
            order.BillDate = (DateTime)this.billDate.SelectedDate;
            order.BillNumber = this.tbBill.Text;
            order.Priority = (bool)this.cbPriority.IsChecked;
        }
    }
}
