﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OtakuShelter.Account;

namespace OtakuShelter.Account.Migrations
{
    [DbContext(typeof(AccountContext))]
    partial class AccountContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("OtakuShelter.Account.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnName("created");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnName("passwordhash")
                        .HasMaxLength(500);

                    b.Property<int>("RoleId")
                        .HasColumnName("roleid");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnName("username")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("accounts");
                });

            modelBuilder.Entity("OtakuShelter.Account.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnName("created");

                    b.Property<int?>("CreatorId")
                        .HasColumnName("creatorid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("OtakuShelter.Account.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccountId")
                        .HasColumnName("accountid");

                    b.Property<DateTime>("Created")
                        .HasColumnName("created");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnName("ipaddress")
                        .HasMaxLength(20);

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnName("refresh")
                        .HasMaxLength(500);

                    b.Property<string>("UserAgent")
                        .HasColumnName("useragent")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("tokens");
                });

            modelBuilder.Entity("OtakuShelter.Account.Account", b =>
                {
                    b.HasOne("OtakuShelter.Account.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_role_accounts")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("OtakuShelter.Account.Role", b =>
                {
                    b.HasOne("OtakuShelter.Account.Account", "Creator")
                        .WithMany("Roles")
                        .HasForeignKey("CreatorId")
                        .HasConstraintName("FK_creator_roles")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("OtakuShelter.Account.Token", b =>
                {
                    b.HasOne("OtakuShelter.Account.Account", "Account")
                        .WithMany("Tokens")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK_account_tokens")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
