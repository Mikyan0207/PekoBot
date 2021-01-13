using Newtonsoft.Json;

namespace PekoBot.Entities.Json
{
	public class Emoji
	{
		[JsonProperty("Emoji")]
		public string Code { get; set; }

		[JsonProperty("EnglishName")]
		public string Member { get; set; }
	}
}