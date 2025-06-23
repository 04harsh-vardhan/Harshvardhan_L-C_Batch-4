using Microsoft.EntityFrameworkCore;
using NewsAggregation.Models.DTO;

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
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ExternalServer> ExternalServers { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<NotificationPreference> NotificationPreferences { get; set; }

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
            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.ToTable("Keywords");
                entity.HasKey(k => k.Keyword_Id);
                entity.HasOne(k => k.User).WithMany(u => u.Keywords).HasForeignKey(k => k.User_id);
                entity.Property(k => k.Keyword_Id).ValueGeneratedOnAdd();
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
            modelBuilder.Entity<Article>().ToTable("Articles").HasKey(a => a.Article_Id);
            modelBuilder.Entity<Category>().ToTable("Categories").HasKey(c => c.Category_Id);
            modelBuilder.Entity<ExternalServer>().ToTable("External_Servers").HasKey(e => e.Server_ID);
            modelBuilder.Entity<SavedArticle>().ToTable("SavedArticles").HasKey(e => e.SavedArticleId);
            modelBuilder.Entity<ArticleCategory>().ToTable("Article_Categories").HasKey(a => a.ArticleCategoryId);
            modelBuilder.Entity<NotificationPreference>().ToTable("Notification_Preferences").HasKey(n => n.NotificationPreferenceId);
        }
    }
}
