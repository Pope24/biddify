using Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DataAccess
{
    public class BiddifyDbContext : IdentityDbContext<UserEntity>
    {
        public BiddifyDbContext(DbContextOptions<BiddifyDbContext> options) : base(options) { }
        public DbSet<AuctionProductEntity> AuctionProducts { get; set; }
        public DbSet<BidEntity> Bids { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<WinningEntity> Winnings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasMany<AuctionProductEntity>()
                .WithOne(a => a.Seller)
                .HasForeignKey(a => a.SellerId);

            modelBuilder.Entity<BidEntity>()
                .HasOne(b => b.AuctionProduct)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionProductId);

            modelBuilder.Entity<BidEntity>()
                .HasOne(b => b.Bidder)
                .WithMany()
                .HasForeignKey(b => b.BidderId);

            modelBuilder.Entity<MessageEntity>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageEntity>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WinningEntity>()
                .HasOne(w => w.Winner)
                .WithMany()
                .HasForeignKey(w => w.WinnerId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AuctionProductEntity>()
                .Property(x => x.ImageUrls)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                );
            modelBuilder.Entity<AuctionProductEntity>()
                .HasOne(ap => ap.WinningDetails)
                .WithOne(w => w.AuctionProduct)
                .HasForeignKey<WinningEntity>(w => w.AuctionProductId);

            modelBuilder.Entity<TransactionEntity>()
                .Property(t => t.Metadata)
                  .HasColumnType("jsonb")
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                      v => JsonSerializer.Deserialize<TransactionMetadata>(v, new JsonSerializerOptions())
                  );
        }
    }
}
