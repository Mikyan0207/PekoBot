using Newtonsoft.Json;
using PekoBot.Core.Services.Interfaces;
using PekoBot.Entities.Json;
using System.IO;

namespace PekoBot.Core.Services
{
	public class ConfigurationService : IService
	{
		public PekoBotConfiguration Configuration { get; }

		public ConfigurationService()
		{
			var content = File.ReadAllText($"Resources/PekoBotConfiguration.json");

			Configuration = JsonConvert.DeserializeObject<PekoBotConfiguration>(content);
		}
	}
}