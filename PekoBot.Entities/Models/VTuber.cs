using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

		public string Platform { get; set; }

		public Statistics Statistics { get; set; }

		public Company Company { get; set; }

		[ForeignKey("EmojiId")]
		public Emoji Emoji { get; set; }

		public string EmojiId { get; set; }

		public List<Role> Roles { get; set; }
	}
}