using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BahiKitaab.Model
{
    public class OrderStageModel
    {
        public OrderStageModel(string name) 
        {
            this.StageColor = Helper.Helper.PickBrush();
            this.StageName = name;
        }

        public string StageName { get; set; }

        public Brush StageColor { get; set; }
    }
}
