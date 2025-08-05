using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    public class ApiClassModel
    {
        [JsonProperty("systemid")]
        public string SystemId { get; set; }

        [JsonProperty("softwareid")]
        public string SoftwareId { get; set; }

        [JsonProperty("note")]
        public string Notice { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
