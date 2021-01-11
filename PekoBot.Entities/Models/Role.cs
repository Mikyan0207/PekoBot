using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PekoBot.Entities.Enums;

namespace PekoBot.Entities.Models
{
	public class Role
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public ulong RoleId { get; set; }

		public string Name { get; set; }

		public string Color { get; set; }

		public Member Member { get; set; }

		public RoleType RoleType { get; set; } = RoleType.None;
	}
}