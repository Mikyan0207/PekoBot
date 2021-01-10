using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PekoBot.Entities;

namespace PekoBot.Core.Modules.VTubers.Common
{
	public class HololiveLives
	{
		[JsonProperty("lives")]
		public List<HololiveLive> Lives { get; set; }

		[JsonProperty("total")]
		public int TotalLives { get; set; }
	}
}