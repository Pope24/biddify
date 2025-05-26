using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BiddifyDbContext : DbContext
    {
        public BiddifyDbContext(DbContextOptions<BiddifyDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
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
        }
    }
}
