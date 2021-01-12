using Newtonsoft.Json;

namespace PekoBot.Entities.GraphQL
{
	public class PageInfoObject
	{
		[JsonProperty("nextCursor", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public string NextCursor { get; set; }

		[JsonProperty("hasNextPage", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public bool HasNextPage { get; set; }
	}
}