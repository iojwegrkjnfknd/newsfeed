using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Data.FakeDbModels
{
    public class CommentLike
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ArticleCommentId { get; set; }

        public string CreatedByUserName { get; set; }
    }
}
