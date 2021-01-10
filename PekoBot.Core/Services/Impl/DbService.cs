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

			if (context.Members.FirstOrDefault(x => x.Name == "不知火フレア") == null)
			{
				context.Members.Add(new Member
				{
					Name = "不知火フレア",
					YoutubeId = "UCvInZx9h3jC2JzsIzoOebWg",
					YoutubeName = "Flare Ch. 不知火フレア",
					YoutubeUrl = "https://www.youtube.com/channel/UCvInZx9h3jC2JzsIzoOebWg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwngtkUgGkgWTz57Er3GSzuMUR07HISM_yDhKQFnR_A=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "白銀ノエル") == null)
			{
				context.Members.Add(new Member
				{
					Name = "白銀ノエル",
					YoutubeId = "UCdyqAaZDKHXg4Ahi7VENThQ",
					YoutubeName = "Noel Ch. 白銀ノエル",
					YoutubeUrl = "https://www.youtube.com/channel/UCdyqAaZDKHXg4Ahi7VENThQ",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnijLF2X1YBVQo3rClt7ub29cYM7OzpmRmliaGbw=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "天音かなた") == null)
			{
				context.Members.Add(new Member
				{
					Name = "天音かなた",
					YoutubeId = "UCZlDXzGoo7d44bwdNObFacg",
					YoutubeName = "Kanata Ch. 天音かなた",
					YoutubeUrl = "https://www.youtube.com/channel/UCZlDXzGoo7d44bwdNObFacg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwniXUgLD1FepLsMoqO7HnhlgwbxGmPeqKWGv1JsO=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "桐生ココ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "桐生ココ",
					YoutubeId = "UCS9uQI-jC3DE0L4IpXyvr6w",
					YoutubeName = "Coco Ch. 桐生ココ",
					YoutubeUrl = "https://www.youtube.com/channel/UCS9uQI-jC3DE0L4IpXyvr6w",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwniPku-5QatVce_BIjIeBxT5rj9lrTlCpXCvEWa7=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "角巻わため") == null)
			{
				context.Members.Add(new Member
				{
					Name = "角巻わため",
					YoutubeId = "UCqm3BQLlJfvkTsX_hvm0UmA",
					YoutubeName = "Watame Ch. 角巻わため",
					YoutubeUrl = "https://www.youtube.com/channel/UCqm3BQLlJfvkTsX_hvm0UmA",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnitWcmmZK60TDG8y5aUeQfZlmH9YlBNJ4D1ZSFI=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "常闇トワ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "常闇トワ",
					YoutubeId = "UC1uv2Oq6kNxgATlCiez59hw",
					YoutubeName = "Towa Ch. 常闇トワ",
					YoutubeUrl = "https://www.youtube.com/channel/UC1uv2Oq6kNxgATlCiez59hw",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnjPaiimZva5GECAyNDn0qraqPm62LlH-0oN21I5=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "姫森ルーナ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "姫森ルーナ",
					YoutubeId = "UCa9Y57gfeY0Zro_noHRVrnw",
					YoutubeName = "Luna Ch. 姫森ルーナ",
					YoutubeUrl = "https://www.youtube.com/channel/UCa9Y57gfeY0Zro_noHRVrnw",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnh8JcbM8f3Zvl9eszRVnuqrAF3bjQIEjuOPdBR2=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "雪花ラミィ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "雪花ラミィ",
					YoutubeId = "UCFKOVgVbGmX65RxO3EtH3iw",
					YoutubeName = "Lamy Ch. 雪花ラミィ",
					YoutubeUrl = "https://www.youtube.com/channel/UCFKOVgVbGmX65RxO3EtH3iw",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwng8eQJdCX3r4RgCmRGwigXkDp9a2JJSPq-dZcMF=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "桃鈴ねね") == null)
			{
				context.Members.Add(new Member
				{
					Name = "桃鈴ねね",
					YoutubeId = "UCAWSyEs_Io8MtpY3m-zqILA",
					YoutubeName = "Nene Ch.桃鈴ねね",
					YoutubeUrl = "https://www.youtube.com/channel/UCAWSyEs_Io8MtpY3m-zqILA",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnim3rUS3EhZrGARcbATumWxnrCAjo8ovmgXT2tm=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "獅白ぼたん") == null)
			{
				context.Members.Add(new Member
				{
					Name = "獅白ぼたん",
					YoutubeId = "UCUKD-uaobj9jiqB-VXt71mA",
					YoutubeName = "Botan Ch.獅白ぼたん",
					YoutubeUrl = "https://www.youtube.com/channel/UCUKD-uaobj9jiqB-VXt71mA",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwngRC-JqguPnj9ljVH3UulyfdlyQzLYzLeSrhvD6=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "尾丸ポルカ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "尾丸ポルカ",
					YoutubeId = "UCK9V2B22uJYu3N7eR_BT9QA",
					YoutubeName = "Polka Ch. 尾丸ポルカ",
					YoutubeUrl = "https://www.youtube.com/channel/UCK9V2B22uJYu3N7eR_BT9QA",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwni13e9pDow2afsp2f5CBPehEvYhFApFZoHaJWLu=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "Ayunda Risu") == null)
			{
				context.Members.Add(new Member
				{
					Name = "Ayunda Risu",
					YoutubeId = "UCOyYb1c43VlX9rc_lT6NKQw",
					YoutubeName = "Ayunda Risu Ch. hololive-ID",
					YoutubeUrl = "https://www.youtube.com/channel/UCOyYb1c43VlX9rc_lT6NKQw",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwniS_ftaXhqYt1SQPjyuFx5MD_-v0fIUkbCTq5Q_gg=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "Moona Hoshinova") == null)
			{
				context.Members.Add(new Member
				{
					Name = "Moona Hoshinova",
					YoutubeId = "UCP0BspO_AMEe3aQqqpo89Dg",
					YoutubeName = "Moona Hoshinova hololive-ID",
					YoutubeUrl = "https://www.youtube.com/channel/UCP0BspO_AMEe3aQqqpo89Dg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnj-XzL4TRZCzwo-7FCmdsTikQo9wMZwz9RGNoT0XA=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "Airani Iofifteen") == null)
			{
				context.Members.Add(new Member
				{
					Name = "Airani Iofifteen",
					YoutubeId = "UCAoy6rzhSf4ydcYjJw3WoVg",
					YoutubeName = "Airani Iofifteen Channel hololive-ID",
					YoutubeUrl = "https://www.youtube.com/channel/UCAoy6rzhSf4ydcYjJw3WoVg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnh4NDjjjUAfPj8Aa4kjyQb4C85KzMWobSCaso-8=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "小鳥遊キアラ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "小鳥遊キアラ",
					YoutubeId = "UCHsx4Hqa-1ORjQTh9TYDhww",
					YoutubeName = "Takanashi Kiara Ch. hololive-EN",
					YoutubeUrl = "https://www.youtube.com/channel/UCHsx4Hqa-1ORjQTh9TYDhww",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnicJ682cy4GKD-ulxRnE2jOuihNWCULnqW2caej=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "森美声") == null)
			{
				context.Members.Add(new Member
				{
					Name = "森美声",
					YoutubeId = "UCL_qhgtOy0dy1Agp8vkySQg",
					YoutubeName = "Mori Calliope Ch. hololive-EN",
					YoutubeUrl = "https://www.youtube.com/channel/UCL_qhgtOy0dy1Agp8vkySQg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwngcfBYiWbxJXhxC2rtEk-2Uj_BrlQ0FNBdZblJn=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "一伊那尓栖") == null)
			{
				context.Members.Add(new Member
				{
					Name = "一伊那尓栖",
					YoutubeId = "UCMwGHR0BTZuLsmjY_NT5Pwg",
					YoutubeName = "Ninomae Ina'nis Ch. hololive-EN",
					YoutubeUrl = "https://www.youtube.com/channel/UCMwGHR0BTZuLsmjY_NT5Pwg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwng37V0l-NwF3bu7QA4XmOP5EZFwk5zJE-78OHP9=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "がうる・ぐら") == null)
			{
				context.Members.Add(new Member
				{
					Name = "がうる・ぐら",
					YoutubeId = "UCoSrY_IQQVpmIRZ9Xf-y93g",
					YoutubeName = "Gawr Gura Ch. hololive-EN",
					YoutubeUrl = "https://www.youtube.com/channel/UCoSrY_IQQVpmIRZ9Xf-y93g",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnhSSaF3Q-PyyTSis4EH6Cu8FZ32LNvkxI9Gl_rn=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "ワトソン・アメリア") == null)
			{
				context.Members.Add(new Member
				{
					Name = "ワトソン・アメリア",
					YoutubeId = "UCyl1z3jo3XHR1riLFKG5UAg",
					YoutubeName = "Watson Amelia Ch. hololive-EN",
					YoutubeUrl = "https://www.youtube.com/channel/UCyl1z3jo3XHR1riLFKG5UAg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwngENhxNHPzV4jjip28G3vswMxutvkaBhBjAMS0i=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "クレイジー・オリー") == null)
			{
				context.Members.Add(new Member
				{
					Name = "クレイジー・オリー",
					YoutubeId = "UCYz_5n-uDuChHtLo7My1HnQ",
					YoutubeName = "Kureiji Ollie Ch. hololive-ID",
					YoutubeUrl = "https://www.youtube.com/channel/UCYz_5n-uDuChHtLo7My1HnQ",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnitFviHjDNyLPjA95sz4Supbhvm8Ea3dKrRAKk=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "アーニャ・メルフィッサ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "アーニャ・メルフィッサ",
					YoutubeId = "UC727SQYUvx5pDDGQpTICNWg",
					YoutubeName = "Anya Melfissa Ch. hololive-ID",
					YoutubeUrl = "https://www.youtube.com/channel/UC727SQYUvx5pDDGQpTICNWg",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnie9uw8oBbjgTKmoLPSo9b_K0AbNPRbD2rjCApl=s88-c-k-c0x00ffffff-no-rj"
				});

				context.SaveChanges();
			}

			if (context.Members.FirstOrDefault(x => x.Name == "パヴォリア・レイネ") == null)
			{
				context.Members.Add(new Member
				{
					Name = "パヴォリア・レイネ",
					YoutubeId = "UChgTyjG-pdNvxxhdsXfHQ5Q",
					YoutubeName = "Pavolia Reine Ch. hololive-ID",
					YoutubeUrl = "https://www.youtube.com/channel/UChgTyjG-pdNvxxhdsXfHQ5Q",
					YoutubeAvatarUrl =
						"https://yt3.ggpht.com/ytc/AAUvwnjXvzYgBg5mJv8RZLZhgn0-Zb_4zzE1zhCHrJ2w=s88-c-k-c0x00ffffff-no-rj"
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