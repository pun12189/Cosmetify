using Cosmetify.Model;
using Cosmetify.Model.Enums;
using Cosmetify.ViewModel;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Cosmetify.RenderView.UserControls
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : UserControl
    {
        private string MfgMonth;

        private string ExpMonth;

        private string MfgYear;

        private string ExpYear;

        // Using a DependencyProperty as the backing store for Products.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductsProperty =
            DependencyProperty.Register("Products", typeof(ObservableCollection<ProductModel>), typeof(AddProduct), new PropertyMetadata(new ObservableCollection<ProductModel>()));
        
        // Using a DependencyProperty as the backing store for CategoryList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryListProperty =
            DependencyProperty.Register("CategoryList", typeof(ObservableCollection<CategoryModel>), typeof(AddProduct), new PropertyMetadata(new ObservableCollection<CategoryModel>()));

        // Using a DependencyProperty as the backing store for ProductImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductImageProperty =
            DependencyProperty.Register("ProductImage", typeof(BitmapImage), typeof(AddProduct));

        public AddProduct()
        {
            InitializeComponent();
            this.CategoryList = HomepageViewModel.CommonViewModel.CategoriesList;
            this.Products = HomepageViewModel.CommonViewModel.ProductRepository.GetAllProducts();
            this.cbMnth.ItemsSource = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames.Take(12).ToList();
            this.cbYear.ItemsSource = Enumerable.Range(2015, DateTime.Now.Year - 2015 + 1).ToList();
            this.cbMnth1.ItemsSource = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames.Take(12).ToList();
            this.cbYear1.ItemsSource = Enumerable.Range(2022, 20).ToList();
            this.cbStk.ItemsSource = new ObservableCollection<string> { "grams", "pieces", "ltrs"};
        }

        public ObservableCollection<ProductModel> Products
        {
            get { return (ObservableCollection<ProductModel>)GetValue(ProductsProperty); }
            set { SetValue(ProductsProperty, value); }
        }

        public ObservableCollection<CategoryModel> CategoryList
        {
            get { return (ObservableCollection<CategoryModel>)GetValue(CategoryListProperty); }
            set { SetValue(CategoryListProperty, value); }
        }

        public BitmapImage ProductImage
        {
            get { return (BitmapImage)GetValue(ProductImageProperty); }
            set { SetValue(ProductImageProperty, value); }
        }

        private void SaveProduct(object sender, RoutedEventArgs e)
        {
            var product = new ProductModel();
            product.Name = this.tbName.Text;
            product.Description = this.tbDesc.Text;
            product.Packing = this.tbPack.Text;
            product.BatchNo = this.tbBatch.Text;
            product.MfgDate = Convert.ToDateTime("01/" + this.MfgMonth + "/" + this.MfgYear);
            /*if (this.tgStatus.IsChecked == true)
            {
                product.Status = ProductStatus.Published;
            }
            else
            {
                product.Status = ProductStatus.Block;
            }*/
            product.Status = ProductStatus.Published;
            product.Expiry = Convert.ToDateTime("01/" + this.ExpMonth + "/" + this.ExpYear);
            product.Stock = Convert.ToInt32(this.tbStock.Text);
            product.PurchasePrice = Convert.ToDouble(this.tbPp.Text);
            product.MRP = Convert.ToDouble(this.tbMrp.Text);
            if (!string.IsNullOrEmpty(this.tbMaxsp.Text))
            {
                product.MaxSellingPrice = Convert.ToDouble(this.tbMaxsp.Text);
            }

            if (!string.IsNullOrEmpty(this.tbMinsp.Text))
            {
                product.MinSellingPrice = Convert.ToDouble(this.tbMinsp.Text);
            }
            
            product.MfrName = this.tbMfr.Text;
            product.Company = this.tbComp.Text;
            product.Category = this.cbCateg.SelectedItem as CategoryModel;
            product.ProductImage = this.ProductImage;
           
            HomepageViewModel.CommonViewModel.ProductRepository.InsertProduct(product);
            this.Products = HomepageViewModel.CommonViewModel.ProductRepository.GetAllProducts();
        }

        private void UploadProductImage(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "png Files (*.png)|*.png|jpg Files (*.jpg)|*.jpeg";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                this.ProductImage = new BitmapImage();
                this.ProductImage.BeginInit();
                this.ProductImage.CacheOption = BitmapCacheOption.OnLoad;
                this.ProductImage.UriSource = new Uri(dlg.FileName);
                this.ProductImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                this.ProductImage.DecodePixelWidth = 50;
                this.ProductImage.EndInit();
            }
        }

        private void cbMnth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                if (cb.SelectedItem != null)
                {
                    this.MfgMonth = (cb.SelectedIndex + 1).ToString();
                }
            }
        }

        private void cbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                if (cb.SelectedItem != null)
                {
                    this.MfgYear = cb.SelectedItem.ToString();
                }
            }
        }

        private void cbMnth1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                if (cb.SelectedItem != null)
                {
                    this.ExpMonth = (cb.SelectedIndex + 1).ToString();
                }
            }
        }

        private void cbYear1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                if (cb.SelectedItem != null)
                {
                    this.ExpYear = cb.SelectedItem.ToString();
                }
            }
        }

        private void tb_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbMrp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbMrp.Text) && !string.IsNullOrEmpty(this.tbPp.Text))
            {
                var pp = Convert.ToDouble(this.tbPp.Text);
                var mrp = Convert.ToDouble(this.tbMrp.Text);
                if (pp > mrp || mrp < pp)
                {
                    MessageBox.Show("MRP should not be less than Purchase Price.", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void tbMrp_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbMrp.Text) && !string.IsNullOrEmpty(this.tbPp.Text))
            {
                var pp = Convert.ToDouble(this.tbPp.Text);
                var mrp = Convert.ToDouble(this.tbMrp.Text);
                if (pp > mrp || mrp < pp)
                {
                    MessageBox.Show("MRP should not be less than Purchase Price.", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
    }
}
