using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddDepartments.xaml
    /// </summary>
    public partial class AddDepartments : Window
    {
        // Using a DependencyProperty as the backing store for Department.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DepartmentProperty =
            DependencyProperty.Register("Department", typeof(string), typeof(AddDepartments), new PropertyMetadata(string.Empty));

        public AddDepartments()
        {
            InitializeComponent();
        }

        public string Department
        {
            get { return (string)this.GetValue(DepartmentProperty); }
            set { this.SetValue(DepartmentProperty, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Department))
            {
                MessageBox.Show("Department Name is empty.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
