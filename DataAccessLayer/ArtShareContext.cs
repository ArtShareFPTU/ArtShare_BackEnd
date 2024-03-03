using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelLayer.BussinessObject;

namespace DataAccessLayer;

public partial class ArtShareContext : DbContext
{
    public ArtShareContext()
    {
    }

    public ArtShareContext(DbContextOptions<ArtShareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; } = null!;
    public virtual DbSet<Artwork> Artworks { get; set; } = null!;
    public virtual DbSet<ArtworkCategory> ArtworkCategories { get; set; } = null!;
    public virtual DbSet<ArtworkTag> ArtworkTags { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Comment> Comments { get; set; } = null!;
    public virtual DbSet<Follow> Follows { get; set; } = null!;
    public virtual DbSet<Inbox> Inboxes { get; set; } = null!;
    public virtual DbSet<Like> Likes { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public virtual DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer(GetConnectionStrings()).EnableSensitiveDataLogging();
        }
    }

    private string GetConnectionStrings()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .SetBasePath(Directory.GetCurrentDirectory())
            .Build();
        return config.GetConnectionString("DefaultDB");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");
            entity.Property(e => e.Avatar).HasMaxLength(255);

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Birthday).HasColumnType("datetime");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.FullName).HasMaxLength(255);

            entity.Property(e => e.Password).HasMaxLength(16);

            entity.Property(e => e.Status).HasMaxLength(30);

            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<Artwork>(entity =>
        {
            entity.ToTable("Artwork");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Fee).HasColumnType("decimal(18, 0)");

            entity.Property(e => e.Status).HasMaxLength(30);

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Artworks)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Artwork__Account__4BAC3F29");
        });

        modelBuilder.Entity<ArtworkCategory>(entity =>
        {
            entity.ToTable("ArtworkCategory");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Artwork)
                .WithMany(p => p.ArtworkCategories)
                .HasForeignKey(d => d.ArtworkId)
                .HasConstraintName("FK__ArtworkCa__Artwo__60A75C0F");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.ArtworkCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__ArtworkCa__Categ__619B8048");
        });

        modelBuilder.Entity<ArtworkTag>(entity =>
        {
            entity.ToTable("ArtworkTag");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Artwork)
                .WithMany(p => p.ArtworkTags)
                .HasForeignKey(d => d.ArtworkId)
                .HasConstraintName("FK__ArtworkTa__Artwo__6477ECF3");

            entity.HasOne(d => d.Tag)
                .WithMany(p => p.ArtworkTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__ArtworkTa__TagId__656C112C");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Comment__Account__59063A47");

            entity.HasOne(d => d.Artwork)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.ArtworkId)
                .HasConstraintName("FK__Comment__Artwork__59FA5E80");
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.ToTable("Follow");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Artist)
                .WithMany(p => p.FollowArtists)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK__Follow__ArtistId__52593CB8");

            entity.HasOne(d => d.Follower)
                .WithMany(p => p.FollowFollowers)
                .HasForeignKey(d => d.FollowerId)
                .HasConstraintName("FK__Follow__Follower__5165187F");
        });

        modelBuilder.Entity<Inbox>(entity =>
        {
            entity.ToTable("Inbox");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Receiver)
                .WithMany(p => p.InboxReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("FK__Inbox__ReceiverI__70DDC3D8");

            entity.HasOne(d => d.Sender)
                .WithMany(p => p.InboxSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK__Inbox__SenderId__6FE99F9F");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Likes)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Likes__AccountId__5535A963");

            entity.HasOne(d => d.Artwork)
                .WithMany(p => p.LikesNavigation)
                .HasForeignKey(d => d.ArtworkId)
                .HasConstraintName("FK__Likes__ArtworkId__5629CD9C");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.Property(e => e.PaymentMethod).HasMaxLength(255);

            entity.Property(e => e.Status).HasMaxLength(30);

            entity.Property(e => e.TotalFee).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Orders__AccountI__4E88ABD4");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Artwork)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ArtworkId)
                .HasConstraintName("FK__OrderDeta__Artwo__693CA210");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__68487DD7");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}