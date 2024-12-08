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
    /// Interaction logic for AddOrderStages.xaml
    /// </summary>
    public partial class AddOrderStages : Window
    {
        // Using a DependencyProperty as the backing store for OrderStages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderStagesProperty =
            DependencyProperty.Register("OrderStages", typeof(string), typeof(AddOrderStages), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for DepartmentList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderStagesListProperty =
            DependencyProperty.Register("OrderStagesList", typeof(ObservableCollection<OrderStageModel>), typeof(AddOrderStages), new PropertyMetadata(new ObservableCollection<OrderStageModel>()));


        public AddOrderStages()
        {
            InitializeComponent();
        }

        public string OrderStages
        {
            get { return (string)GetValue(OrderStagesProperty); }
            set { SetValue(OrderStagesProperty, value); }
        }

        public ObservableCollection<OrderStageModel> OrderStagesList
        {
            get { return (ObservableCollection<OrderStageModel>)GetValue(OrderStagesListProperty); }
            set { SetValue(OrderStagesListProperty, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.OrderStages))
            {
                MessageBox.Show("Order Stage Name is empty.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (this.OrderStages.StartsWith(',') || this.OrderStages.EndsWith(','))
            {
                MessageBox.Show("Order Stage Name cannot starts or ends with comma.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var stages = this.OrderStages.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                foreach (var stage in stages)
                {
                    if (string.IsNullOrEmpty(stage)) continue;
                    this.OrderStagesList.Add(new OrderStageModel(stage));
                }

                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
