using Cosmetify.Model.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    public class ActivesModel : INotifyPropertyChanged
    {
        private double remainingStock = 0;
        private double totalRequired = 0;
        private double totalBatchOrders = 0;
        private double diffStock;

        public int Id { get; set; }

        public string ActivesName { get; set; }

        public string ShortCode { get; set; }

        public double Stocks { get; set; }

        public ProductUnits Units { get; set; }

        public CategoryModel Category { get; set; }

        public SubCategoryModel SubCategory { get; set; }

        public SubSubCategoryModel SubSubCategory { get; set; }

        public double SKU { get; set; }

        public string BrandNames { get; set; }

        public double TotalBatchOrders
        {
            get => this.totalBatchOrders; set
            {
                this.totalBatchOrders = value;
                this.NotifyPropertyChanged(nameof(TotalBatchOrders));
            }
        }
        public double TotalRequired
        {
            get => this.totalRequired; 
            set
            {
                this.totalRequired = value;
                this.NotifyPropertyChanged(nameof(TotalRequired));
                this.RemainingStock = this.Stocks - value;
            }
        }

        public double RemainingStock
        {
            get => this.remainingStock; 
            set
            {
                this.remainingStock = value;
                this.NotifyPropertyChanged(nameof(RemainingStock));
                this.DiffStock = this.Stocks;
            }
        }

        public double DiffStock
        {
            get => this.diffStock; set
            {
                this.diffStock = value;
                this.NotifyPropertyChanged(nameof(DiffStock));
            }
        }

        public double TotalCreated { get; set; }

        public double TotalCreatedRequired { get; set; }

        public double TotalHold { get; set; }

        public double TotalHoldRequired { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
