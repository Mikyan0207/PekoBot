using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PekoBot.Entities
{
	public class HololiveLive
	{
		public int Id { get; set; }

		public string Title { get; set; }

		[JsonProperty("start_at")]
		public DateTime StartAt { get; set; }

		public object Duration { get; set; }

		[JsonProperty("channel_id")]
		public int ChannelId { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		public string Cover { get; set; }

		public string Room { get; set; }

		public string Platform { get; set; }

		public string Channel { get; set; }

		public object Video { get; set; }
	}
}