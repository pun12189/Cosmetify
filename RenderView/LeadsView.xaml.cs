using BahiKitaab.Dialogs;
using BahiKitaab.Helper;
using BahiKitaab.Model;
using BahiKitaab.RenderView.UserControls;
using BahiKitaab.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for LeadsView.xaml
    /// </summary>
    public partial class LeadsView : Page
    {        
        public LeadsView()
        {
            InitializeComponent();
            this.DataContext = HomepageViewModel.CommonViewModel;
        }

        private void ImportFile(object sender, RoutedEventArgs e)
        {
            /*var dialog = new ImportDialog();
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.ResizeMode = ResizeMode.NoResize;
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var fp = dialog.FilePath;
                using (var reader = new StreamReader(fp.FullName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<dynamic>();
                    this.dataGrid1.ItemsSource = records;
                    this.dataGrid1.DataContext = records;
                }
            }*/
        }
    }
}
