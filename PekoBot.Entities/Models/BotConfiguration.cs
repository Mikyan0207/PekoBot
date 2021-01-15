using System;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class BotConfiguration
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }

		public ulong BotId { get; set; }

		public string GuildMessage { get; set; }

		public string Prefix { get; set; }
	}
}