using BahiKitaab.Dialogs;
using BahiKitaab.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace BahiKitaab.RenderView
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Page
    {
        // Using a DependencyProperty as the backing store for ProfilePicture.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfilePictureProperty =
            DependencyProperty.Register("ProfilePicture", typeof(BitmapImage), typeof(AdminView));

        // Using a DependencyProperty as the backing store for OrderStageCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderStageCollectionProperty =
            DependencyProperty.Register("OrderStageCollection", typeof(ObservableCollection<OrderStageModel>), typeof(AdminView));


        public AdminView()
        {
            InitializeComponent();
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
    }
}
