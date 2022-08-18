using HotfixMods.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.MySqlProvider.EntityFrameworkCore.Contexts
{
    public class HotfixesDbContext : DbContext
    {
        string _connectionString;
        public HotfixesDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Pomelo MySQL
            // optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));

            // Oracle MySQL
            optionsBuilder.UseMySQL(_connectionString);

            optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NpcModelItemSlotDisplayInfo>(entity =>
            {
                entity.ToTable("npc_model_item_slot_display_info");
            });
            modelBuilder.Entity<CreatureDisplayInfoExtra>(entity =>
            {
                entity.ToTable("creature_display_info_extra");
            });
            modelBuilder.Entity<CreatureDisplayInfoOption>(entity =>
            {
                entity.ToTable("creature_display_info_option");
            });
            modelBuilder.Entity<HotfixData>(entity =>
            {
                entity.ToTable("hotfix_data");
            });
            modelBuilder.Entity<CreatureDisplayInfo>(entity =>
            {
                entity.ToTable("creature_display_info");
            });
            modelBuilder.Entity<ItemDisplayInfoMaterialRes>(entity =>
            {
                entity.ToTable("item_display_info_material_res");
            });
            modelBuilder.Entity<ItemDisplayInfo>(entity =>
            {
                entity.ToTable("item_display_info");
            });
            modelBuilder.Entity<ItemAppearance>(entity =>
            {
                entity.ToTable("item_appearance");
            });
            modelBuilder.Entity<ItemSearchName>(entity =>
            {
                entity.ToTable("item_search_name");
            });
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");
            });
            modelBuilder.Entity<ItemModifiedAppearance>(entity =>
            {
                entity.ToTable("item_modified_appearance");
            });
            modelBuilder.Entity<ItemSparse>(entity =>
            {
                entity.ToTable("item_sparse");
            });
            modelBuilder.Entity<HotfixModsData>(entity =>
            {
                entity.ToTable("hotfix_mods_data");
            });
            modelBuilder.Entity<SoundKit>(entity =>
            {
                entity.ToTable("sound_kit");
            });
            modelBuilder.Entity<SoundKitEntry>(entity =>
            {
                entity.ToTable("sound_kit_entry");
            });
            modelBuilder.Entity<AnimKit>(entity =>
            {
                entity.ToTable("anim_kit");
            });
            modelBuilder.Entity<AnimKitSegment>(entity =>
            {
                entity.ToTable("anim_kit_segment");
            });
        }
    }
}
