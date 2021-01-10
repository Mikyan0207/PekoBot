using System.Threading.Tasks;

namespace PekoBot.Core
{
	internal static class Program
	{
		private static async Task Main(string[] args)
		{
			await new PekoBot().RunAsync().ConfigureAwait(false);
		}
	}
}
