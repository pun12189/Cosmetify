using Cosmetify.Model.Enums;
using System.Windows.Media.Imaging;

namespace Cosmetify.Model
{
    public class ProductModel
    {
        private long remainingStock = 0;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CategoryModel Category { get; set; }

        public ProductStatus Status { get; set; }

        public string Packing { get; set; }

        public string BatchNo { get; set; }

        public DateTime Expiry { get; set; }

        public DateTime MfgDate { get; set; }

        public Int64 Stock { get; set; } = 0;

        public double PurchasePrice { get; set; }

        public double MRP { get; set; }

        public double MaxSellingPrice { get; set; } 

        public double MinSellingPrice { get; set; }

        public double Rate { get; set; }

        public string MfrName { get; set; }

        public string Company { get; set; }

        public BitmapImage ProductImage { get; set; }

        public ProductUnits ProductUnit { get; set; }

        public double UnitValue { get; set; }

        public Int64 RequiredStock { get; set; } = 0;

        public Int64 RemainingStock
        {
            get 
            { 
                return this.remainingStock; 
            }
            set
            {
                value = this.Stock - this.RequiredStock;
                this.remainingStock = value;
            }
        }
    }
}
