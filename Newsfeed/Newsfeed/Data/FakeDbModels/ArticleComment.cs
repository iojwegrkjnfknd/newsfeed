using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Data.FakeDbModels
{
    public class ArticleComment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ArticleId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string CreatedByUserName { get; set; }

        public string Text { get; set; }

        public IEnumerable<CommentLike> Likes { get; set; } = new List<CommentLike>();
    }
}
