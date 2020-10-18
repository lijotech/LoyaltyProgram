using MemberAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemberAPI.Data.Database
{
    public class MemberContext : DbContext
    {
        protected MemberContext()
        {
        }

        public MemberContext(DbContextOptions<MemberContext> options)
            : base(options)
        {
        }
        public DbSet<Member> Member { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.MemberId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DOB).HasColumnType("date");

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();
            });
        }
    }
}
