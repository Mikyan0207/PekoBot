using System.Collections.Generic;

namespace PekoBot.Entities.Json
{
	public class Member
	{
		public string Name { get; set; }

		public IEnumerable<string> Nicknames { get; set; }

		public string YouTubeId { get; set; }

		public string AvatarUrl { get; set; }

		public string Company { get; set; }

		public string Color { get; set; }
	}
}