using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PekoBot.Entities.Enums;

namespace PekoBot.Entities.Models
{
	public class Channel
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public ulong ChannelId { get; set; }

		public string ChannelName { get; set; }

		public string ChannelType { get; set; }

		public List<Message> Messages { get; set; }

		public Guild Guild { get; set; }
	}
}