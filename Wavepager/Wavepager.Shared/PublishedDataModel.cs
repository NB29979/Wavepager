using System;
using Newtonsoft.Json;

namespace Wavepager.Shared
{
    public class PublishedDataModel
    {
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("ts")]
        public string Ts { get; set; }
        [JsonProperty("ispublic")]
        public Boolean IsPublic { get; set; }
        
        public PublishedDataModel(string data, string ts, Boolean isPublic)
        {
            Data = data;
            Ts = ts;
            IsPublic = isPublic;
        }
    }
}
