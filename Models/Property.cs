using Newtonsoft.Json;

namespace Gtpx.ModelSync.DataModel.Models
{
    public class Property
    {
        [JsonIgnore]
        public bool HasValue { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name}: \"{Value}\"";
        }
    }
}