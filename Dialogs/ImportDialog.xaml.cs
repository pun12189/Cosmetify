using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace Cosmetify.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportDialog.xaml
    /// </summary>
    public partial class ImportDialog : Window
    {
        // Using a DependencyProperty as the backing store for FilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(FileInfo), typeof(ImportDialog));

        public ImportDialog()
        {
            InitializeComponent();
        }        

        public FileInfo FilePath
        {
            get { return (FileInfo)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        private void UploadExcelFile(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "xls Files (*.xls)|*.xls|xlsx Files (*.xlsx)|*.xlsx|csv Files (*.csv)|*.csv";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = System.IO.Path.GetFileName(dlg.FileName);
                fileNameTb.Text = filename;
                this.FilePath = new FileInfo(dlg.FileName);
            }
        }

        private void SubmitFile(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();            
        }
    }
}
