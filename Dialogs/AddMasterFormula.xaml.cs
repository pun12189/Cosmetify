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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Cosmetify.Dialogs
{
    /// <summary>
    /// Interaction logic for AddMasterFormula.xaml
    /// </summary>
    public partial class AddMasterFormula : Window
    {
        // Using a DependencyProperty as the backing store for RemainingWater.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemainingWaterProperty =
            DependencyProperty.Register("RemainingWater", typeof(double), typeof(AddMasterFormula), new PropertyMetadata(100.0));

        // Using a DependencyProperty as the backing store for FormulaCode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormulaCodeProperty =
            DependencyProperty.Register("FormulaCode", typeof(string), typeof(AddMasterFormula), new PropertyMetadata());

        // Using a DependencyProperty as the backing store for FormulaName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormulaNameProperty =
            DependencyProperty.Register("FormulaName", typeof(string), typeof(AddMasterFormula), new PropertyMetadata());

        // Using a DependencyProperty as the backing store for ActivesList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivesListProperty =
            DependencyProperty.Register("ActivesList", typeof(ObservableCollection<MasterProductModel>), typeof(AddMasterFormula), new PropertyMetadata(new ObservableCollection<MasterProductModel>()));

        public AddMasterFormula()
        {
            this.Loaded += AddMasterFormula_Loaded;
            InitializeComponent();
            this.ddActives.ItemsSource = HomepageViewModel.CommonViewModel.ActivesRepository.GetAllProducts();            
        }

        private void AddMasterFormula_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbName.Text = this.FormulaName;
            this.tbCode.Text = this.FormulaCode;
            this.tbWater.Text = this.RemainingWater.ToString();
        }

        public ObservableCollection<MasterProductModel> ActivesList
        {
            get { return (ObservableCollection<MasterProductModel>)GetValue(ActivesListProperty); }
            set { SetValue(ActivesListProperty, value); }
        }

        public string FormulaName
        {
            get { return (string)GetValue(FormulaNameProperty); }
            set { SetValue(FormulaNameProperty, value); }
        }

        public string FormulaCode
        {
            get { return (string)GetValue(FormulaCodeProperty); }
            set { SetValue(FormulaCodeProperty, value); }
        }

        public double RemainingWater
        {
            get { return (double)GetValue(RemainingWaterProperty); }
            set { SetValue(RemainingWaterProperty, value); }
        }        

        private void ActivesListAdd(object sender, RoutedEventArgs e)
        {
            var product = this.ddActives.SelectedItem as ActivesModel;
            if (product != null)
            {
                var model = new MasterProductModel();
                model.Id = product.Id;
                model.Name = product.ActivesName;
                model.Required = Convert.ToDouble(this.tbRem.Text);
                this.tbWater.Text = (Convert.ToDouble(this.tbWater.Text) - model.Required).ToString();
                this.ActivesList.Add(model);
                this.ddActives.SelectedIndex = 0;
                this.tbRem.Clear();
            }
        }

        private void dataGrid1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var total = 0.0;
            foreach (var model in this.ActivesList)
            {
                total += model.Required;
            }

            this.tbWater.Text = (100.0 - total).ToString();
        }

        private void btnAddFormula_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbName.Text))
            {
                MessageBox.Show("Name cannot be empty", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                this.FormulaName = this.tbName.Text;
            }

            if (string.IsNullOrEmpty(this.tbCode.Text))
            {
                MessageBox.Show("Code cannot be empty", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                this.FormulaCode = this.tbCode.Text;
            }

            if (this.ActivesList.Count <= 0)
            {
                MessageBox.Show("Please add some Actives", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.RemainingWater = Convert.ToDouble(this.tbWater.Text);
            this.tbName.Clear();
            this.tbCode.Clear();
            this.tbRem.Clear();
            this.tbWater.Clear();
            this.tbWater.Text = 100.ToString();
            this.Close();
        }
    }
}
