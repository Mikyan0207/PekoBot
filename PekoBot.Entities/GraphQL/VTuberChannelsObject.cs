using System.Collections.Generic;
using Newtonsoft.Json;

namespace PekoBot.Entities.GraphQL
{
	public class VTuberChannelsObject
	{
		[JsonProperty("items", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public List<ChannelObject> Channels { get; set; }

		[JsonProperty("pageInfo", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public PageInfoObject PageInfo { get; set; }
	}
}