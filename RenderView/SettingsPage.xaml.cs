using Cosmetify.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cosmetify.RenderView
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        // Using a DependencyProperty as the backing store for ProfilePicture.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfilePictureProperty =
            DependencyProperty.Register("ProfilePicture", typeof(BitmapImage), typeof(SettingsPage));

        // Using a DependencyProperty as the backing store for OrderStageCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderStageCollectionProperty =
            DependencyProperty.Register("OrderStageCollection", typeof(ObservableCollection<OrderStageModel>), typeof(SettingsPage));

        // Using a DependencyProperty as the backing store for MasterFormulaCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MasterFormulaCollectionProperty =
            DependencyProperty.Register("MasterFormulaCollection", typeof(ObservableCollection<MasterFormulaModel>), typeof(SettingsPage), new PropertyMetadata(new ObservableCollection<MasterFormulaModel>()));


        public SettingsPage()
        {
            InitializeComponent();
            this.MasterFormulaCollection = HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
            this.dgColor.ItemsSource = HomepageViewModel.CommonViewModel.ColoursRepository.GetColors();
            this.dgPerfume.ItemsSource = HomepageViewModel.CommonViewModel.PerfumeRepository.GetAllPerfumes();
        }

        public BitmapImage ProfilePicture
        {
            get { return (BitmapImage)GetValue(ProfilePictureProperty); }
            set { SetValue(ProfilePictureProperty, value); }
        }

        public ObservableCollection<OrderStageModel> OrderStageCollection
        {
            get { return (ObservableCollection<OrderStageModel>)GetValue(OrderStageCollectionProperty); }
            set { SetValue(OrderStageCollectionProperty, value); }
        }

        public ObservableCollection<MasterFormulaModel> MasterFormulaCollection
        {
            get { return (ObservableCollection<MasterFormulaModel>)GetValue(MasterFormulaCollectionProperty); }
            set { SetValue(MasterFormulaCollectionProperty, value); }
        }

        private void Upload_Profile_Picture(object sender, RoutedEventArgs e)
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
                this.ProfilePicture = new BitmapImage();
                this.ProfilePicture.BeginInit();
                this.ProfilePicture.CacheOption = BitmapCacheOption.OnLoad;
                this.ProfilePicture.UriSource = new Uri(dlg.FileName);
                this.ProfilePicture.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                this.ProfilePicture.DecodePixelWidth = 100;
                this.ProfilePicture.EndInit();
            }
        }

        private void Save_Profile(object sender, RoutedEventArgs e)
        {

        }

        private void Upload_Logo(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Performa_Details(object sender, RoutedEventArgs e)
        {

        }

        private void Change_Password(object sender, RoutedEventArgs e)
        {

        }

        //private void CreateDepartment(object sender, RoutedEventArgs e)
        //{
        //    var addDepart = new AddDepartments();
        //    var result = addDepart.ShowDialog();
        //    if (result == true)
        //    {
        //        this.departList.Items.Add(addDepart.Department);
        //    }
        //}

        private void CreateOdStages(object sender, RoutedEventArgs e)
        {
            var addOdStage = new AddOrderStages();
            var result = addOdStage.ShowDialog();
            if (result == true)
            {
                this.OrderStageCollection = addOdStage.OrderStagesList;
            }
        }

        private void CreateMasterFormula(object sender, RoutedEventArgs e)
        {
            var addMasterFormula = new AddMasterFormula();
            addMasterFormula.ActivesList.Clear();
            addMasterFormula.ShowDialog();
            if (!string.IsNullOrEmpty(addMasterFormula.FormulaName) || !string.IsNullOrEmpty(addMasterFormula.FormulaCode) || addMasterFormula.ActivesList.Count > 0)
            {
                var model = new MasterFormulaModel();
                model.Name = addMasterFormula.FormulaName;
                model.Code = addMasterFormula.FormulaCode;
                model.Requirements = new ObservableCollection<MasterProductModel>();
                foreach (var item in addMasterFormula.ActivesList)
                {
                    model.Requirements.Add(item);
                }

                model.RemainingWater = addMasterFormula.RemainingWater;
                HomepageViewModel.CommonViewModel.MasterFormulaRepository.InsertFormula(model);
                this.MasterFormulaCollection = HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
            }
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                var grid = sender as Grid;
                if (grid != null && grid.DataContext is MasterFormulaModel)
                {
                    var model = grid.DataContext as MasterFormulaModel;
                    var addMasterFormula = new AddMasterFormula();
                    addMasterFormula.FormulaName = model.Name;
                    addMasterFormula.FormulaCode = model.Code;
                    addMasterFormula.ActivesList.Clear();
                    foreach (var item in model.Requirements)
                    {
                        addMasterFormula.ActivesList.Add(item);
                    }
                    addMasterFormula.RemainingWater = model.RemainingWater;
                    addMasterFormula.ShowDialog();
                    if (!string.IsNullOrEmpty(addMasterFormula.FormulaName) || !string.IsNullOrEmpty(addMasterFormula.FormulaCode) || addMasterFormula.ActivesList.Count > 0)
                    {
                        model.Name = addMasterFormula.FormulaName;
                        model.Code = addMasterFormula.FormulaCode;
                        model.Requirements.Clear();
                        foreach (var item in addMasterFormula.ActivesList)
                        {
                            model.Requirements.Add(item);
                        }

                        model.RemainingWater = addMasterFormula.RemainingWater;
                        HomepageViewModel.CommonViewModel.MasterFormulaRepository.UpdateFormula(model);
                        this.MasterFormulaCollection = HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
                    }
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as System.Windows.Controls.MenuItem;
            if (menu != null)
            {
                var model = menu.DataContext as MasterFormulaModel;
                if (model != null)
                {                    
                    HomepageViewModel.CommonViewModel.MasterFormulaRepository.DeleteFormula(model.Id);
                    this.MasterFormulaCollection = HomepageViewModel.CommonViewModel.MasterFormulaRepository.GetAllFormulas();
                }
            }
        }

        private void PushToDBClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddColors(object sender, RoutedEventArgs e)
        {
            var dialog = new AddColorPerfume(true);
            if ((bool)dialog.ShowDialog())
            {
                this.dgColor.ItemsSource = HomepageViewModel.CommonViewModel.ColoursRepository.GetColors();
            }            
        }

        private void UploadColors(object sender, RoutedEventArgs e)
        {

        }

        private void AddPerfumes(object sender, RoutedEventArgs e)
        {
            var dialog = new AddColorPerfume(false);
            if ((bool)dialog.ShowDialog())
            {
                this.dgPerfume.ItemsSource = HomepageViewModel.CommonViewModel.PerfumeRepository.GetAllPerfumes();
            }
        }

        private void UploadPerfumes(object sender, RoutedEventArgs e)
        {

        }
    }
}
