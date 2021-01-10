using System;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class Member
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }

		public string YoutubeId { get; set; }

		public string YoutubeName { get; set; }

		public string YoutubeUrl { get; set; }

		public string YoutubeAvatarUrl { get; set; }
	}
}