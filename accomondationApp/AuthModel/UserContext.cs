﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace accomondationApp.AuthModel;

public partial class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LoginModel> LoginModels { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserInRole> UserInRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__CD98462A807D5601");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Role1)
                .HasMaxLength(200)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<UserInRole>(entity =>
        {
            entity.HasKey(e => e.UserInRolesId).HasName("PK__UserInRo__E379CDFE3660A7A8");

            entity.Property(e => e.UserInRolesId).HasColumnName("userInRolesId");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Role).WithMany(p => p.UserInRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserInRol__roleI__4F7CD00D");

            entity.HasOne(d => d.User).WithMany(p => p.UserInRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserInRol__userI__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}