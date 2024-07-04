using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Book_Reservation.Models;

public partial class ReservationDevContext : DbContext
{
    public ReservationDevContext()
    {
    }

    public ReservationDevContext(DbContextOptions<ReservationDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookStock> BookStocks { get; set; }

    public virtual DbSet<BookType> BookTypes { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleItem> SaleItems { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<GenerateNumber> GenerateNumbers { get; set; }

    public virtual DbSet<Flagnoti> Flagnotis { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //=> optionsBuilder.UseSqlServer("Data Source=BORR0W-C19188-B\\SQLEXPRESS;Initial Catalog=Reservation_Dev;Integrated Security=True; Trusted_Connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Thai_100_CI_AS");

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Book");

            entity.Property(e => e.BookIsbn)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BookName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Path) 
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.BookType).WithMany(p => p.Books)
                .HasForeignKey(d => d.BookTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_BookType");
        });

        modelBuilder.Entity<BookStock>(entity =>
        {
            entity.HasKey(e => e.BookStockId).HasName("PK_BookStock_1");

            entity.ToTable("BookStock");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.BookStocks)
            //entity.HasOne(d => d.Book).WithOne(p => p.BookStocks)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_Book_Book");
        });

        modelBuilder.Entity<BookType>(entity =>
        {
            entity.ToTable("BookType");

            entity.Property(e => e.BooktypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK_Person_1");

            entity.ToTable("Person");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.PersonAddress)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PersonCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(96)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(32) // Adjust the length as needed
                .IsUnicode(false);
            entity.Property(e => e.PersonName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PersonTel)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("Sale");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.SaleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Person).WithMany(p => p.Sales)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Person");

            entity.HasOne(d => d.Status).WithMany(p => p.Sales)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Status");
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(e => e.SaleItemId).HasName("PK_SaleItem_1");

            entity.ToTable("SaleItem");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItem_Book");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItem_Sale");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<GenerateNumber>(entity =>
        {
            entity.HasKey(e => new { e.Year, e.Month }).HasName("PK_GenerateNumber");

            entity.ToTable("GenerateNumber");

            entity.Property(e => e.Year).HasColumnName("Year");
            entity.Property(e => e.Month).HasColumnName("Month");
            entity.Property(e => e.Sequence).HasColumnName("Sequence");
        });

        modelBuilder.Entity<Flagnoti>(entity =>
        {
            entity.ToTable("Flagnoti");

            entity.Property(e => e.FlagnotiName)
            .HasMaxLength(3)
            .IsUnicode (false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
