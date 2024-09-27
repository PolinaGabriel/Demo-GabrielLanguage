using GabrielLanguage.Models;
using Microsoft.EntityFrameworkCore;

namespace GabrielLanguage.Context;

public partial class User723Context : DbContext
{
    public User723Context()
    {
    }

    public User723Context(DbContextOptions<User723Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerTag> CustomerTags { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Sex> Sexes { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<VisitFile> VisitFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Host=192.168.2.159;Database=user723;Username=user723;password=53344");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pk");

            entity.ToTable("customer");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Firstdate).HasColumnName("firstdate");
            entity.Property(e => e.Firstname)
                .HasColumnType("character varying")
                .HasColumnName("firstname");
            entity.Property(e => e.Lastdate).HasColumnName("lastdate");
            entity.Property(e => e.Lastname)
                .HasColumnType("character varying")
                .HasColumnName("lastname");
            entity.Property(e => e.Patronym)
                .HasColumnType("character varying")
                .HasColumnName("patronym");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.Photo)
                .HasColumnType("character varying")
                .HasColumnName("photo");
            entity.Property(e => e.SexId).HasColumnName("sex_id");
            entity.Property(e => e.Visits).HasColumnName("visits");

            entity.HasOne(d => d.Sex).WithMany(p => p.Customers)
                .HasForeignKey(d => d.SexId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_sex_fk");
        });

        modelBuilder.Entity<CustomerTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_tag_pk");

            entity.ToTable("customer_tag");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerTags)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_tag_customer_fk");

            entity.HasOne(d => d.Tag).WithMany(p => p.CustomerTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_tag_tag_fk");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("file_pk");

            entity.ToTable("file");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.FileName)
                .HasColumnType("character varying")
                .HasColumnName("fileName");
        });

        modelBuilder.Entity<Sex>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sex_pk");

            entity.ToTable("sex");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.SexName)
                .HasColumnType("character varying")
                .HasColumnName("sexName");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tag_pk");

            entity.ToTable("tag");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Color)
                .HasColumnType("character varying")
                .HasColumnName("color");
            entity.Property(e => e.TagName)
                .HasColumnType("character varying")
                .HasColumnName("tagName");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("visit_pk");

            entity.ToTable("visit");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.Customer).WithMany(p => p.VisitsNavigation)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("visit_customer_fk");
        });

        modelBuilder.Entity<VisitFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_file_pk");

            entity.ToTable("visit_file");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.VisitId).HasColumnName("visit_id");

            entity.HasOne(d => d.File).WithMany(p => p.VisitFiles)
                .HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("visit_file_file_fk");

            entity.HasOne(d => d.Visit).WithMany(p => p.VisitFiles)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("visit_file_visit_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
