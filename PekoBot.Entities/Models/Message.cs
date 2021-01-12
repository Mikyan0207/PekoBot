namespace PekoBot.Entities.Models
{
	public class Message
	{
		public string Id { get; set; }

		public ulong MessageId { get; set; }

		public string Content { get; set; }

		public User Author { get; set; }

		public Channel Channel { get; set; }
	}
}