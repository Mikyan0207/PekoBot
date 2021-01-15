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

		public string[] Nicknames { get; set; }

		public DateTime DebutDate { get; set; }

		public Company Company { get; set; }

		public string Generation { get; set; }

		public List<Account> Accounts { get; set; }

		public Emoji Emoji { get; set; }
	}
}