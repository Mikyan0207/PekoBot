using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class Company
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }

		public string EnglishName { get; set; }

		public string Code { get; set; }

		public string Image { get; set; }

		public IEnumerable<VTuber> VTubers { get; set; }
	}
}