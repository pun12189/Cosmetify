using BahiKitaab.Model;
using BahiKitaab.ViewModel;
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

namespace BahiKitaab.RenderView
{   
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        // Using a DependencyProperty as the backing store for OrdersCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrdersCollectionProperty =
            DependencyProperty.Register("OrdersCollection", typeof(ObservableCollection<OrderModel>), typeof(OrdersPage), new PropertyMetadata(new ObservableCollection<OrderModel>()));

        public OrdersPage()
        {
            InitializeComponent();
            this.DataContext = HomepageViewModel.CommonViewModel;
            this.OrdersCollection = HomepageViewModel.CommonViewModel.OrderRepository.GetAllOrders();
        }

        public ObservableCollection<OrderModel> OrdersCollection
        {
            get { return (ObservableCollection<OrderModel>)GetValue(OrdersCollectionProperty); }
            set { SetValue(OrdersCollectionProperty, value); }
        }
    }
}
