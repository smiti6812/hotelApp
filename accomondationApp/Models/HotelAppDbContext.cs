using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace accomondationApp.Models;

public partial class HotelAppDbContext : DbContext
{
    public HotelAppDbContext()
    {
    }

    public HotelAppDbContext(DbContextOptions<HotelAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomCapacity> RoomCapacities { get; set; }

    public virtual DbSet<RoomStatus> RoomStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__B611CB7D359B6339");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.PaymentStatusId).HasName("PK__PaymentS__29CD0BBCAA055745");

            entity.ToTable("PaymentStatus");

            entity.Property(e => e.PaymentStatusId).HasColumnName("paymentStatusId");
            entity.Property(e => e.PaymentStatus1)
                .HasMaxLength(200)
                .HasColumnName("paymentStatus");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__B14BF5C5241AB6AB");

            entity.ToTable("Reservation");

            entity.Property(e => e.ReservationId).HasColumnName("reservationId");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("endDate");
            entity.Property(e => e.PaymentStatusId).HasColumnName("paymentStatusId");
            entity.Property(e => e.RommId).HasColumnName("rommId");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("startDate");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Reservati__custo__440B1D61");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.PaymentStatusId)
                .HasConstraintName("FK__Reservati__payme__44FF419A");

            entity.HasOne(d => d.Romm).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.RommId)
                .HasConstraintName("FK__Reservati__rommI__4316F928");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__6C3BF5BE7642F3F4");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("roomId");
            entity.Property(e => e.RoomCapacityId).HasColumnName("roomCapacityId");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(250)
                .HasColumnName("roomNumber");
            entity.Property(e => e.RoomStatusId).HasColumnName("roomStatusId");

            entity.HasOne(d => d.RoomCapacity).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomCapacityId)
                .HasConstraintName("FK__Room__roomCapaci__3B75D760");

            entity.HasOne(d => d.RoomStatus).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomStatusId)
                .HasConstraintName("FK__Room__roomStatus__3C69FB99");
        });

        modelBuilder.Entity<RoomCapacity>(entity =>
        {
            entity.HasKey(e => e.RoomCapacityId).HasName("PK__RoomCapa__FB83523F509E77E5");

            entity.ToTable("RoomCapacity");

            entity.Property(e => e.RoomCapacityId)
                .HasComment("it shows the number of beds")
                .HasColumnName("roomCapacityId");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
        });

        modelBuilder.Entity<RoomStatus>(entity =>
        {
            entity.HasKey(e => e.RoomStatusId).HasName("PK__RoomStat__AB16F41A7A22DF69");

            entity.ToTable("RoomStatus");

            entity.Property(e => e.RoomStatusId).HasColumnName("roomStatusId");
            entity.Property(e => e.Status)
                .HasMaxLength(200)
                .HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
