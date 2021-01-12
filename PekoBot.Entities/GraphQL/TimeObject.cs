using Newtonsoft.Json;
using System;

namespace PekoBot.Entities.GraphQL
{
	public class TimeObject
	{
		[JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public int StartTime { get; set; }

		[JsonProperty("endTime", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public int EndTime { get; set; }

		[JsonProperty("scheduledStartTime", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public int ScheduledStartTime { get; set; }

		[JsonProperty("publishedAt")]
		public DateTime PublishedAt { get; set; }

		[JsonProperty("lateBy", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public int LateBy { get; set; }

		[JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
		public int Duration { get; set; }
	}
}