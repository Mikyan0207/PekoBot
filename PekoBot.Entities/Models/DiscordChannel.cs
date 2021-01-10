using System.ComponentModel.DataAnnotations;

namespace PekoBot.Entities.Models
{
	public class DiscordChannel
	{
		[Key]
		public ulong ChannelId { get; set; }

		public string ChannelName { get; set; }

		public ChannelType ChannelType { get; set; }
	}

	public enum ChannelType
	{
		HololiveNotifications,
		OtherNotifications
	}
}