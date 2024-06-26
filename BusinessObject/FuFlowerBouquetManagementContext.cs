﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace BusinessObject;

public partial class FuFlowerBouquetManagementContext : DbContext
{
    private string? _connString = null;
    private static IConfiguration _config = null;
    public FuFlowerBouquetManagementContext()
    {
    }


    public FuFlowerBouquetManagementContext(DbContextOptions<FuFlowerBouquetManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<FlowerBouquet> FlowerBouquets { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    private static object instanceLock = new object();

    public static IConfiguration GetConfiguration()
    {
        lock (instanceLock)
        {
            if (_config == null)
                _config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
            return _config;
        }
    }

    private string? GetConnectionString()
    {
        lock (instanceLock)
        {
            if (_connString == null)
                _connString = GetConfiguration()["ConnectionStrings:FuFlowerBouquetManagementContext"];
            return _connString;
        }
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
        {
            builder.AddNLog(new NLogProviderOptions
            {
                CaptureMessageTemplates = true,
                CaptureMessageProperties = true
            });
        })).UseSqlServer(GetConnectionString()).EnableSensitiveDataLogging();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BE675E6E6");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryDescription).HasMaxLength(150);
            entity.Property(e => e.CategoryName)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8F503400A");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("CustomerID");
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(180)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FlowerBouquet>(entity =>
        {
            entity.HasKey(e => e.FlowerBouquetId).HasName("PK__FlowerBo__A139130165E4C1AE");

            entity.ToTable("FlowerBouquet");

            entity.Property(e => e.FlowerBouquetId)
                .ValueGeneratedNever()
                .HasColumnName("FlowerBouquetID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description)
                .HasMaxLength(220)
                .IsUnicode(false);
            entity.Property(e => e.FlowerBouquetName)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(d => d.Category).WithMany(p => p.FlowerBouquets)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__FlowerBou__Categ__2E1BDC42");

            entity.HasOne(d => d.Supplier).WithMany(p => p.FlowerBouquets)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FlowerBou__Suppl__2F10007B");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF1938CF33");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ShippedDate).HasColumnType("datetime");
            entity.Property(e => e.Total).HasColumnType("money");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Order__CustomerI__300424B4");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.FlowerBouquetId }).HasName("PK__OrderDet__C983CA9F0A12A23F");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.FlowerBouquetId).HasColumnName("FlowerBouquetID");
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(d => d.FlowerBouquet).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.FlowerBouquetId)
                .HasConstraintName("FK__OrderDeta__Flowe__30F848ED");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__31EC6D26");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.Property(e => e.SupplierId)
                .ValueGeneratedNever()
                .HasColumnName("SupplierID");
            entity.Property(e => e.SupplierAddress).HasMaxLength(150);
            entity.Property(e => e.SupplierName).HasMaxLength(50);
            entity.Property(e => e.Telephone)
                .HasMaxLength(15)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

