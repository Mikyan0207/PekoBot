using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace PekoBot.Entities.Models
{
	public class Member
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }

		public string EnglishName { get; set; }

		public string YoutubeId { get; set; }

		public string AvatarUrl { get; set; }

		public int SubscriberCount { get; set; }

		public Company Company { get; set; }

		public List<Role> Roles { get; set; }
	}
}