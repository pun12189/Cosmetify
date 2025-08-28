using Cosmetify.Model;
using Cosmetify.Model.Enums;
using Cosmetify.RenderView;
using Cosmetify.ViewModel;
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

namespace Cosmetify.Dialogs
{
    /// <summary>
    /// Interaction logic for OrderEditViewPage.xaml
    /// </summary>
    public partial class OrderEditViewPage : Window
    {
        public ObservableCollection<BatchOrderModel> BatchOrderCollection
        {
            get { return (ObservableCollection<BatchOrderModel>)GetValue(BatchOrderCollectionProperty); }
            set { SetValue(BatchOrderCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BatchOrderCollectionProperty =
            DependencyProperty.Register("BatchOrderCollection", typeof(ObservableCollection<BatchOrderModel>), typeof(OrderEditViewPage), new PropertyMetadata(new ObservableCollection<BatchOrderModel>()));

        public ObservableCollection<BatchModel> BatchModelCollection
        {
            get { return (ObservableCollection<BatchModel>)GetValue(BatchModelCollectionProperty); }
            set { SetValue(BatchModelCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BatchModelCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BatchModelCollectionProperty =
            DependencyProperty.Register("BatchModelCollection", typeof(ObservableCollection<BatchModel>), typeof(OrderEditViewPage), new PropertyMetadata(new ObservableCollection<BatchModel>()));

        public CustomerModel CustomerName
        {
            get { return (CustomerModel)GetValue(CustomerNameProperty); }
            set { SetValue(CustomerNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomerName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerNameProperty =
            DependencyProperty.Register("CustomerName", typeof(CustomerModel), typeof(OrderEditViewPage), new PropertyMetadata());

        public string BrandName
        {
            get { return (string)GetValue(BrandNameProperty); }
            set { SetValue(BrandNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BrandName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BrandNameProperty =
            DependencyProperty.Register("BrandName", typeof(string), typeof(OrderEditViewPage), new PropertyMetadata(string.Empty));

        public OrderEditViewPage()
        {
            InitializeComponent();
            this.Loaded += this.OrderEditViewPage_Loaded;
            
        }

        private async void OrderEditViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbProduct.ItemsSource = await HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
            this.cbColor.ItemsSource = HomepageViewModel.CommonViewModel.ColoursRepository.GetColors();
            this.cbCust.ItemsSource = await HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
            this.cbCust.SelectedValue = this.CustomerName.Id;
        }

        private void cbProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbProd.SelectedItem != null && this.cbProd.SelectedItem is ActivesModel)
            {
                var batchOrder = new BatchOrderModel();
                batchOrder.Actives = this.cbProd.SelectedItem as ActivesModel;
                batchOrder.Units = batchOrder.Actives.Units;
                this.BatchOrderCollection.Add(batchOrder);
            }
        }

        private void cbCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbCust.SelectedItem != null && this.cbCust.SelectedItem is CustomerModel)
            {
                var cust = this.cbCust.SelectedItem as CustomerModel;
                if (cust != null)
                {
                    this.cbBrand.ItemsSource = HomepageViewModel.CommonViewModel.LeadsRepository.GetBrandsOfCustomer(cust.Id);
                    this.cbBrand.SelectedItem = this.BrandName;
                }
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectCategory();

            if ((bool)dialog.ShowDialog())
            {
                this.cbProd.ItemsSource = dialog.ItemsCollection;
            }
        }

        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderid = this.BatchModelCollection[0].OrderId;
            foreach (var batchModel in this.BatchModelCollection)
            {
                var model = HomepageViewModel.CommonViewModel.BatchOrderRepository.GetProduct(batchModel.Id);
                if (model != null) 
                {
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.UpdateProduct(batchModel);
                }
                else
                {
                    batchModel.OrderId = orderid;
                    batchModel.Status = BatchStatus.Created;
                    batchModel.BatchDate = DateTime.Now;
                    HomepageViewModel.CommonViewModel.BatchOrderRepository.InsertProduct(batchModel);
                }
            }
            
            this.Close();
        }

        private async void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = this.cbProduct.SelectedItem as MasterFormulaModel;
            if (t != null)
            {
                this.BatchOrderCollection.Clear();
                foreach (var actives in t.Requirements)
                {
                    var batchOrder = new BatchOrderModel();
                    batchOrder.Actives = await HomepageViewModel.CommonViewModel.ActivesRepository.GetProduct(actives.Id);
                    batchOrder.PercentageRequired = actives.Required;
                    this.BatchOrderCollection.Add(batchOrder);
                }
            }
        }

        private async void btnAddProd_Click(object sender, RoutedEventArgs e)
        {
            var addCust = new AddMasterFormula();
            addCust.ActivesList.Clear();
            if ((bool)addCust.ShowDialog())
            {
                if (!string.IsNullOrEmpty(addCust.FormulaName) || !string.IsNullOrEmpty(addCust.FormulaCode) || addCust.ActivesList.Count > 0)
                {
                    var model = new MasterFormulaModel();
                    model.Name = addCust.FormulaName;
                    model.Code = addCust.FormulaCode;
                    model.Requirements = new ObservableCollection<MasterProductModel>();
                    foreach (var item in addCust.ActivesList)
                    {
                        model.Requirements.Add(item);
                    }

                    model.RemainingWater = addCust.RemainingWater;
                    HomepageViewModel.CommonViewModel.MasterFormulaRepository.InsertFormula(model);
                    this.cbProd.ItemsSource = await HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
                    MessageBox.Show("List Refreshed, Please select added formula", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnAddColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddColorPerfume(true);
            if ((bool)dialog.ShowDialog())
            {
                this.cbColor.ItemsSource = HomepageViewModel.CommonViewModel.ColoursRepository.GetColors();
            }
        }

        private void DeleteBatch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var model = button.DataContext as BatchModel;
                if (model != null)
                {
                    this.BatchModelCollection.Remove(model);
                }
            }
        }

        private void dgBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dgrid = sender as DataGrid;
            if (dgrid != null)
            {
                var model = dgrid.SelectedItem as BatchModel;
                if (model != null) 
                {
                    this.cbColor.SelectedItem = model.Colour;
                    foreach (var claim in model.Claims)
                    {
                        this.tbClaims.Text += claim + ",";
                    }
                    
                    this.BatchOrderCollection = model.BatchOrderCollection;
                }
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var batchModel = new BatchModel();
            if (this.chCust != null && this.chCust.IsChecked == true)
            {
                if (this.tbPname != null && string.IsNullOrEmpty(this.tbPname.Text))
                {
                    MessageBox.Show("Enter Product Name", "Custom Product", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return;
                }
                else
                {
                    batchModel.ProductName = this.tbPname.Text;
                    batchModel.ProductID = "PRD" + Math.Abs(DateTime.Now.GetHashCode());
                }
            }
            else
            {
                var mfmodel = this.cbProduct.SelectedItem as MasterFormulaModel;
                if (mfmodel != null)
                {
                    batchModel.ProductID = mfmodel.Code;
                    if (string.IsNullOrEmpty(mfmodel.Name))
                    {
                        batchModel.ProductName = mfmodel.Code;
                    }
                    else
                    {
                        batchModel.ProductName = mfmodel.Name;
                    }

                }
            }

            batchModel.BatchOrderNo = "COS-" + Math.Abs(DateTime.Now.GetHashCode()).ToString();
            var cust = this.cbCust.SelectedItem as CustomerModel;
            if (cust != null)
            {
                batchModel.Customer = cust;
            }

            var bname = this.cbBrand.SelectedItem as string;
            if (bname != null)
            {
                batchModel.BrandName = bname;
            }

            foreach (var item in this.BatchOrderCollection)
            {
                batchModel.BatchOrderCollection.Add(item);
            }

            var color = this.cbColor.SelectedItem as ColoursModel;
            if (color != null)
            {
                batchModel.Colour = color.Name;
            }           

            var claims = this.tbClaims.Text.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (claims.Length > 0)
            {
                batchModel.Claims = new ObservableCollection<string>(claims);
            }

            if (!string.IsNullOrEmpty(this.tbMProd.Text))
            {
                batchModel.AdditionalInfo = this.tbMProd.Text;
            }            

            this.BatchModelCollection.Add(batchModel);

            this.cbCust.IsEnabled = false;
            this.cbBrand.SelectedIndex = -1;
            this.cbProduct.SelectedIndex = -1;
            this.tbMProd.Text = string.Empty;;
            this.cbColor.SelectedIndex = -1;
            this.tbClaims.Text = string.Empty;
            this.cbProd.SelectedIndex = -1;
            this.BatchOrderCollection.Clear();
        }
    }
}
