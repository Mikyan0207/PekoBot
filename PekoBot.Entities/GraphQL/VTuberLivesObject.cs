using System.Collections.Generic;
using Newtonsoft.Json;

namespace PekoBot.Entities.GraphQL
{
	public class VTuberLivesObject
	{
		[JsonProperty("items", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public List<LiveObject> Lives { get; set; }
	}
}