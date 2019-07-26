using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
//using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using MySql.Data.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace MvcMovie.Models
{
    public class DataContext : DbContext
    {


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          

            //  modelBuilder.Entity<Role>()
            // .HasMany(c => c.Users)
            // .WithOne(e => e.Roles);

            modelBuilder.Entity<UserRole>()
                .HasKey(bc => new { bc.UserId, bc.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserRoles)
                .HasForeignKey(bc => bc.RoleId);

            modelBuilder.Entity<UserRole>()
                .HasOne(bc => bc.Role)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(bc => bc.RoleId);


            // modelBuilder.Entity<LibDocument>()
            // .Property(b => b.User_id)
            // .HasColumnName("User_Id_Fk");

            modelBuilder.Entity<User>()
            .HasMany(us => us.Documents)
            .WithOne(u => u.User);

            //  modelBuilder.Entity<User>()
            // .HasMany(us => us.Roles)
            // .WithOne(u => u.User);
            
        }



        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<LibDocument> Documents { get; set; }





        // protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        // {
        //     optionBuilder.UseMySql("server=localhost;database=library;user=root;password=5080405;SslMode=none;CharSet=utf8;");
        // }

    }

}