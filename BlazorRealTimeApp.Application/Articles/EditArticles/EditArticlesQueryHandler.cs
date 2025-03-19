using BlazorRealTimeApp.Domain.Articles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Application.Articles.EditArticles
{
    public class EditArticlesQueryHandler : IRequestHandler<EditArticlesQuery, Article>
    {
        private readonly IArticleRepository _articleRepository;

        public EditArticlesQueryHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<Article> Handle(EditArticlesQuery request, CancellationToken cancellationToken)
        {
            var result = await _articleRepository.EditArticleAsync(request.Article);

            return result;
        }
    }
}
