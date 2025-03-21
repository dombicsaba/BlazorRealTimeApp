using BlazorRealTimeApp.Application.Common.Interfaces;
using BlazorRealTimeApp.Domain.Articles;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly ILogger _logger;
        private readonly IRealTimeNotifier _notifier;

        public ArticleRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IRealTimeNotifier notifier)
        {
            _contextFactory = contextFactory;
            _logger = Log.ForContext<ArticleRepository>();
            _notifier = notifier;
        }

        public async Task<Article> EditArticleAsync(Article? updatedArticle)
        {
            if (updatedArticle == null)
            {
                throw new Exception("Article is null");
            }
            
            _logger.Information("ArticleRepository.EditArticleAsync() | Editing article: {updatedArticle}", updatedArticle);

            using var context = _contextFactory.CreateDbContext();
            var article = await context.Articles.FirstOrDefaultAsync(x => x.Id == updatedArticle.Id);

            if (article == null)
            {
                throw new Exception("Article not found");
            }

            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;
            article.DatePublished = updatedArticle.DatePublished;
            article.IsPublished = updatedArticle.IsPublished;
            article.DateUpdated = DateTime.Now;

            await context.SaveChangesAsync();
            _logger.Information("ArticleRepository.EditArticleAsync() | Article with ID: {ArticleId} updated successfully", article.Id);
            // Értesítse a klienseket a változásról
            //await _notifier.NotifyArticlesUpdated("Article updated");

            return article;
        }

        public async Task<List<Article>> GetAllArticlesAsync()
        {
            _logger.Information("ArticleRepository.GetAllArticlesAsync() | Getting all articles");
            using var context = _contextFactory.CreateDbContext();
            return await context.Articles.ToListAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            _logger.Information("ArticleRepository.GetArticleByIdAsync() | Getting article by id: {id}", id);
            using var context = _contextFactory.CreateDbContext();
            return await context.Articles.FindAsync(id);
        }
    }
}
