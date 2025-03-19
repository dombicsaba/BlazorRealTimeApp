using BlazorRealTimeApp.Domain.Articles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Article> EditArticleAsync(Article? updatedArticle)
        {
            if (updatedArticle == null)
            {
                throw new Exception("Article is null");
            }

            var article = await _context.Articles.FirstOrDefaultAsync(x => x.Id == updatedArticle.Id);

            if (article == null)
            {
                throw new Exception("Article not found");
            }

            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;
            article.DatePublished = updatedArticle.DatePublished;
            article.IsPublished = updatedArticle.IsPublished;
            article.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();
     
            return article;
        }

        public async Task<List<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }
    }
}
