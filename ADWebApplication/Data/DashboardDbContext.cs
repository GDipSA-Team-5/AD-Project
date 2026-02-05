using ADWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ADWebApplication.Data
{
    public class DashboardDbContext : DbContext
    {
        public DashboardDbContext(DbContextOptions<DashboardDbContext> options) : base(options) { }

        public DbSet<PublicUser> Users => Set<PublicUser>();
        public DbSet<DisposalLogs> DisposalLogs => Set<DisposalLogs>();
        public DbSet<DisposalLogItem> DisposalLogItems => Set<DisposalLogItem>();
        public DbSet<EWasteItemType> EWasteItemTypes => Set<EWasteItemType>();
        public DbSet<EWasteCategory> EWasteCategories => Set<EWasteCategory>();
        public DbSet<CollectionBin> CollectionBins => Set<CollectionBin>();
        public DbSet<Region> Regions => Set<Region>();
        public DbSet<FillLevelPrediction> FillLevelPredictions => Set<FillLevelPrediction>();

        //Campaigns
        public DbSet<Campaign> Campaigns => Set<Campaign>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublicUser>()
                .ToTable("publicuser")
                .Ignore("RewardWallet");

            modelBuilder.Entity<EWasteItemType>()
                .HasOne(t => t.Category)
                .WithMany(c => c.EWasteItemTypes)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<DisposalLogItem>()
                .HasOne(i => i.ItemType)
                .WithMany(t => t.DisposalLogItems)
                .HasForeignKey(i => i.ItemTypeId);

            modelBuilder.Entity<DisposalLogItem>()
                .HasOne(i => i.DisposalLog)
                .WithOne(l => l.DisposalLogItem)
                .HasForeignKey<DisposalLogItem>(i => i.LogId);

            modelBuilder.Entity<CollectionBin>()
                .HasOne(b => b.Region)
                .WithMany()
                .HasForeignKey(b => b.RegionId);

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.ToTable("campaign");
                entity.HasKey(c => c.CampaignId);
                entity.Property(e => e.CampaignId)
                    .HasColumnName("campaignId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CampaignName)
                    .HasColumnName("campaignName")
                    .IsRequired();

                entity.Property(e => e.StartDate)
                    .HasColumnName("startDate")
                    .IsRequired();

                entity.Property(e => e.EndDate)
                    .HasColumnName("endDate")
                    .IsRequired();

                entity.Property(e => e.IncentiveType)
                    .HasColumnName("incentiveType")
                    .IsRequired();

                entity.Property(e => e.IncentiveValue)
                    .HasColumnName("incentiveValue")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasColumnName("description");

                // Indexes
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => new { e.StartDate, e.EndDate });
            });

        }
    }
}
