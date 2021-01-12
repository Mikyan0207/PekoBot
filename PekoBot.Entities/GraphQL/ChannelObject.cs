using Newtonsoft.Json;

namespace PekoBot.Entities.GraphQL
{
	public class ChannelObject
	{
		[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Name { get; set; }

		[JsonProperty("en_name", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string EnglishName { get; set; }

		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Id { get; set; }

		[JsonProperty("image", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Image { get; set; }

		[JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Platform { get; set; }

		[JsonProperty("group", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Group { get; set; }

		[JsonProperty("statistics", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public StatisticsObject Statistics { get; set; }
	}
}