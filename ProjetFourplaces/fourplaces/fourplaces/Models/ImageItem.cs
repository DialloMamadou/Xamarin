using Newtonsoft.Json;

namespace fourplaces.Models
{
    public class ImageItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }

        public ImageItem(int id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}