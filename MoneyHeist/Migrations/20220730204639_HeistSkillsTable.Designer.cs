﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneyHeist.DbContexts;

#nullable disable

namespace MoneyHeist.Migrations
{
    [DbContext(typeof(MoneyHeistContext))]
    [Migration("20220730204639_HeistSkillsTable")]
    partial class HeistSkillsTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("MoneyHeist.Entities.Heist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Heists");
                });

            modelBuilder.Entity("MoneyHeist.Entities.HeistSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HeistId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Members")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HeistId");

                    b.ToTable("HeistSkills");
                });

            modelBuilder.Entity("MoneyHeist.Entities.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MainSkill")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Members");
                });

            modelBuilder.Entity("MoneyHeist.Entities.MemberSkill", b =>
                {
                    b.Property<int>("MemberId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SkillId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Level")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("*");

                    b.HasKey("MemberId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("MembersSkills");
                });

            modelBuilder.Entity("MoneyHeist.Entities.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("MoneyHeist.Entities.HeistSkill", b =>
                {
                    b.HasOne("MoneyHeist.Entities.Heist", "Heist")
                        .WithMany("HeistSkills")
                        .HasForeignKey("HeistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Heist");
                });

            modelBuilder.Entity("MoneyHeist.Entities.MemberSkill", b =>
                {
                    b.HasOne("MoneyHeist.Entities.Member", "Member")
                        .WithMany("MemberSkills")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoneyHeist.Entities.Skill", "Skill")
                        .WithMany("MemberSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("MoneyHeist.Entities.Heist", b =>
                {
                    b.Navigation("HeistSkills");
                });

            modelBuilder.Entity("MoneyHeist.Entities.Member", b =>
                {
                    b.Navigation("MemberSkills");
                });

            modelBuilder.Entity("MoneyHeist.Entities.Skill", b =>
                {
                    b.Navigation("MemberSkills");
                });
#pragma warning restore 612, 618
        }
    }
}
