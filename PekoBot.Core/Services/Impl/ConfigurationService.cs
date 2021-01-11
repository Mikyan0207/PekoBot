using System.IO;
using Newtonsoft.Json;
using PekoBot.Entities.Json;

namespace PekoBot.Core.Services.Impl
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