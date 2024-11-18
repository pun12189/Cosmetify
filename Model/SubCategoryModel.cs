using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    [JsonSerializable(typeof(SubCategoryModel))]
    public class SubCategoryModel
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public int ParentCategoryId { get; set; }

        public ObservableCollection<SubSubCategoryModel> SubSubCategories { get; set; } = new ObservableCollection<SubSubCategoryModel>();
    }
}
