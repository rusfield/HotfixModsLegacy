using HotfixMods.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.MySqlProvider.EntityFrameworkCore.Contexts
{
    public class CharactersDbContext : DbContext
    {
        string _connectionString;
        public CharactersDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Pomelo MySQL
            //optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));

            // Oracle MySQL
            optionsBuilder.UseMySQL(_connectionString);

            optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterCustomizations>(entity =>
            {
                entity.ToTable("character_customizations");
                entity.HasKey(c => new { c.Guid, c.ChrCustomizationOptionId });
            });

            modelBuilder.Entity<CharacterInventory>(entity =>
            {
                entity.ToTable("character_inventory");
                entity.HasNoKey();
            });

            modelBuilder.Entity<Characters>(entity =>
            {
                entity.ToTable("characters");
            });

            modelBuilder.Entity<ItemInstance>(entity =>
            {
                entity.ToTable("item_instance");
            });

            modelBuilder.Entity<ItemInstanceTransmog>(entity =>
            {
                entity.ToTable("item_instance_transmog");
                entity.HasNoKey();
            });
        }
    }
}
