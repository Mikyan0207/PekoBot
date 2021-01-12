using Newtonsoft.Json;

namespace PekoBot.Entities.GraphQL
{
	public class VTuberObject
	{
		[JsonProperty("live", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public VTuberLivesObject VTuberLives { get; set; }

		[JsonProperty("upcoming", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public VTuberLivesObject VTuberUpcomingLives { get; set; }

		[JsonProperty("pageInfo", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public PageInfoObject PageInfo { get; set; }
	}
}