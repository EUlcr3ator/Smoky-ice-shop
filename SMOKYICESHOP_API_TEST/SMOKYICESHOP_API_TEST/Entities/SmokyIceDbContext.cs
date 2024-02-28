using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class SmokyIceDbContext : DbContext
    {
        public SmokyIceDbContext()
        {
        }

        public SmokyIceDbContext(DbContextOptions<SmokyIceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CartHasGood> CartHasGoods { get; set; } = null!;
        public virtual DbSet<CartrigesAndVaporizer> CartrigesAndVaporizers { get; set; } = null!;
        public virtual DbSet<CartrigesAndVaporizersGroup> CartrigesAndVaporizersGroups { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Coal> Coals { get; set; } = null!;
        public virtual DbSet<CoalsGroup> CoalsGroups { get; set; } = null!;
        public virtual DbSet<DeliveryInfo> DeliveryInfos { get; set; } = null!;
        public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; } = null!;
        public virtual DbSet<DiscountGood> DiscountGoods { get; set; } = null!;
        public virtual DbSet<Ecigarette> Ecigarettes { get; set; } = null!;
        public virtual DbSet<EcigaretteTasteMix> EcigaretteTasteMixes { get; set; } = null!;
        public virtual DbSet<EcigarettesGroup> EcigarettesGroups { get; set; } = null!;
        public virtual DbSet<EcigarettesTaste> EcigarettesTastes { get; set; } = null!;
        public virtual DbSet<Good> Goods { get; set; } = null!;
        public virtual DbSet<HookahTobacco> HookahTobaccos { get; set; } = null!;
        public virtual DbSet<HookahTobaccoGroup> HookahTobaccoGroups { get; set; } = null!;
        public virtual DbSet<HookahTobaccoStrength> HookahTobaccoStrengths { get; set; } = null!;
        public virtual DbSet<HookahTobaccoTaste> HookahTobaccoTastes { get; set; } = null!;
        public virtual DbSet<HookahTobaccoTasteMix> HookahTobaccoTasteMixes { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Liquid> Liquids { get; set; } = null!;
        public virtual DbSet<LiquidTasteMix> LiquidTasteMixes { get; set; } = null!;
        public virtual DbSet<LiquidsGroup> LiquidsGroups { get; set; } = null!;
        public virtual DbSet<LiquidsTaste> LiquidsTastes { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderHasGood> OrderHasGoods { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<Pod> Pods { get; set; } = null!;
        public virtual DbSet<PodsGroup> PodsGroups { get; set; } = null!;
        public virtual DbSet<PopularGood> PopularGoods { get; set; } = null!;
        public virtual DbSet<Producer> Producers { get; set; } = null!;
        public virtual DbSet<RecomendedGood> RecomendedGoods { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-CNCL2FQ;Initial Catalog=smoky-ice-db;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartHasGood>(entity =>
            {
                entity.HasKey(e => new { e.CartId, e.GoodId });

                entity.ToTable("CartHasGood");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.GoodId).HasColumnName("GoodID");

                entity.Property(e => e.ProductCount).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartHasGoods)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartHasGood_Clients");

                entity.HasOne(d => d.Good)
                    .WithMany(p => p.CartHasGoods)
                    .HasForeignKey(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartHasGood_Goods");
            });

            modelBuilder.Entity<CartrigesAndVaporizer>(entity =>
            {
                entity.HasKey(e => e.GoodId);

                entity.Property(e => e.GoodId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoodID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.CartrigesAndVaporizer)
                    .HasForeignKey<CartrigesAndVaporizer>(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartrigesAndVaporizers_Goods");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.CartrigesAndVaporizers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartrigesAndVaporizers_CartrigesAndVaporizersGroups");
            });

            modelBuilder.Entity<CartrigesAndVaporizersGroup>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Line).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProducerId).HasColumnName("ProducerID");

                entity.Property(e => e.SpiralType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.CartrigesAndVaporizersGroups)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartrigesAndVaporizersGroups_Images");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.CartrigesAndVaporizersGroups)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartrigesAndVaporizersGroups_Producers");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameUkr)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasMany(d => d.Producers)
                    .WithMany(p => p.Categories)
                    .UsingEntity<Dictionary<string, object>>(
                        "CategoryHasProducer",
                        l => l.HasOne<Producer>().WithMany().HasForeignKey("ProducerId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CategoryHasProducer_Producers"),
                        r => r.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CategoryHasProducer_Categories"),
                        j =>
                        {
                            j.HasKey("CategoryId", "ProducerId");

                            j.ToTable("CategoryHasProducer");

                            j.IndexerProperty<byte>("CategoryId").HasColumnName("CategoryID");

                            j.IndexerProperty<Guid>("ProducerId").HasColumnName("ProducerID");
                        });
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ChatId).HasColumnName("ChatID");

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clients_Roles");
            });

            modelBuilder.Entity<Coal>(entity =>
            {
                entity.HasKey(e => e.GoodId)
                    .HasName("PK_Coal");

                entity.Property(e => e.GoodId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoodID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.Coal)
                    .HasForeignKey<Coal>(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Coal_Goods");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Coals)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Coals_CoalsGroups");
            });

            modelBuilder.Entity<CoalsGroup>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Line).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProducerId).HasColumnName("ProducerID");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.CoalsGroups)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoalsGroups_Images");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.CoalsGroups)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoalsGroups_Producers");
            });

            modelBuilder.Entity<DeliveryInfo>(entity =>
            {
                entity.ToTable("DeliveryInfo");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.DeliveryCity).IsUnicode(false);

                entity.Property(e => e.DeliveryMethodId).HasColumnName("DeliveryMethodID");

                entity.Property(e => e.DeliveryRegion).IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostOfficeNumber).IsUnicode(false);

                entity.HasOne(d => d.DeliveryMethod)
                    .WithMany(p => p.DeliveryInfos)
                    .HasForeignKey(d => d.DeliveryMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryInfo_DeliveryMethods");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.DeliveryInfo)
                    .HasForeignKey<DeliveryInfo>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryInfo_Orders");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.DeliveryInfos)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryInfo_PaymentMethods");
            });

            modelBuilder.Entity<DeliveryMethod>(entity =>
            {
                entity.ToTable("DeliveryMethods ");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.NameCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameUkr)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DiscountGood>(entity =>
            {
                entity.HasKey(e => e.GoodId);

                entity.Property(e => e.GoodId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoodID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.DiscountGood)
                    .HasForeignKey<DiscountGood>(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountGoods_Goods");
            });

            modelBuilder.Entity<Ecigarette>(entity =>
            {
                entity.HasKey(e => e.GoodId);

                entity.ToTable("ECigarettes");

                entity.Property(e => e.GoodId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoodID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.TasteMixId).HasColumnName("TasteMixID");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.Ecigarette)
                    .HasForeignKey<Ecigarette>(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ECigarettes_Goods");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Ecigarettes)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ECigarettes_ECigarettesGroups");

                entity.HasOne(d => d.TasteMix)
                    .WithMany(p => p.Ecigarettes)
                    .HasForeignKey(d => d.TasteMixId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ECigarettes_ECigaretteTasteMixes");
            });

            modelBuilder.Entity<EcigaretteTasteMix>(entity =>
            {
                entity.ToTable("ECigaretteTasteMixes");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<EcigarettesGroup>(entity =>
            {
                entity.ToTable("ECigarettesGroups");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Line).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProducerId).HasColumnName("ProducerID");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.EcigarettesGroups)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ECigarettesGroups_Images");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.EcigarettesGroups)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ECigarettesGroups_Producers");
            });

            modelBuilder.Entity<EcigarettesTaste>(entity =>
            {
                entity.ToTable("ECigarettesTastes");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasMany(d => d.Mixes)
                    .WithMany(p => p.Tastes)
                    .UsingEntity<Dictionary<string, object>>(
                        "EcigaretteHasTaste",
                        l => l.HasOne<EcigaretteTasteMix>().WithMany().HasForeignKey("MixId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ECigaretteHasTaste_ECigaretteTasteMixes"),
                        r => r.HasOne<EcigarettesTaste>().WithMany().HasForeignKey("TasteId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ECigaretteHasTaste_ECigarettesTastes"),
                        j =>
                        {
                            j.HasKey("TasteId", "MixId");

                            j.ToTable("ECigaretteHasTaste");

                            j.IndexerProperty<Guid>("TasteId").HasColumnName("TasteID");

                            j.IndexerProperty<Guid>("MixId").HasColumnName("MixID");
                        });
            });

            modelBuilder.Entity<Good>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Goods_Categories");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Goods_Images");
            });

            modelBuilder.Entity<HookahTobacco>(entity =>
            {
                entity.HasKey(e => e.GoodId);

                entity.ToTable("HookahTobacco");

                entity.Property(e => e.GoodId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoodID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.HookahTobacco)
                    .HasForeignKey<HookahTobacco>(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HookahTobacco_Goods");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.HookahTobaccos)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HookahTobacco_HookahTobaccoGroups");
            });

            modelBuilder.Entity<HookahTobaccoGroup>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Line).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProducerId).HasColumnName("ProducerID");

                entity.Property(e => e.StrengthId).HasColumnName("StrengthID");

                entity.Property(e => e.TasteMixId).HasColumnName("TasteMixID");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.HookahTobaccoGroups)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HookahTobaccoGroups_Images");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.HookahTobaccoGroups)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HookahTobaccoGroups_Producers");

                entity.HasOne(d => d.Strength)
                    .WithMany(p => p.HookahTobaccoGroups)
                    .HasForeignKey(d => d.StrengthId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HookahTobaccoGroups_HookahTobaccoStrengths");

                entity.HasOne(d => d.TasteMix)
                    .WithMany(p => p.HookahTobaccoGroups)
                    .HasForeignKey(d => d.TasteMixId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HookahTobaccoGroups_HookahTobaccoTasteMixes");
            });

            modelBuilder.Entity<HookahTobaccoStrength>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HookahTobaccoTaste>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Taste)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasMany(d => d.Mixes)
                    .WithMany(p => p.Tastes)
                    .UsingEntity<Dictionary<string, object>>(
                        "HookahTobaccoHasTaste",
                        l => l.HasOne<HookahTobaccoTasteMix>().WithMany().HasForeignKey("MixId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_HookahTobaccoHasTaste_HookahTobaccoTasteMixes"),
                        r => r.HasOne<HookahTobaccoTaste>().WithMany().HasForeignKey("TasteId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_HookahTobaccoHasTaste_HookahTobaccoTastes"),
                        j =>
                        {
                            j.HasKey("TasteId", "MixId");

                            j.ToTable("HookahTobaccoHasTaste");

                            j.IndexerProperty<Guid>("TasteId").HasColumnName("TasteID");

                            j.IndexerProperty<Guid>("MixId").HasColumnName("MixID");
                        });
            });

            modelBuilder.Entity<HookahTobaccoTasteMix>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<Liquid>(entity =>
            {
                entity.HasKey(e => e.GoodId);

                entity.Property(e => e.GoodId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoodID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.TasteMixId).HasColumnName("TasteMixID");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.Liquid)
                    .HasForeignKey<Liquid>(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Liquids_Goods");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Liquids)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Liquids_LiquidsGroups");

                entity.HasOne(d => d.TasteMix)
                    .WithMany(p => p.Liquids)
                    .HasForeignKey(d => d.TasteMixId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Liquids_LiquidTasteMixes");
            });

            modelBuilder.Entity<LiquidTasteMix>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<LiquidsGroup>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Line).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NicotineType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProducerId).HasColumnName("ProducerID");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.LiquidsGroups)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LiquidsGroups_Images");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.LiquidsGroups)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LiquidsGroups_Producers");
            });

            modelBuilder.Entity<LiquidsTaste>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Taste)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TasteGroup)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasMany(d => d.TasteMixes)
                    .WithMany(p => p.Tastes)
                    .UsingEntity<Dictionary<string, object>>(
                        "LiquidHasTaste",
                        l => l.HasOne<LiquidTasteMix>().WithMany().HasForeignKey("TasteMixId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_LiquidHasTaste_LiquidTasteMixes"),
                        r => r.HasOne<LiquidsTaste>().WithMany().HasForeignKey("TasteId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_LiquidHasTaste_LiquidsTastes"),
                        j =>
                        {
                            j.HasKey("TasteId", "TasteMixId");

                            j.ToTable("LiquidHasTaste");

                            j.IndexerProperty<Guid>("TasteId").HasColumnName("TasteID");

                            j.IndexerProperty<Guid>("TasteMixId").HasColumnName("TasteMixID");
                        });
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Orders_Clients");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_OrderStatuses");
            });

            modelBuilder.Entity<OrderHasGood>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.GoodId });

                entity.ToTable("OrderHasGood");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.GoodId).HasColumnName("GoodID");

                entity.HasOne(d => d.Good)
                    .WithMany(p => p.OrderHasGoods)
                    .HasForeignKey(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHasGood_Goods");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderHasGoods)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHasGood_Orders");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameUkr)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.NameCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameUkr)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pod>(entity =>
            {
                entity.HasKey(e => e.GoodId);

                entity.Property(e => e.GoodId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoodID");

                entity.Property(e => e.Appearance).HasMaxLength(50);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.Pod)
                    .HasForeignKey<Pod>(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pods_Goods");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Pods)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pods_PodsGroups");
            });

            modelBuilder.Entity<PodsGroup>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Line).HasMaxLength(50);

                entity.Property(e => e.Material)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Port)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Power)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProducerId).HasColumnName("ProducerID");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PodsGroups)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PodsGroups_Images");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.PodsGroups)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PodsGroups_Producers");

                entity.HasMany(d => d.Cartriges)
                    .WithMany(p => p.Pods)
                    .UsingEntity<Dictionary<string, object>>(
                        "CartrigeHasPod",
                        l => l.HasOne<CartrigesAndVaporizersGroup>().WithMany().HasForeignKey("CartrigeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CartrigeHasPod_CartrigesAndVaporizersGroups"),
                        r => r.HasOne<PodsGroup>().WithMany().HasForeignKey("PodId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CartrigeHasPod_PodsGroups"),
                        j =>
                        {
                            j.HasKey("PodId", "CartrigeId");

                            j.ToTable("CartrigeHasPod");

                            j.IndexerProperty<Guid>("PodId").HasColumnName("PodID");

                            j.IndexerProperty<Guid>("CartrigeId").HasColumnName("CartrigeID");
                        });
            });

            modelBuilder.Entity<PopularGood>(entity =>
            {
                entity.HasKey(e => e.GoodId);

                entity.Property(e => e.GoodId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoodID");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.PopularGood)
                    .HasForeignKey<PopularGood>(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PopularGoods_Goods");
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Producers)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Producers_Images");
            });

            modelBuilder.Entity<RecomendedGood>(entity =>
            {
                entity.HasKey(e => e.GoodId);

                entity.Property(e => e.GoodId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoodID");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.RecomendedGood)
                    .HasForeignKey<RecomendedGood>(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecomendedGoods_Goods");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.Property(e => e.ClientId)
                    .ValueGeneratedNever()
                    .HasColumnName("ClientID");

                entity.Property(e => e.Expiration).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithOne(p => p.RefreshToken)
                    .HasForeignKey<RefreshToken>(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefreshTokens_Clients");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Customer')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
