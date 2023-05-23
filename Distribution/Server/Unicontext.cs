using Distribution.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Distribution.Server
{
    public class Unicontext : IdentityDbContext<IdentityUser>
    {
        public Unicontext(DbContextOptions<Unicontext> options)
            : base(options)
        {
        }
        //public virtual DbSet<Area> Area { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });


            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });




            //modelBuilder.Entity<LogisticTaskDetail>(entity =>
            //{
            //    entity.ToTable(nameof(LogisticTaskDetail));
            //    entity.HasOne(d => d.Task)
            //        .WithMany(p => p.LogisticTaskDetails)
            //        .HasForeignKey(d => d.TaskId)
            //        .OnDelete(DeleteBehavior.NoAction)
            //        .HasConstraintName("FK_TaskTaskLine");

            //    modelBuilder.SetStringMaxLenght<LogisticTaskDetail>(250);
            //});



        }


    }

}
