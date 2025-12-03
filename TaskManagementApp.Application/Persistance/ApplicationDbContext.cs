using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<ProjectDeveloper> ProjectDevelopers => Set<ProjectDeveloper>();
        public DbSet<TaskRequest> TaskRequests => Set<TaskRequest>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed default roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Manager" },
                new Role { RoleId = 2, Name = "Developer" }
            );

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany(u => u.ManagedProjects)
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectDeveloper>()
                .HasKey(pd => new { pd.ProjectId, pd.DeveloperId });

            modelBuilder.Entity<ProjectDeveloper>()
                .HasOne(pd => pd.Project)
                .WithMany(p => p.ProjectDevelopers)
                .HasForeignKey(pd => pd.ProjectId);

            modelBuilder.Entity<ProjectDeveloper>()
                .HasOne(pd => pd.Developer)
                .WithMany(u => u.ProjectDevelopers)
                .HasForeignKey(pd => pd.DeveloperId);
        }
    }
    }

