using System;
using System.Collections.Generic;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Balance> Balances { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ReceiptDocument> ReceiptDocuments { get; set; }

    public virtual DbSet<ReceiptResource> ReceiptResources { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<SnipmentDocument> SnipmentDocuments { get; set; }

    public virtual DbSet<SnipmentResource> SnipmentResources { get; set; }

    public virtual DbSet<UnitMeasurement> UnitMeasurements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Balance>(entity =>
        {
            entity.HasKey(e => new { e.ResourceId, e.UnitId }).HasName("balance_pkey");

            entity.ToTable("balance", "warehouse");

            entity.HasIndex(e => e.UnitId, "IX_balance_unit_id");

            entity.HasIndex(e => e.Id, "balance_id_unique").IsUnique();

            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.HasOne(d => d.Resource).WithMany(p => p.Balances)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("balance_resource_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.Balances)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("balance_unit_id_fkey");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_pkey");

            entity.ToTable("client", "warehouse");

            entity.HasIndex(e => e.Name, "client_name_idx").HasAnnotation("Npgsql:StorageParameter:deduplicate_items", "true");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Archive)
                .HasDefaultValue(false)
                .HasColumnName("archive");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<ReceiptDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("receipt_document_pkey");

            entity.ToTable("receipt_document", "warehouse");

            entity.HasIndex(e => e.Number, "receipt_document_number_idx").HasAnnotation("Npgsql:StorageParameter:deduplicate_items", "true");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Number).HasColumnName("number");
        });

        modelBuilder.Entity<ReceiptResource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("receipt_resource_pkey");

            entity.ToTable("receipt_resource", "warehouse");

            entity.HasIndex(e => e.DocumentId, "IX_receipt_resource_document_id");

            entity.HasIndex(e => e.ResourceId, "IX_receipt_resource_resource_id");

            entity.HasIndex(e => e.UnitId, "IX_receipt_resource_unit_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Document).WithMany(p => p.ReceiptResources)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("receipt_resource_document_id_fkey");

            entity.HasOne(d => d.Resource).WithMany(p => p.ReceiptResources)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("receipt_resource_resource_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.ReceiptResources)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("receipt_resource_unit_id_fkey");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("resource_pkey");

            entity.ToTable("resource", "warehouse");

            entity.HasIndex(e => e.Name, "resource_name_idx").HasAnnotation("Npgsql:StorageParameter:deduplicate_items", "true");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Archive)
                .HasDefaultValue(false)
                .HasColumnName("archive");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<SnipmentDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("snipment_document_pkey");

            entity.ToTable("snipment_document", "warehouse");

            entity.HasIndex(e => e.ClientId, "IX_snipment_document_client_id");

            entity.HasIndex(e => e.Number, "snipment_document_number_idx").HasAnnotation("Npgsql:StorageParameter:deduplicate_items", "true");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Sign).HasColumnName("sign");

            entity.HasOne(d => d.Client).WithMany(p => p.SnipmentDocuments)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("snipment_document_client_id_fkey");
        });

        modelBuilder.Entity<SnipmentResource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("snipment_resource_pkey");

            entity.ToTable("snipment_resource", "warehouse");

            entity.HasIndex(e => e.DocumentId, "IX_snipment_resource_document_id");

            entity.HasIndex(e => e.ResourceId, "IX_snipment_resource_resource_id");

            entity.HasIndex(e => e.UnitId, "IX_snipment_resource_unit_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Document).WithMany(p => p.SnipmentResources)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("snipment_resource_document_id_fkey");

            entity.HasOne(d => d.Resource).WithMany(p => p.SnipmentResources)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("snipment_resource_resource_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.SnipmentResources)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("snipment_resource_unit_id_fkey");
        });

        modelBuilder.Entity<UnitMeasurement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("unit_measurement_pkey");

            entity.ToTable("unit_measurement", "warehouse");

            entity.HasIndex(e => e.Name, "unit_measurement_name_idx").HasAnnotation("Npgsql:StorageParameter:deduplicate_items", "true");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Archive)
                .HasDefaultValue(false)
                .HasColumnName("archive");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
