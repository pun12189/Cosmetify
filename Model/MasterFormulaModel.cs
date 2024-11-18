using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitaab.Model
{
    public class MasterFormulaModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public ObservableCollection<MasterProductModel> Requirements { get; set; }

        public double RemainingWater { get; set; } = 100;
    }
}
