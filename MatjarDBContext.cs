using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public partial class MatjarDBContext : DbContext
    {
        public MatjarDBContext()
        {
        }
        public MatjarDBContext(DbContextOptions<MatjarDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<ItemTag> ItemTags { get; set; }
        public virtual DbSet<Attribute> Attributes { get; set; }
        public virtual DbSet<ItemVariation> ItemVariations { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Startup.Configuration.GetSection("Connection")["ConnectionString"].ToString();
                optionsBuilder.UseSqlServer(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
                base.OnConfiguring(optionsBuilder);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AS");


            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Disabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Disabled)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Token).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_UserRoles");

            });

            modelBuilder.Entity<Category>(entity =>
            {

                entity.Property(e => e.ArabicName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishName)
                    .IsRequired()
                    .HasMaxLength(50);

            });

            modelBuilder.Entity<UserRole>(entity =>
            {

                entity.Property(e => e.ArabicName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishName)
                    .IsRequired()
                    .HasMaxLength(50);
            });


            modelBuilder.Entity<Item>(entity =>
            {

                entity.Property(e => e.ArabicName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Disabled).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Type)
                .WithMany(p => p.Items)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Items_Types");

            });


            modelBuilder.Entity<Tag>(entity =>
            {

                entity.Property(e => e.ArabicName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ItemTag>()
                .HasKey(bc => new { bc.ItemId, bc.TagId });
            modelBuilder.Entity<ItemTag>()
                .HasOne(bc => bc.Item)
                .WithMany(b => b.ItemTags)
                .HasForeignKey(bc => bc.ItemId);
            modelBuilder.Entity<ItemTag>()
                .HasOne(bc => bc.Tag)
                .WithMany(c => c.ItemTags)
                .HasForeignKey(bc => bc.TagId);

            modelBuilder.Entity<Attribute>(entity =>
            {

                entity.Property(e => e.ArabicName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Type)
                .WithMany(p => p.Attributes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attributes_Types");

            });


            modelBuilder.Entity<ItemVariation>(entity =>
            {

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemVariations)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemVariations_Items");

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.ItemVariations)
                    .HasForeignKey(d => d.AttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemVariations_Attributes");

            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
