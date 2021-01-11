using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PekoBot.Entities.Models
{
	public class Member
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }

		public string[] Nicknames { get; set; }

		public string YoutubeId { get; set; }

		public string AvatarUrl { get; set; }

		public Company Company { get; set; }

		[ForeignKey("RoleId")]
		public Role Role { get; set; }

		public string RoleId { get; set; }
	}
}