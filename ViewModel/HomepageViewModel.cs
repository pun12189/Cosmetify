using Cosmetify.Command;
using Cosmetify.Helper;
using Cosmetify.Model;
using Cosmetify.RenderView;
using MahApps.Metro.IconPacks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MenuItem = Cosmetify.Model.MenuItem;

namespace Cosmetify.ViewModel
{
    class HomepageViewModel : INotifyPropertyChanged
    {
        #region Properties
        public Action CloseAction { get; set; }

        private Window window;

        public ICommand LogoutCommand { get; set; }

        private string _welcomeText;

        public string WelcomeText
        {
            get { return _welcomeText; }
            set
            {
                if (_welcomeText == value) return;
                _welcomeText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WelcomeText"));
            }
        }

        public ObservableCollection<MenuItem> Menu { get; }

        public ObservableCollection<MenuItem> OptionsMenu { get; }

        public static CommonViewModel CommonViewModel { get; private set; }
        #endregion

        static HomepageViewModel()
        {
            CommonViewModel = new CommonViewModel();
        }

        #region Constructor
        public HomepageViewModel(Window window)
        {
            this.window = window;
            LogoutCommand = new RelayCommand(LogoutCommandExecute);
            WelcomeText = UserModel.Instance.Email;
            this.Menu = new ObservableCollection<MenuItem>();
            this.OptionsMenu = new ObservableCollection<MenuItem>();

            // Build the menus
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.AirbnbBrands },
                Label = "Dashboard",
                NavigationType = typeof(DashboardPage),
                NavigationDestination = new Uri("RenderView/DashboardPage.xaml", UriKind.RelativeOrAbsolute)
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.DashcubeBrands },
                Label = "Products",
                NavigationType = typeof(InventoryView),
                NavigationDestination = new Uri("RenderView/InventoryView.xaml", UriKind.RelativeOrAbsolute),
                IsVisible = false
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CatSolid },
                Label = "Categories",
                NavigationType = typeof(CategoriesPage),
                NavigationDestination = new Uri("RenderView/CategoriesPage.xaml", UriKind.RelativeOrAbsolute)
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.FileInvoiceSolid },
                Label = "Inventory",
                NavigationType = typeof(ActivesView),
                NavigationDestination = new Uri("RenderView/ActivesView.xaml", UriKind.RelativeOrAbsolute)
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.FileInvoiceSolid },
                Label = "Procurement",
                NavigationType = typeof(PurchaseView),
                NavigationDestination = new Uri("RenderView/PurchaseView.xaml", UriKind.RelativeOrAbsolute)
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.FirstOrderBrands },
                Label = "Batch Order",
                NavigationType = typeof(BatchOrder),
                NavigationDestination = new Uri("RenderView/BatchOrder.xaml", UriKind.RelativeOrAbsolute),
                IsEnabled = true
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.FirstOrderBrands },
                Label = "Sales Order",
                NavigationType = typeof(OrderInvoice),
                NavigationDestination = new Uri("RenderView/OrderInvoice.xaml", UriKind.RelativeOrAbsolute),
                IsEnabled = true
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.UsersSolid },
                Label = "Customer",
                NavigationType = typeof(LeadsView),
                NavigationDestination = new Uri("RenderView/LeadsView.xaml", UriKind.RelativeOrAbsolute)
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.SalesforceBrands },
                Label = "Sales",
                NavigationType = typeof(SalesPage),
                NavigationDestination = new Uri("RenderView/SalesPage.xaml", UriKind.RelativeOrAbsolute),
                IsEnabled = false
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.VenusDoubleSolid },
                Label = "Vendors",
                NavigationType = typeof(VendorsPage),
                NavigationDestination = new Uri("RenderView/VendorsPage.xaml", UriKind.RelativeOrAbsolute),
                IsEnabled = false
            });            
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.ChartBarSolid },
                Label = "Batch Reports",
                NavigationType = typeof(ReportsPage),
                NavigationDestination = new Uri("RenderView/ReportsPage.xaml", UriKind.RelativeOrAbsolute),
                IsEnabled = false
            });

            this.OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.UserCogSolid },
                Label = "Settings",
                NavigationType = typeof(SettingsPage),
                NavigationDestination = new Uri("RenderView/SettingsPage.xaml", UriKind.RelativeOrAbsolute),
                IsEnabled = true
            });
            this.OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.UserCogSolid },
                Label = "Reminders",
                NavigationType = typeof(RemindersPage),
                NavigationDestination = new Uri("RenderView/RemindersPage.xaml", UriKind.RelativeOrAbsolute),
                IsEnabled = false
            });
            this.OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.InfoCircleSolid },
                Label = "About",
                NavigationType = typeof(AboutPage),
                NavigationDestination = new Uri("RenderView/AboutPage.xaml", UriKind.RelativeOrAbsolute),
                IsEnabled = false

            });

            this.OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.PowerOffSolid },
                Label = "LogOut",
                Command = LogoutCommand
            });
        }
        #endregion

        #region Private Methods
        private void LogoutCommandExecute()
        {
            var dialogResult = MessageBox.Show("Are you sure you want to log out?", "Log out", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                var loginViewModel = new LoginViewModel(window);
                WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
