using System;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class Role
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public ulong RoleId { get; set; }

		public string Name { get; set; }

		public string Mention { get; set; }

		public string Color { get; set; }

		public VTuber VTuber { get; set; }

		public Guild Guild { get; set; }
	}
}