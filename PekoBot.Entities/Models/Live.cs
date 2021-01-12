using System;
using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class Live
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string LiveId { get; set; }

		public string Title { get; set; }

		public string Thumbnail { get; set; }

		public Platform Platform { get; set; }

		public uint Viewers { get; set; }

		public bool IsPremiere { get; set; }

		public Status Status { get; set; }

		public DateTime ScheduledStartTime { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public DateTime CreatedAt { get; set; }

		public int LateBy { get; set; }

		public int Duration { get; set; }

		public VTuber VTuber { get; set; }

		public bool Notified { get; set; } = false;
	}

	public enum Platform
	{
		Youtube,
		Twitch,
		Bilibili,
		Twitcasting,
		Other
	}

	public enum Status
	{
		Upcoming,
		Live,
		Ended,
		Unknown
	}
}