using NewsAggregation.Middlewares;
using NewsAggregation.Models;
using NewsAggregation.Repository;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services;
using NewsAggregation.Services.BackgroundServices;
using NewsAggregation.Services.Interfaces;
using NewsAggregation.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NewsAggDBContext>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServerService, ServerService>();
builder.Services.AddScoped<IServerRepository, ServerRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IModeratedKeywordService, ModeratedKeywordService>();
builder.Services.AddScoped<IModeratedKeywordRepository, ModeratedKeywordRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<INewsApiAdapter, NewsAggregation.Services.Adapters.TheNewsApiAdapter>();
builder.Services.AddSingleton<INewsApiAdapter, NewsAggregation.Services.Adapters.NewsApiAdapter>();

builder.Services.AddHostedService<ArticleSyncHostedService>();
builder.Services.AddHostedService<EmailNotificationHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ContentTypeVaildation>();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
