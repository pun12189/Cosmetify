using BahiKitaab.Model;
using BahiKitaab.Repository;
using BahiKitaab.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BahiKitaab.RenderView
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        // Using a DependencyProperty as the backing store for Categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register("Categories", typeof(ObservableCollection<CategoryModel>), typeof(CategoriesPage), new PropertyMetadata(new ObservableCollection<CategoryModel>()));

        // Using a DependencyProperty as the backing store for Categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubCategoriesProperty = DependencyProperty.Register("SubCategories", typeof(ObservableCollection<SubCategoryModel>), typeof(CategoriesPage), new PropertyMetadata(new ObservableCollection<SubCategoryModel>()));

        // Using a DependencyProperty as the backing store for Categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubSubCategoriesProperty = DependencyProperty.Register("SubSubCategories", typeof(ObservableCollection<SubSubCategoryModel>), typeof(CategoriesPage), new PropertyMetadata(new ObservableCollection<SubSubCategoryModel>()));

        public CategoriesPage()
        {
            InitializeComponent();
            /*this.SubCategoryRepository = new SubCategoryRepository();
            this.CategoryRepository = new CategoryRepository(this.SubCategoryRepository);
            this.SubSubCategoryRepository = new SubSubCategoryRepository(this.SubCategoryRepository);
            this.SubCategoryRepository.CategoryRepository = this.CategoryRepository;
            this.SubCategoryRepository.SubSubCategoryRepository = this.SubSubCategoryRepository;*/            
            this.UpdateCategories();
            this.UpdateSubCategories();
            this.UpdateSubSubCategories();
        }

        public ObservableCollection<CategoryModel> Categories
        {
            get { return (ObservableCollection<CategoryModel>)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }

        public ObservableCollection<SubCategoryModel> SubCategories
        {
            get { return (ObservableCollection<SubCategoryModel>)GetValue(SubCategoriesProperty); }
            set { SetValue(SubCategoriesProperty, value); }
        }

        public ObservableCollection<SubSubCategoryModel> SubSubCategories
        {
            get { return (ObservableCollection<SubSubCategoryModel>)GetValue(SubSubCategoriesProperty); }
            set { SetValue(SubSubCategoriesProperty, value); }
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            var categ = new CategoryModel() { CategoryName = this.tbCateg.Text, GstRate = Convert.ToDouble(((ComboBoxItem)cbGst.SelectedItem).Tag.ToString()) };
            HomepageViewModel.CommonViewModel.CategoryRepository.InsertCategory(categ.CategoryName, categ.GstRate);
            this.tbCateg.Text = string.Empty;
            this.cbGst.SelectedIndex = 0;
            this.UpdateCategories();
        }

        private void AddSubCategory(object sender, RoutedEventArgs e)
        {
            var categ = new SubCategoryModel() { CategoryName = this.tbSubCateg.Text };
            var mainCateg = this.cbCateg.SelectedItem as CategoryModel;
            if (mainCateg != null)
            {
                HomepageViewModel.CommonViewModel.SubCategoryRepository.InsertSubCategory(categ.CategoryName, mainCateg.Id);
            }

            this.tbSubCateg.Text = string.Empty;
            this.cbCateg.SelectedIndex = 0;
            this.UpdateSubCategories();
            this.UpdateSubSubCategories();
        }

        private void AddSubSubCategory(object sender, RoutedEventArgs e)
        {
            var categ = new SubSubCategoryModel() { CategoryName = this.tbSSubCateg.Text };
            var mainCateg = this.cbSubCateg.SelectedItem as SubCategoryModel;
            if (mainCateg != null)
            {
                HomepageViewModel.CommonViewModel.SubSubCategoryRepository.InsertSubSubCategory(categ.CategoryName, mainCateg.Id);
            }

            this.tbSSubCateg.Text = string.Empty;
            this.cbSubCateg.SelectedIndex = 0;
            this.UpdateCategories();
            this.UpdateSubCategories();
            this.UpdateSubSubCategories();
        }

        private void UpdateCategories()
        {
            var categories = HomepageViewModel.CommonViewModel.CategoryRepository.GetCategories();
            this.cbCateg.ItemsSource = categories;
            this.Categories = categories;
        }

        private void UpdateSubCategories()
        {
            var categories = HomepageViewModel.CommonViewModel.SubCategoryRepository.GetSubCategories();
            this.cbSubCateg.ItemsSource = categories;
            this.SubCategories = categories;
        }

        private void UpdateSubSubCategories()
        {
            var categories = HomepageViewModel.CommonViewModel.SubSubCategoryRepository.GetSubSubCategories();
            this.SubSubCategories = categories;
        }

        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                CategoryModel category = e.Row.DataContext as CategoryModel;
                if (category != null)
                {
                    if (category.Id > 0)
                    {
                        HomepageViewModel.CommonViewModel.CategoryRepository.UpdateCategory(category.Id, category.CategoryName, category.GstRate);
                    }
                    else
                    {
                        HomepageViewModel.CommonViewModel.CategoryRepository.InsertCategory(category.CategoryName, category.GstRate);
                    }
                }
            }
        }

        private void dataGrid1_PreviewExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg != null)
            {
                CategoryModel category = dg.SelectedItem as CategoryModel;
                if (e.Command == DataGrid.DeleteCommand && category != null)
                {
                    HomepageViewModel.CommonViewModel.CategoryRepository.DeleteCategory(category.Id);
                }
            }
        }

        private void dataGrid2_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                SubCategoryModel category = e.Row.DataContext as SubCategoryModel;
                if (category != null)
                {
                    if (category.Id > 0)
                    {
                        HomepageViewModel.CommonViewModel.SubCategoryRepository.UpdateSubCategory(category.Id, category.CategoryName, category.ParentCategoryId);
                    }
                    else
                    {
                        HomepageViewModel.CommonViewModel.SubCategoryRepository.InsertSubCategory(category.CategoryName, category.ParentCategoryId);
                    }
                }
            }
        }

        private void dataGrid2_PreviewExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg != null)
            {
                SubCategoryModel category = dg.SelectedItem as SubCategoryModel;
                if (e.Command == DataGrid.DeleteCommand && category != null)
                {
                    HomepageViewModel.CommonViewModel.SubCategoryRepository.DeleteSubCategory(category.Id);
                }
            }
        }

        private void dataGrid3_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                SubSubCategoryModel category = e.Row.DataContext as SubSubCategoryModel;
                if (category != null)
                {
                    if (category.Id > 0)
                    {
                        HomepageViewModel.CommonViewModel.SubSubCategoryRepository.UpdateSubSubCategory(category.Id, category.CategoryName, category.ParentCategoryId);
                    }
                    else
                    {
                        HomepageViewModel.CommonViewModel.SubSubCategoryRepository.InsertSubSubCategory(category.CategoryName, category.ParentCategoryId);
                    }
                }
            }
        }

        private void dataGrid3_PreviewExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg != null)
            {
                SubSubCategoryModel category = dg.SelectedItem as SubSubCategoryModel;
                if (e.Command == DataGrid.DeleteCommand && category != null)
                {
                    HomepageViewModel.CommonViewModel.SubSubCategoryRepository.DeleteSubSubCategory(category.Id);
                }
            }
        }
    }
}
