using Cosmetify.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Cosmetify.Model
{
    public class BatchModel : INotifyPropertyChanged
    {
        private double remainingWater = 100.0;

        public int Id { get; set; }

        public string BatchOrderNo { get; set; }

        public string OrderId { get; set; }

        public CustomerModel Customer { get; set; }

        public string ProductName { get; set; }

        public DateTime BatchDate { get; set; }

        public DateTime Expiry { get; set; }

        public ObservableCollection<BatchOrderModel> BatchOrderCollection { get; set; } = new ObservableCollection<BatchOrderModel>();

        public string PkgType { get; set; }

        public string PkgOrderQuantity { get; set;}

        public string Description { get; set; }

        public DateTime PlanningDate { get; set; }

        public DateTime PlannedDate { get; set; }

        public DateTime MfgDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public string Remarks { get; set; }

        public string AdditionalInfo { get; set; }

        public BatchStatus Status { get; set; }

        public OrderStageModel OrderStage { get; set; }

        public string Colour { get; set; }

        public string Perfume { get; set; }

        public ObservableCollection<string> Claims { get; set; }

        public BitmapImage PackagingTypeImage { get; set; }

        public string BrandName { get; set; }

        public string ProductID { get; set; }

        public double RemainingWater
        {
            get => this.remainingWater; set
            {
                this.remainingWater = value;
                this.NotifyPropertyChanged(nameof(RemainingWater));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
