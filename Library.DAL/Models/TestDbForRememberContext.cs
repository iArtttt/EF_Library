using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Models;

public partial class TestDbForRememberContext : DbContext
{
    public TestDbForRememberContext()
    {
    }

    public TestDbForRememberContext(DbContextOptions<TestDbForRememberContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<AutorsBook> AutorsBooks { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<Librarian> Librarians { get; set; }

    public virtual DbSet<PublusherType> PublusherTypes { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TEST_DB_FOR_REMEMBER;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Autors__3214EC27B8BFA5B8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Birthday)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("BIRTHDAY");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.SecondName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SECOND_NAME");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SURNAME");
        });

        modelBuilder.Entity<AutorsBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AutorsBo__3214EC275C6BF2A6");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AutorId).HasColumnName("AutorID");
            entity.Property(e => e.BookId).HasColumnName("BookID");

            entity.HasOne(d => d.Autor).WithMany(p => p.AutorsBooks)
                .HasForeignKey(d => d.AutorId)
                .HasConstraintName("FK__AutorsBoo__Autor__02084FDA");

            entity.HasOne(d => d.Book).WithMany(p => p.AutorsBooks)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__AutorsBoo__BookI__02FC7413");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC27BBE8AA0B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AutorId).HasColumnName("AUTOR_ID");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.CodeTypeId).HasColumnName("CodeTypeID");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COUNTRY");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.PublishCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PUBLISH_CODE");
            entity.Property(e => e.Year)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Autor).WithMany(p => p.Books)
                .HasForeignKey(d => d.AutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Books__AUTOR_ID__7D439ABD");

            entity.HasOne(d => d.CodeType).WithMany(p => p.Books)
                .HasForeignKey(d => d.CodeTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Books__CodeTypeI__7E37BEF6");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC2751C9425A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TYPE");
        });

        modelBuilder.Entity<Librarian>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Libraria__3214EC272A2A9F49");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOGIN");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
        });

        modelBuilder.Entity<PublusherType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publushe__3214EC2734FF1996");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Publusher)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.Login).HasName("PK__Readers__E39E26647265DE1F");

            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOGIN");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_NUMBER");
            entity.Property(e => e.DocumentTypeId).HasColumnName("DOCUMENT_TYPE_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SURNAME");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Readers)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Readers__DOCUMEN__778AC167");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
