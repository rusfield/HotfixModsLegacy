using HotfixMods.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.MySqlProvider.EntityFrameworkCore.Contexts
{
    public class WorldDbContext : DbContext
    {
        string _connectionString;
        public WorldDbContext(string connectionString)
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
            modelBuilder.Entity<CreatureTemplate>(entity =>
            {
                entity.ToTable("creature_template");
                entity.HasKey(c => c.Entry);
            });

            modelBuilder.Entity<CreatureEquipTemplate>(entity =>
            {
                entity.ToTable("creature_equip_template");
                entity.HasKey(c => new { c.Id, c.CreatureId });
            });

            modelBuilder.Entity<CreatureTemplateModel>(entity =>
            {
                entity.ToTable("creature_template_model");
                entity.HasKey(c => new { c.CreatureDisplayId, c.CreatureId });
            });

            modelBuilder.Entity<CreatureModelInfo>(entity =>
            {
                entity.ToTable("creature_model_info");
                entity.HasKey(c => c.DisplayId);
            });

            modelBuilder.Entity<CreatureTemplateAddon>(entity =>
            {
                entity.ToTable("creature_template_addon");
                entity.HasKey(c => c.Entry);
            });
        }
    }
}
