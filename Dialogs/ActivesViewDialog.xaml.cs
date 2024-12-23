using Cosmetify.Model;
using Cosmetify.RenderView;
using Cosmetify.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Interaction logic for ActivesViewDialog.xaml
    /// </summary>
    public partial class ActivesViewDialog : Window
    {
        // Using a DependencyProperty as the backing store for ActivesModelsCollection.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivesModelsCollectionProperty =
            DependencyProperty.Register("ActivesModelsCollection", typeof(ObservableCollection<ActivesModel>), typeof(ActivesViewDialog), new PropertyMetadata(new ObservableCollection<ActivesModel>()));

        public ActivesViewDialog()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        public ObservableCollection<ActivesModel> ActivesModelsCollection
        {
            get { return (ObservableCollection<ActivesModel>)GetValue(ActivesModelsCollectionProperty); }
            set { SetValue(ActivesModelsCollectionProperty, value); }
        }
    }
}
