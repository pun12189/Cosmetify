using Cosmetify.Model;
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
    /// Interaction logic for SelectCategory.xaml
    /// </summary>
    public partial class SelectCategory : Window
    {
        public SelectCategory()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.LoadComponents();
        }

        private async void LoadComponents()
        {
            this.cbCateg.ItemsSource = await HomepageViewModel.CommonViewModel.CategoryRepository.GetCategories();
        }

        public ObservableCollection<ActivesModel> ItemsCollection
        {
            get { return (ObservableCollection<ActivesModel>)GetValue(ItemsCollectionProperty); }
            set { SetValue(ItemsCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsCollectionProperty =
            DependencyProperty.Register("ItemsCollection", typeof(ObservableCollection<ActivesModel>), typeof(SelectCategory), new PropertyMetadata(new ObservableCollection<ActivesModel>()));

        private void cbCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as System.Windows.Controls.ComboBox;
            if (cb != null && cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem as CategoryModel;
                if (item != null)
                {
                    this.cbSCateg.ItemsSource = item.SubCategories;
                }
            }
        }

        private void cbSCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as System.Windows.Controls.ComboBox;
            if (cb != null && cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem as SubCategoryModel;
                if (item != null)
                {
                    this.cbSSCateg.ItemsSource = item.SubSubCategories;
                }
            }
        }

        private async void cbSSCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as System.Windows.Controls.ComboBox;
            if (cb != null && cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem as SubSubCategoryModel;
                if (item != null)
                {
                    this.ItemsCollection = await HomepageViewModel.CommonViewModel.ActivesRepository.SearchActivesBySSubCategory(item.Id);
                }
            }
        }

        private void btnAply_Click(object sender, RoutedEventArgs e)
        {
            if (this.cbCateg.SelectedIndex < 0)
            {
                MessageBox.Show("Please select category first.", "Category", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (this.cbSCateg.SelectedIndex < 0)
            {
                MessageBox.Show("Please select sub-category first.", "Category", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (this.cbSSCateg.SelectedIndex < 0)
            {
                MessageBox.Show("Please select Sub-sub-category first.", "Category", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
