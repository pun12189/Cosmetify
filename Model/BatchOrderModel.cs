using Cosmetify.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    public class BatchOrderModel : INotifyPropertyChanged
    {
        private double stocksRequired = 0.0;
        private double remainingStock = 0.0;
        private double percentageRequired = 1.0;
        private ActivesModel actives;
        private ProductUnits units = ProductUnits.kg;
        private double batchSize;

        public int Id { get; set; }

        public string MFName { get; set; }

        public double BatchSize
        {
            get => batchSize; 
            set
            {
                batchSize = value;
                this.NotifyPropertyChanged(nameof(BatchSize));
                if (percentageRequired >= 0 && this.actives != null)
                {
                    this.StocksRequired = this.BatchSize * percentageRequired / 100;
                }
            }
        }

        public ProductUnits Units
        {
            get => units; set
            {
                units = value;
                this.NotifyPropertyChanged(nameof(Units));
            }
        }

        public ActivesModel Actives
        {
            get => actives; set
            {
                actives = value;
                this.NotifyPropertyChanged(nameof(Actives));
            }
        }        

        public double PercentageRequired
        {
            get => percentageRequired; set
            {
                percentageRequired = value;
                this.NotifyPropertyChanged(nameof(PercentageRequired));
                if (percentageRequired > 0 && this.actives != null)
                {
                    this.StocksRequired = this.BatchSize * percentageRequired / 100;
                }
            }
        }

        public double StocksRequired
        {
            get => stocksRequired; set
            {
                stocksRequired = value;
                this.NotifyPropertyChanged(nameof(StocksRequired));
                if (this.actives != null)
                {
                    this.RemainingStock = this.Actives.Stocks - this.StocksRequired;
                    // this.Actives.Stocks = this.RemainingStock;
                }
            }
        }

        public double RemainingStock
        {
            get => remainingStock; set
            {
                remainingStock = value;
                this.NotifyPropertyChanged(nameof(RemainingStock));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
