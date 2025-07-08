using Microsoft.EntityFrameworkCore;

namespace NewsAggregation.Models
{
    public class NewsAggDBContext : DbContext
    {
        protected IConfiguration _configuration;
        private ILogger<NewsAggDBContext> _logger;
        public NewsAggDBContext(IConfiguration configuration, ILogger<NewsAggDBContext> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ExternalServer> ExternalServers { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PendingNotification> PendingNotifications { get; set; }
        public DbSet<ModeratedKeywords> ModeratedKeywords { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("NewsAggDB"));
            _logger.LogInformation("DB connection is established");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.UserId);
                entity.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
                entity.Property(u => u.UserId).ValueGeneratedOnAdd();

            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r => r.RoleId);
                entity.HasMany(r => r.Users).WithOne(u => u.Role);
                entity.Property(r => r.RoleId).ValueGeneratedOnAdd();
            }
            );
            modelBuilder.Entity<Like>(entity =>
            {
                entity.ToTable("Likes");
                entity.HasKey(l => l.LikeId);
                entity.HasOne(l => l.User).WithMany(u => u.Likes).HasForeignKey(l => l.UserId);
                entity.HasOne(l => l.Article).WithMany(a => a.Likes).HasForeignKey(l => l.ArticleId);
                entity.Property(l => l.LikeId).ValueGeneratedOnAdd();
            }
            );
            modelBuilder.Entity<SavedArticle>(entity =>
            {
                entity.ToTable("SavedArticles");
                entity.HasKey(sa => sa.SavedArticleId);
                entity.HasOne(sa => sa.Article).WithMany(a => a.SavedArticles).HasForeignKey(sa => sa.ArticleId);
                entity.HasOne(sa => sa.User).WithMany(u => u.SavedArticles).HasForeignKey(sa => sa.UserId);
                entity.Property(sa => sa.SavedArticleId).ValueGeneratedOnAdd();
            }
            );
            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("Articles");
                entity.HasKey(a => a.Article_Id);
                entity.Property(a => a.Article_Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<ArticleCategory>(entity =>
            {
                entity.ToTable("Article_Categories");
                entity.HasKey(ac => ac.ArticleCategoryId);
                entity.HasOne(ac => ac.Article).WithMany(a => a.ArticleCategories).HasForeignKey(ac => ac.ArticleId);
                entity.HasOne(ac => ac.Category).WithMany(c => c.ArticleCategories).HasForeignKey(ac => ac.CategoryId);
                entity.Property(ac => ac.ArticleCategoryId).ValueGeneratedOnAdd();
            }
            );
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.Category_Id);
                entity.Property(c => c.Category_Id).ValueGeneratedOnAdd();
            }
            );
            modelBuilder.Entity<ExternalServer>(entity =>
            {
                entity.ToTable("External_Servers");
                entity.HasKey(es => es.Server_ID);
                entity.Property(es => es.Server_ID).ValueGeneratedOnAdd();
            }
            );
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notifications");
                entity.HasKey(n => n.Notification_Id);
                entity.HasOne(n => n.User).WithMany(a => a.Notifications).HasForeignKey(n => n.UserId);
                entity.HasOne(n => n.Category).WithMany(c => c.Notifications).HasForeignKey(n => n.CategoryId);
                entity.Property(n => n.Notification_Id).ValueGeneratedOnAdd();
            }
            );
            modelBuilder.Entity<PendingNotification>(entity =>
            {
                entity.ToTable("Pending_Notifications");
                entity.HasKey(pn => pn.PendingNotificationId);
                entity.HasOne(pn => pn.User).WithMany(u => u.PendingNotifications).HasForeignKey(pn => pn.UserId);
                entity.HasOne(pn => pn.Article).WithMany(a => a.PendingNotifications).HasForeignKey(pn => pn.ArticleId);
                entity.Property(pn => pn.PendingNotificationId).ValueGeneratedOnAdd();
            }
            );
            modelBuilder.Entity<ModeratedKeywords>(entity =>
            {
                entity.ToTable("Moderated_Keywords");
                entity.HasKey(mk => mk.Id);
                entity.Property(mk => mk.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
