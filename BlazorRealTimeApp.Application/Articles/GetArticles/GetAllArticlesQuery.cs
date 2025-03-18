using BlazorRealTimeApp.Domain.Articles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Application.Articles.GetArticles
{
    public record GetAllArticlesQuery : IRequest<List<Article>>;
}
