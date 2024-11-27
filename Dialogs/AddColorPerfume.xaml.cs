using Cosmetify.Model;
using Cosmetify.ViewModel;
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
    /// Interaction logic for AddColorPerfume.xaml
    /// </summary>
    public partial class AddColorPerfume : Window
    {
        public bool IsColor { get; set; }

        public AddColorPerfume(bool isColor)
        {
            InitializeComponent();
            this.IsColor = isColor;
            if (isColor)
            {
                this.tbTitle.Text = "Add Color";
            }
            else
            {
                this.tbTitle.Text = "Add Perfume";
            }
        }

        private void SaveCustomer(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbBNames.Text))
            {
                MessageBox.Show("Please enter some values", "Alert", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }            

            if (this.IsColor)
            {
                if (!string.IsNullOrEmpty(this.tbBNames.Text))
                {
                    var str = this.tbBNames.Text.Split(',');
                    foreach (var b in str)
                    {
                        HomepageViewModel.CommonViewModel.ColoursRepository.InsertColor(b);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.tbBNames.Text))
                {
                    var str = this.tbBNames.Text.Split(',');
                    foreach (var b in str)
                    {
                        HomepageViewModel.CommonViewModel.PerfumeRepository.InsertPerfume(b);
                    }
                }
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
