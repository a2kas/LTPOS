using Newtonsoft.Json;

namespace POS_display.Models.Recipe.Classifiers
{
    public class RouteNameClassifier
    {
        [JsonProperty("LANGUAGE")]
        public string Language { get; set; }

        [JsonProperty("NAME")]
        public string Name { get; set; }

        [JsonProperty("SHORTNAME")]
        public string ShortName { get; set; }
    }
}
