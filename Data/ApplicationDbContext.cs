﻿namespace МоитеГуми.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using МоитеГуми.Data.Models;
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; init; }
        public DbSet<Announcement> Announcements { get; init; }
        public DbSet<Dealer> Dealers { get; init; }
        public DbSet<Connection> Connections { get; init; }
        public DbSet<Confidentiality> Confidentialitis { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Announcement>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Обяви)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .Entity<Announcement>()
                .HasOne(c => c.Dealer)
                .WithMany(d => d.Obqwi)
                .HasForeignKey(c => c.DealerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .Entity<Dealer>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Dealer>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            //kato iztriem edin dilar da ne se trie usera

            base.OnModelCreating(builder);
        }
    }
}
