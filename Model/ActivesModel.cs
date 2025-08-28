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
        private string productNames;
        private int productNamesCount;
        private string brandNames;
        private int brandNamesCount;
        private double stocks;
        private string qtyReqd;
        private double totalCreated;
        private double totalCreatedRequired;
        private double totalHold;
        private double totalHoldRequired;

        public int Id { get; set; }

        public string ActivesName { get; set; }

        public string ShortCode { get; set; }

        public double Stocks
        {
            get => this.stocks; set
            {
                this.stocks = value;
                this.NotifyPropertyChanged(nameof(Stocks));
            }
        }

        public ProductUnits Units { get; set; }

        public CategoryModel Category { get; set; }

        public SubCategoryModel SubCategory { get; set; }

        public SubSubCategoryModel SubSubCategory { get; set; }

        public double SKU { get; set; }

        public string BrandNames
        {
            get => this.brandNames; set
            {
                this.brandNames = value;
                this.NotifyPropertyChanged(nameof(BrandNames));
            }
        }

        public int BrandNamesCount
        {
            get => this.brandNamesCount; set
            {
                this.brandNamesCount = value;
                this.NotifyPropertyChanged(nameof(BrandNamesCount));
            }
        }

        public string PkgTypes { get; set; }

        public string PkgQty { get; set; }

        public string BatchQty { get; set; }

        public string ProductNames
        {
            get => this.productNames; set
            {
                this.productNames = value;
                this.NotifyPropertyChanged(nameof(ProductNames));
            }
        }

        public int ProductNamesCount
        {
            get => this.productNamesCount; set
            {
                this.productNamesCount = value;
                this.NotifyPropertyChanged(nameof(ProductNamesCount));
            }
        }

        public string QtyReqd
        {
            get => this.qtyReqd; set
            {
                this.qtyReqd = value;
                this.NotifyPropertyChanged(nameof(QtyReqd));
            }
        }

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

        public double TotalCreated
        {
            get => this.totalCreated; set
            {
                this.totalCreated = value;
                this.NotifyPropertyChanged(nameof(TotalCreated));
            }
        }

        public double TotalCreatedRequired
        {
            get => this.totalCreatedRequired; set
            {
                this.totalCreatedRequired = value;
                this.NotifyPropertyChanged(nameof(TotalCreatedRequired));
            }
        }

        public double TotalHold
        {
            get => this.totalHold; set
            {
                this.totalHold = value;
                this.NotifyPropertyChanged(nameof(TotalHold));
            }
        }

        public double TotalHoldRequired
        {
            get => this.totalHoldRequired; set
            {
                this.totalHoldRequired = value;
                this.NotifyPropertyChanged(nameof(TotalHoldRequired));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
