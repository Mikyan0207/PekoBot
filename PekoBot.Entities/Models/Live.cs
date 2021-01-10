using System;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class Live
	{
		[Key] public int LiveId { get; set; }

		public string Title { get; set; }

		public DateTime StartAt { get; set; }

		public DateTime CreatedAt { get; set; }

		public string Cover { get; set; }

		public string Room { get; set; }

		public Platform Platform { get; set; }

		public Member Member { get; set; }

		public bool Reminded { get; set; } = false;

		public bool Notified { get; set; } = false;

	}

	public enum Platform
	{
		Youtube,
		Other,
	}
}