using Cosmetify.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    public class BatchModel
    {
        public int Id { get; set; }

        public string BatchOrderNo { get; set; }

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
    }
}
