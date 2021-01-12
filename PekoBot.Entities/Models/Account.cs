namespace PekoBot.Entities.Models
{
	public class Account
	{
		public string Id { get; set; }

		public string AccountId { get; set; }

		public string Name { get; set; }

		public string EnglishName { get; set; }

		public string Image { get; set; }

		public Platform Platform { get; set; }

		public VTuber VTuber { get; set; }
	}
}