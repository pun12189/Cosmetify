using Cosmetify.Model;
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
    /// Interaction logic for MasterFormulaViewDialog.xaml
    /// </summary>
    public partial class MasterFormulaViewDialog : Window
    {
        // Using a DependencyProperty as the backing store for ActivesModelsCollection.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MFActivesModelsCollectionProperty =
            DependencyProperty.Register("MFActivesModelsCollection", typeof(ObservableCollection<MasterProductModel>), typeof(MasterFormulaViewDialog), new PropertyMetadata(new ObservableCollection<MasterProductModel>()));

        public MasterFormulaViewDialog()
        {
            InitializeComponent();
        }

        public ObservableCollection<MasterProductModel> MFActivesModelsCollection
        {
            get { return (ObservableCollection<MasterProductModel>)GetValue(MFActivesModelsCollectionProperty); }
            set { SetValue(MFActivesModelsCollectionProperty, value); }
        }
    }
}
