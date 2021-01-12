using Newtonsoft.Json;

namespace PekoBot.Entities.GraphQL
{
	public class ResponseObject
	{
		[JsonProperty("vtuber")]
		public VTuberObject VTuber { get; set; }
	}
}