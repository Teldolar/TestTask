using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerApplication.DB.Models;

namespace ServerApplication.DB
{
    public class ServerApplicationDBContext : DbContext
    {
        private string ConnectionString;
        public ServerApplicationDBContext(string connectionString) : base()
        {
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(ConnectionString);
            }
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<MouseLogs> MouseLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Login).HasMaxLength(30);

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.Mail).HasMaxLength(40);

                entity.Property(e => e.Mail).HasMaxLength(15);

                entity.Property(e => e.Role).HasMaxLength(5);
            });

            modelBuilder.Entity<MouseLogs>(entity =>
            {
                entity.HasKey(e =>e.Id);

                entity.Property(e => e.UserId).HasMaxLength(30);

                entity.Property(e => e.Message).HasMaxLength(20);

                entity.Property(e => e.DateTime).HasMaxLength(40);

                entity.Property(e => e.PositionX).HasMaxLength(5);

                entity.Property(e => e.PositionY).HasMaxLength(5);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MouseLogs)
                    .HasForeignKey(d => d.UserId);
            });
        }
    }
}
