﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Models.Repositories;

public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : 
    IdentityDbContext<ApplicationUser>(options)
{
#if DEBUG2
    public ApplicationDbContext() :base(BuildOptions())
    {
    }

    private static DbContextOptions BuildOptions()
    {
        return new DbContextOptionsBuilder()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TaskMetaDev;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;
    }
#endif

    public virtual DbSet<Fund> Funds { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TaskActivity> TaskActivities { get; set; }

    public virtual DbSet<TaskDefinition> TaskDefinitions { get; set; }

    public virtual DbSet<TaskWeek> TaskWeeks { get; set; }

    public virtual DbSet<TransactionCategory> TransactionCategories { get; set; }

    public virtual DbSet<TransactionLog> TransactionLogs { get; set; }
    public virtual DbSet<Job> Jobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Fund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Funds");

            entity.Property(e => e.TargetBalance).HasDefaultValue(0.0m);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status");
        });

        modelBuilder.Entity<TaskActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskActivities");

            entity.HasOne(d => d.TaskWeek).WithMany(p => p.TaskActivityList)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskActivities_TaskWeeks");
        });

        modelBuilder.Entity<TaskDefinition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskDefinition");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.HasOne(d => d.User).WithMany(p => p.TaskDefinitionUsers).HasConstraintName("FK_TaskDefinitions_AspNetUsers");
        });

        modelBuilder.Entity<TaskWeek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TaskWeeks");
            entity.HasOne(d => d.User).WithMany(p => p.UserTaskWeeks).HasConstraintName("FK_TaskWeeks_User");

        });

        modelBuilder.Entity<TransactionCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TransactionCategories");

            entity.Property(e => e.Name).IsFixedLength();
        });

        modelBuilder.Entity<TransactionLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TransactionLog");

            entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.SourceFund).WithMany(p => p.TransactionLogSourceFunds).HasConstraintName("FK_TransactionLog_SourceFund");

            entity.HasOne(d => d.TargetFund).WithMany(p => p.TransactionLogTargetFunds).HasConstraintName("FK_TransactionLog_TargetFund");
            entity.HasOne(d => d.Category).WithMany(p => p.TransactionLogs).OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("FK_TransactionLog_Category");
        });
        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Jobs");
            entity.Property(e => e.DateAssigned).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.User).WithMany(p => p.JobUsers).HasConstraintName("FK_Jobs_AspNetUsers");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}