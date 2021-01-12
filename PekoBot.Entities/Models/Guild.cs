using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class Guild
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public ulong GuildId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string IconUrl { get; set; }

		public DateTime JoinedAt { get; set; }

		public int MemberCount { get; set; }

		public List<Channel> Channels { get; set; }

		public List<Role> Roles { get; set; }

		public List<User> Users { get; set; }
	}
}