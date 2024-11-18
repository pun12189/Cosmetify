using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    [JsonSerializable(typeof(CategoryModel))]
    public class CategoryModel
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public double GstRate { get; set; }

        public ObservableCollection<SubCategoryModel> SubCategories { get; set; } = new ObservableCollection<SubCategoryModel>();
    }
}
