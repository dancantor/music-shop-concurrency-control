using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MusicShop.DataAccess.Entity;

namespace MusicShop.DataAccess.Context;

public partial class InstrumentContext : DbContext
{
    public InstrumentContext()
    {
    }

    public InstrumentContext(DbContextOptions<InstrumentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InstrumentPurchase> InstrumentPurchases { get; set; }

    public virtual DbSet<MusicalInstrument> MusicalInstruments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:InstrumentConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InstrumentPurchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Instrument_Purchase_pkey");

            entity.ToTable("Instrument_Purchase");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateSold).HasColumnName("date_sold");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.InstrumentId).HasColumnName("instrument_id");

            entity.HasOne(d => d.Instrument).WithMany(p => p.InstrumentPurchases)
                .HasForeignKey(d => d.InstrumentId)
                .HasConstraintName("instrument_purchase_foreign_key");
        });

        modelBuilder.Entity<MusicalInstrument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Musical_Instrument_pkey");

            entity.ToTable("Musical_Instrument");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemsStock).HasColumnName("items_stock");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
