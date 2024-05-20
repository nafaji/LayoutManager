using System;
using System.Collections.Generic;
using LayoutManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LayoutManager.Infrastructure;

public partial class LayoutContext : DbContext
{
    public LayoutContext()
    {
    }

    public LayoutContext(DbContextOptions<LayoutContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Folder> Folders { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Folder>(entity =>
        {
            entity.HasKey(e => new { e.ItemType, e.OwnerId, e.FolderId });

            entity.Property(e => e.ItemType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("itemType");
            entity.Property(e => e.OwnerId).HasColumnName("ownerId");
            entity.Property(e => e.FolderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("folderId");
            entity.Property(e => e.FolderDisplayOrder).HasColumnName("folderDisplayOrder");
            entity.Property(e => e.FolderName)
                .HasMaxLength(255)
                .HasColumnName("folderName");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => new { e.FolderId, e.ItemId });

            entity.Property(e => e.FolderId).HasColumnName("folderId");
            entity.Property(e => e.ItemId)
                .ValueGeneratedOnAdd()
                .HasColumnName("itemId");
            entity.Property(e => e.ItemContent).HasColumnName("itemContent");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .HasColumnName("itemName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
