using MeetingWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Message> Messages { get; set; }


        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Фотографии пользователя
            modelBuilder.Entity<User>()
                    .HasMany(p => p.Photos)
                    .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId);

            //Диалоги
            modelBuilder.Entity<DialogsUsers>()
                .HasKey(t => new { t.UserId, t.DialogId });

            modelBuilder.Entity<DialogsUsers>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.DialogsUsers)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<DialogsUsers>()
                .HasOne(sc => sc.Dialog)
                .WithMany(c => c.DialogsUsers)
                .HasForeignKey(sc => sc.DialogId);
        }
    }
}
