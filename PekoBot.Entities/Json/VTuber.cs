using Newtonsoft.Json;
using System.Collections.Generic;

namespace PekoBot.Entities.Json
{
	public class Statistics
	{

		[JsonProperty("SubscriberCount")]
		public int SubscriberCount { get; set; }

		[JsonProperty("VideoCount")]
		public int VideoCount { get; set; }

		[JsonProperty("ViewCount")]
		public int ViewCount { get; set; }
	}

	public class Account
	{

		[JsonProperty("Platform")]
		public string Platform { get; set; }

		[JsonProperty("AccountId")]
		public string AccountId { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Image")]
		public string Image { get; set; }

		[JsonProperty("Statistics")]
		public Statistics Statistics { get; set; }
	}

	public class VTuber
	{

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("EnglishName")]
		public string EnglishName { get; set; }

		[JsonProperty("Nicknames")]
		public IList<string> Nicknames { get; set; }

		[JsonProperty("DebutDate")]
		public string DebutDate { get; set; }

		[JsonProperty("Company")]
		public string Company { get; set; }

		[JsonProperty("Generation")]
		public string Generation { get; set; }

		[JsonProperty("Accounts")]
		public IList<Account> Accounts { get; set; }

		[JsonProperty("Emoji")]
		public string Emoji { get; set; }
	}
}