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
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ExternalServer> ExternalServers { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("NewsAggDB"));
            _logger.LogInformation("DB connection is established");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.UserId);
            modelBuilder.Entity<Role>().ToTable("Roles").HasKey(r => r.RoleId);
            modelBuilder.Entity<Article>().ToTable("Articles").HasKey(a => a.Article_Id);
            modelBuilder.Entity<Category>().ToTable("Categories").HasKey(c => c.Category_Id);
            modelBuilder.Entity<ExternalServer>().ToTable("ExternalServers").HasKey(e => e.Server_ID);
            modelBuilder.Entity<SavedArticle>().ToTable("SavedArticles").HasKey(e => e.SavedArticleId);
        }
    }
}
