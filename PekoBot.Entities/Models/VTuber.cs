using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class VTuber
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }

		public string EnglishName { get; set; }

		public string ChannelId { get; set; }

		public string AvatarUrl { get; set; }

		public Statistics Statistics { get; set; }

		public Company Company { get; set; }

		public Emoji Emoji { get; set; }

		public List<Role> Roles { get; set; }
	}
}