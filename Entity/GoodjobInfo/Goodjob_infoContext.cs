using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entity.GoodjobInfo
{
    public partial class Goodjob_infoContext : DbContext
    {
        public Goodjob_infoContext()
        {
        }

        public Goodjob_infoContext(DbContextOptions<Goodjob_infoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<ArticleClass> ArticleClasses { get; set; } = null!;
        public virtual DbSet<ArticleImage> ArticleImages { get; set; } = null!;
        public virtual DbSet<ArticleKeyword> ArticleKeywords { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<MemJiLu> MemJiLus { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=192.168.3.2;Initial Catalog=Goodjob_info;User ID=goodjobjishu;Password=juncaiwang/*-;Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("Article");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ArticleContent)
                    .HasColumnType("ntext")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.CoreContent)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EditorId).HasColumnName("EditorID");

                entity.Property(e => e.EditorName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Keyword)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OldUrl)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RelateJobFunction)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<ArticleClass>(entity =>
            {
                entity.ToTable("ArticleClass");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FolderName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PageDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PageKeyword)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PageTitle)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.TemplatePath)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<ArticleImage>(entity =>
            {
                entity.ToTable("ArticleImage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArticleId).HasColumnName("ArticleID");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");
            });

            modelBuilder.Entity<ArticleKeyword>(entity =>
            {
                entity.ToTable("ArticleKeyword");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HtmlCode)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Keyword)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Introduce)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SmallImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MemJiLu>(entity =>
            {
                entity.HasKey(e => e.MemId);

                entity.ToTable("Mem_JiLu");

                entity.Property(e => e.MemId)
                    .ValueGeneratedNever()
                    .HasColumnName("MemID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
