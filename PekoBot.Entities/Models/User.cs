using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class User
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public ulong UserId { get; set; }

		public string Name { get; set; }

		public string Discriminator { get; set; }

		public string Mention { get; set; }

		public string AvatarUrl { get; set; }

		public Guild Guild { get; set; }

		public List<Role> Roles { get; set; }
	}
}