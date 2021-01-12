using System;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class Emoji
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string Code { get; set; }

		public Member Member { get; set; }
	}
}