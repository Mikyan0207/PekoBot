using System.Linq;
using Microsoft.EntityFrameworkCore;
using PekoBot.Database;
using PekoBot.Entities;
using PekoBot.Entities.Models;

namespace PekoBot.Core.Services.Impl
{
	public class DbService : IService
	{
		public DbService()
		{
			using var context = new PekoBotContext();
			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
				context.SaveChanges();
			}

			context.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL; PRAGMA synchronous=OFF");
			context.SaveChanges();
			Initialize(context);
			context.SaveChanges();
		}

		public PekoBotContext GetContext()
		{
			var context = new PekoBotContext();
			context.Database.SetCommandTimeout(60);

			var conn = context.Database.GetDbConnection();
			conn.Open();

			using var com = conn.CreateCommand();
			com.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=OFF";
			com.ExecuteNonQuery();

			return context;
		}

		private static void Initialize(PekoBotContext context)
		{
			if (context.Members.FirstOrDefault(x => x.Name == "戌神ころね") == null)
			{
				context.Members.Add(new Member
				{
					Name = "戌神ころね",
					YoutubeId = "UChAnqc_AY5_I3Px5dig3X1Q",
					YoutubeName = "Korone Ch. 戌神ころね",
					YoutubeUrl = "https://www.youtube.com/channel/UChAnqc_AY5_I3Px5dig3X1Q",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnimjdERaJDGopfH8UaB0r9tr_p8uyuEWWyYVkAd5Q=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "兎田ぺこら") == null)
			{
				context.Members.Add(new Member
				{
					Name = "兎田ぺこら",
					YoutubeId = "UC1DCedRgGHBdm81E1llLhOQ",
					YoutubeName = "Pekora Ch. 兎田ぺこら",
					YoutubeUrl = "https://www.youtube.com/channel/UC1DCedRgGHBdm81E1llLhOQ",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnjvkyPGzOmEXZ34mEFPlwMKTbCDl1ZkQ_HkxY-O5Q=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "夜空メル") == null)
			{
				context.Members.Add(new Member
				{
					Name = "夜空メル",
					YoutubeId = "UCD8HOxPs4Xvsm8H0ZxXGiBw",
					YoutubeName = "Mel Channel 夜空メルチャンネル",
					YoutubeUrl = "https://www.youtube.com/channel/UCD8HOxPs4Xvsm8H0ZxXGiBw",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnj5J44v4_Ua9qGrh_n8IR0Dp-r2yj0Nl0mv4lkR=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "白上フブキ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "白上フブキ",
					YoutubeId = "UCdn5BQ06XqgXoAxIhbqw5Rg",
					YoutubeName = "フブキCh。白上フブキ",
					YoutubeUrl = "https://www.youtube.com/channel/UCdn5BQ06XqgXoAxIhbqw5Rg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnjfV6gSQIXQpGlFI21BO1EaRuwM_tnkH65IBruhxA=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "夏色まつり") == null)
			{
				context.Members.Add(new Member
				{
					Name = "夏色まつり",
					YoutubeId = "UCQ0UDLQCjY0rmuxCDE38FGg",
					YoutubeName = "Matsuri Channel 夏色まつり",
					YoutubeUrl = "https://www.youtube.com/channel/UCQ0UDLQCjY0rmuxCDE38FGg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwni8cjtyc08E7rocvO9_gR1b5BhO1O6O1VreDxMW=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "アキ・ローゼンタール") == null)
			{
				context.Members.Add(new Member
				{
					Name = "アキ・ローゼンタール",
					YoutubeId = "UCFTLzh12_nrtzqBPsTCqenA",
					YoutubeName = "アキロゼCh。Vtuber/ホロライブ所属",
					YoutubeUrl = "https://www.youtube.com/channel/UCFTLzh12_nrtzqBPsTCqenA",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnhPGqWt_E_8rBXpKUTgHCuyTq0Zz7TXlkRtiIww=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "赤井はあと") == null)
			{
				context.Members.Add(new Member
				{
					Name = "赤井はあと",
					YoutubeId = "UC1CfXB_kRs3C-zaeTG3oGyg",
					YoutubeName = "Haachama Ch. 赤井はあと",
					YoutubeUrl = "https://www.youtube.com/channel/UC1CfXB_kRs3C-zaeTG3oGyg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwniCTIXC8xYREeCKIkyKCrxI3mZmVlpKuLB-H1NFjg=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "湊あくあ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "湊あくあ",
					YoutubeId = "UC1opHUrw8rvnsadT-iGp7Cg",
					YoutubeName = "Aqua Ch. 湊あくあ",
					YoutubeUrl = "https://www.youtube.com/channel/UC1opHUrw8rvnsadT-iGp7Cg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwngM9Jmc29dvbOY43w7RWFbOZLU4tGtOkEwtt-g7PA=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "紫咲シオン") == null)
			{
				context.Members.Add(new Member
				{
					Name = "紫咲シオン",
					YoutubeId = "UCXTpFs_3PqI41qX2d9tL2Rw",
					YoutubeName = "Shion Ch. 紫咲シオン",
					YoutubeUrl = "https://www.youtube.com/channel/UCXTpFs_3PqI41qX2d9tL2Rw",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwni5CHNTkw8RlOj-78oqDbWS6sEdH4VQR_c06B6s6Q=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "百鬼あやめ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "百鬼あやめ",
					YoutubeId = "UC7fk0CB07ly8oSl0aqKkqFg",
					YoutubeName = "Nakiri Ayame Ch. 百鬼あやめ",
					YoutubeUrl = "https://www.youtube.com/channel/UC7fk0CB07ly8oSl0aqKkqFg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnh66ZORNcVma4Pn-Qic23kU3Kl4ZkHM3asCWjDh=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "癒月ちょこ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "癒月ちょこ",
					YoutubeId = "UC1suqwovbL1kzsoaZgFZLKg",
					YoutubeName = "Choco Ch. 癒月ちょこ",
					YoutubeUrl = "https://www.youtube.com/channel/UC1suqwovbL1kzsoaZgFZLKg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnh0dIB3uStuGrUZGGrubOATPiVSFDiweFCK-0cr=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "大空スバル") == null)
			{
				context.Members.Add(new Member
				{
					Name = "大空スバル",
					YoutubeId = "UCvzGlP9oQwU--Y0r9id_jnA",
					YoutubeName = "Subaru Ch. 大空スバル",
					YoutubeUrl = "https://www.youtube.com/channel/UCvzGlP9oQwU--Y0r9id_jnA",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwniCgko15I_x5bYWm0G2vnf5hZqD5hLOtLEDw0Na=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "大神ミオ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "大神ミオ",
					YoutubeId = "UCp-5t9SrOQwXMU7iIjQfARg",
					YoutubeName = "Mio Channel 大神ミオ",
					YoutubeUrl = "https://www.youtube.com/channel/UCp-5t9SrOQwXMU7iIjQfARg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwng30Wmfu39r1PLrF05pvAafn2a2Ex90ok2C6CTBmg=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "猫又おかゆ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "猫又おかゆ",
					YoutubeId = "UCvaTdHTWBGv3MKj3KVqJVCw",
					YoutubeName = "Okayu Ch. 猫又おかゆ",
					YoutubeUrl = "https://www.youtube.com/channel/UCvaTdHTWBGv3MKj3KVqJVCw",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnjXQ3Gt3t3SdUMZHBhhEb_c1jqThHfDaVNJF_LJ=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "猫又おかゆ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "潤羽るしあ",
					YoutubeId = "UCl_gCybOJRIgOXw6Qb4qJzQ",
					YoutubeName = "Rushia Ch. 潤羽るしあ",
					YoutubeUrl = "https://www.youtube.com/channel/UCl_gCybOJRIgOXw6Qb4qJzQ",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwngKVHYXNDzaEG9KIXm9lK0nBxHkA-NxlE88dLtl=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "宝鐘マリン") == null)
			{
				context.Members.Add(new Member
				{
					Name = "宝鐘マリン",
					YoutubeId = "UCCzUftO8KOVkV4wQG1vkUvg",
					YoutubeName = "Marine Ch. 宝鐘マリン",
					YoutubeUrl = "https://www.youtube.com/channel/UCCzUftO8KOVkV4wQG1vkUvg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnjPuFWs42Vx2yIhK7z1w4L-e1GIpHn_5R1uknbS=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Channels.FirstOrDefault(x => x.ChannelId == 797092264030896138) == null)
			{
				context.Channels.Add(new DiscordChannel()
				{
					ChannelId = 797092264030896138,
					ChannelName = "lives",
					ChannelType = ChannelType.HololiveNotifications
				});

				context.SaveChanges();
			}
		}
	}
}