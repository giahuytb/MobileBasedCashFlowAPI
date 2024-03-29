﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class MobileBasedCashFlowGameContext : DbContext
    {
        public MobileBasedCashFlowGameContext()
        {
        }

        public MobileBasedCashFlowGameContext(DbContextOptions<MobileBasedCashFlowGameContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Asset> Assets { get; set; } = null!;
        public virtual DbSet<AssetType> AssetTypes { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameMatch> GameMatches { get; set; } = null!;
        public virtual DbSet<GameMod> GameMods { get; set; } = null!;
        public virtual DbSet<GameReport> GameReports { get; set; } = null!;
        public virtual DbSet<GameServer> GameServers { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public virtual DbSet<UserAsset> UserAssets { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("Asset");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.AssetName)
                    .HasMaxLength(20)
                    .HasColumnName("asset_name");

                entity.Property(e => e.AssetPrice).HasColumnName("asset_price");

                entity.Property(e => e.AssetTypeId).HasColumnName("asset_type_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(200)
                    .HasColumnName("image_url");

                entity.Property(e => e.IsInShop).HasColumnName("is_in_shop");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.AssetType)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.AssetTypeId)
                    .HasConstraintName("FK__Asset__asset_typ__4AB81AF0");
            });

            modelBuilder.Entity<AssetType>(entity =>
            {
                entity.ToTable("Asset_type");

                entity.Property(e => e.AssetTypeId).HasColumnName("asset_type_id");

                entity.Property(e => e.AssetTypeName)
                    .HasMaxLength(20)
                    .HasColumnName("asset_type_name");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.GameName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("game_name");

                entity.Property(e => e.GameServerId).HasColumnName("game_server_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.GameServer)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.GameServerId)
                    .HasConstraintName("FK__Game__game_serve__3D5E1FD2");
            });

            modelBuilder.Entity<GameMatch>(entity =>
            {
                entity.HasKey(e => e.MatchId)
                    .HasName("PK__Game_mat__9D7FCBA3CBA9A069");

                entity.ToTable("Game_match");

                entity.Property(e => e.MatchId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("match_id")
                    .IsFixedLength();

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.GameModId).HasColumnName("game_mod_id");

                entity.Property(e => e.HostId).HasColumnName("host_id");

                entity.Property(e => e.LastHostId).HasColumnName("last_host_id");

                entity.Property(e => e.MaxNumberPlayer).HasColumnName("max_number_player");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");

                entity.Property(e => e.TotalRound).HasColumnName("total_round");

                entity.Property(e => e.WinnerId).HasColumnName("winner_id");

                entity.HasOne(d => d.GameMod)
                    .WithMany(p => p.GameMatches)
                    .HasForeignKey(d => d.GameModId)
                    .HasConstraintName("FK__Game_matc__game___5441852A");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.GameMatchHosts)
                    .HasForeignKey(d => d.HostId)
                    .HasConstraintName("FK__Game_matc__host___52593CB8");

                entity.HasOne(d => d.LastHost)
                    .WithMany(p => p.GameMatchLastHosts)
                    .HasForeignKey(d => d.LastHostId)
                    .HasConstraintName("FK__Game_matc__last___534D60F1");

                entity.HasOne(d => d.Winner)
                    .WithMany(p => p.GameMatchWinners)
                    .HasForeignKey(d => d.WinnerId)
                    .HasConstraintName("FK__Game_matc__winne__5165187F");
            });

            modelBuilder.Entity<GameMod>(entity =>
            {
                entity.ToTable("Game_mod");

                entity.Property(e => e.GameModId).HasColumnName("game_mod_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

                entity.Property(e => e.ModName)
                    .HasMaxLength(20)
                    .HasColumnName("mod_name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameMods)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Game_mod__game_i__403A8C7D");
            });

            modelBuilder.Entity<GameReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__Game_rep__779B7C58E596625E");

                entity.ToTable("Game_report");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.ChildrenAmount).HasColumnName("children_amount");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.ExpensePerMonth).HasColumnName("expense_per_month");

                entity.Property(e => e.IncomePerMonth).HasColumnName("income_per_month");

                entity.Property(e => e.IsWin).HasColumnName("is_win");

                entity.Property(e => e.MatchId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("match_id")
                    .IsFixedLength();

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.TotalMoney).HasColumnName("total_money");

                entity.Property(e => e.TotalStep).HasColumnName("total_step");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.MatchId)
                    .HasConstraintName("FK__Game_repo__match__5AEE82B9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Game_repo__user___5BE2A6F2");
            });

            modelBuilder.Entity<GameServer>(entity =>
            {
                entity.ToTable("Game_server");

                entity.HasIndex(e => e.GameVersion, "UQ__Game_ser__3BAE19ACD4E04069")
                    .IsUnique();

                entity.Property(e => e.GameServerId).HasColumnName("game_server_id");

                entity.Property(e => e.GameVersion)
                    .HasMaxLength(20)
                    .HasColumnName("game_version");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MatchId })
                    .HasName("pk_participant_id");

                entity.ToTable("Participant");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.MatchId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("match_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__match__5812160E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__user___571DF1D5");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__User_acc__B9BE370FD53B20AD");

                entity.ToTable("User_account");

                entity.HasIndex(e => e.UserName, "UQ__User_acc__7C9273C415CC0CCB")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(300)
                    .HasColumnName("address");

                entity.Property(e => e.Coin).HasColumnName("coin");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.GameServerId).HasColumnName("game_server_id");

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .HasColumnName("gender");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

                entity.Property(e => e.LastJobSelected)
                    .HasMaxLength(20)
                    .HasColumnName("lastJobSelected");

                entity.Property(e => e.NickName)
                    .HasMaxLength(12)
                    .HasColumnName("nick_name");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("password_hash");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Point).HasColumnName("point");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UserName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.GameServer)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.GameServerId)
                    .HasConstraintName("FK__User_acco__game___44FF419A");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User_acco__role___45F365D3");
            });

            modelBuilder.Entity<UserAsset>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.AssetId })
                    .HasName("pk_userAsset_id");

                entity.ToTable("User_asset");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.LastUsed)
                    .HasColumnType("datetime")
                    .HasColumnName("last_used");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.UserAssets)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_asse__asset__4E88ABD4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAssets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_asse__user___4D94879B");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__User_rol__760965CC6C46945F");

                entity.ToTable("User_role");

                entity.HasIndex(e => e.RoleName, "UQ__User_rol__783254B124D79B74")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(10)
                    .HasColumnName("role_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
