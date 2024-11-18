using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    [JsonSerializable(typeof(SubSubCategoryModel))]
    public class SubSubCategoryModel
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public int ParentCategoryId { get; set; }
    }
}
