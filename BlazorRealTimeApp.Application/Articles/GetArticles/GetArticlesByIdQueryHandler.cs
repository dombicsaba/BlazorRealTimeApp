using BlazorRealTimeApp.Domain.Articles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Application.Articles.GetArticles
{
    public class GetArticlesByIdQueryHandler : IRequestHandler<GetArticlesByIdQuery, Article?>
    {
        private readonly IArticleRepository _articleRepository;

        public GetArticlesByIdQueryHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<Article?> Handle(GetArticlesByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetArticleByIdAsync(request.id);

            return article;
        }
    }
}
