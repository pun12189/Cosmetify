using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    public class MasterFormulaModel : INotifyPropertyChanged
    {
        private double remainingWater = 100;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public ObservableCollection<MasterProductModel> Requirements { get; set; }

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
