using System;

namespace PekoBot.Entities.Models
{
	public class Account
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }

		public string AccountId { get; set; }

		public string Image { get; set; }

		public Platform Platform { get; set; }

		public Statistics Statistics { get; set; }
	}
}