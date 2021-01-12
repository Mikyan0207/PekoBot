using Newtonsoft.Json;

namespace PekoBot.Entities.GraphQL
{
	public class StatisticsObject
	{
		[JsonProperty("subscriberCount", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public int SubscriberCount { get; set; }

		[JsonProperty("viewCount", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public int ViewCount { get; set; }

		[JsonProperty("videoCount", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public int VideoCount { get; set; }
	}
}