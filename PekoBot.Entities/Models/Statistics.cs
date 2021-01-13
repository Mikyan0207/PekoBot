using System;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class Statistics
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public int SubscriberCount { get; set; }

		public int ViewCount { get; set; }

		public int VideoCount { get; set; }
	}
}