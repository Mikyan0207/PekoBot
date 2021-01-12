using Newtonsoft.Json;

namespace PekoBot.Entities.GraphQL
{
	public class LiveObject
	{
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Id { get; set; }

		[JsonProperty("title", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Title { get; set; }

		[JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Thumbnail { get; set; }

		[JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Platform { get; set; }

		[JsonProperty("group", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Company { get; set; }

		[JsonProperty("viewers", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public uint Viewers { get; set; }

		[JsonProperty("is_premiere", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public bool IsPremiere { get; set; }

		[JsonProperty("status", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Status { get; set; }

		[JsonProperty("timeData", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public TimeObject Time { get; set; }

		[JsonProperty("channel", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public ChannelObject Channel { get; set; }
	}
}