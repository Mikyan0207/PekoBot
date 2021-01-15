﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PekoBot.Database;

namespace PekoBot.Database.Migrations
{
    [DbContext(typeof(PekoBotContext))]
    partial class PekoBotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("PekoBot.Entities.Models.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AccountId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Platform")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StatisticsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("VTuberId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StatisticsId");

                    b.HasIndex("VTuberId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Channel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChannelName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChannelType")
                        .HasColumnType("TEXT");

                    b.Property<string>("GuildId")
                        .HasColumnType("TEXT");

                    b.Property<string>("VTuberId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("VTuberId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Company", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("EnglishName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Emoji", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Emojis");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Guild", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IconUrl")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("MemberCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Live", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPremiere")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LateBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LiveId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Notified")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Platform")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ScheduledStartTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("VTuberId")
                        .HasColumnType("TEXT");

                    b.Property<uint>("Viewers")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VTuberId");

                    b.ToTable("Lives");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Message", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChannelId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("MessageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ChannelId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<string>("GuildId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Mention")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("VTuberId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.HasIndex("VTuberId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Statistics", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("SubscriberCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VideoCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ViewCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .HasColumnType("TEXT");

                    b.Property<string>("GuildId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Mention")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.VTuber", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("CompanyId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DebutDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmojiId")
                        .HasColumnType("TEXT");

                    b.Property<string>("EnglishName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Generation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nicknames")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("EmojiId");

                    b.ToTable("VTubers");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Account", b =>
                {
                    b.HasOne("PekoBot.Entities.Models.Statistics", "Statistics")
                        .WithMany()
                        .HasForeignKey("StatisticsId");

                    b.HasOne("PekoBot.Entities.Models.VTuber", null)
                        .WithMany("Accounts")
                        .HasForeignKey("VTuberId");

                    b.Navigation("Statistics");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Channel", b =>
                {
                    b.HasOne("PekoBot.Entities.Models.Guild", "Guild")
                        .WithMany("Channels")
                        .HasForeignKey("GuildId");

                    b.HasOne("PekoBot.Entities.Models.VTuber", null)
                        .WithMany("Channels")
                        .HasForeignKey("VTuberId");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Live", b =>
                {
                    b.HasOne("PekoBot.Entities.Models.VTuber", "VTuber")
                        .WithMany()
                        .HasForeignKey("VTuberId");

                    b.Navigation("VTuber");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Message", b =>
                {
                    b.HasOne("PekoBot.Entities.Models.User", "Author")
                        .WithMany("Messages")
                        .HasForeignKey("AuthorId");

                    b.HasOne("PekoBot.Entities.Models.Channel", "Channel")
                        .WithMany("Messages")
                        .HasForeignKey("ChannelId");

                    b.Navigation("Author");

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Role", b =>
                {
                    b.HasOne("PekoBot.Entities.Models.Guild", "Guild")
                        .WithMany("Roles")
                        .HasForeignKey("GuildId");

                    b.HasOne("PekoBot.Entities.Models.User", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserId");

                    b.HasOne("PekoBot.Entities.Models.VTuber", "VTuber")
                        .WithMany()
                        .HasForeignKey("VTuberId");

                    b.Navigation("Guild");

                    b.Navigation("VTuber");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.User", b =>
                {
                    b.HasOne("PekoBot.Entities.Models.Guild", "Guild")
                        .WithMany("Users")
                        .HasForeignKey("GuildId");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.VTuber", b =>
                {
                    b.HasOne("PekoBot.Entities.Models.Company", "Company")
                        .WithMany("VTubers")
                        .HasForeignKey("CompanyId");

                    b.HasOne("PekoBot.Entities.Models.Emoji", "Emoji")
                        .WithMany()
                        .HasForeignKey("EmojiId");

                    b.Navigation("Company");

                    b.Navigation("Emoji");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Channel", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Company", b =>
                {
                    b.Navigation("VTubers");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.Guild", b =>
                {
                    b.Navigation("Channels");

                    b.Navigation("Roles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.User", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("PekoBot.Entities.Models.VTuber", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Channels");
                });
#pragma warning restore 612, 618
        }
    }
}
