using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Data.FakeDbModels
{
    public class Article
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CreatedByUserName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public IEnumerable<ArticleContent> Contents { get; set; } = new List<ArticleContent>();

        public IEnumerable<ArticleComment> Comments { get; set; } = new List<ArticleComment>();

        public IEnumerable<ArticleLike> Likes { get; set; } = new List<ArticleLike>();
    }
}
