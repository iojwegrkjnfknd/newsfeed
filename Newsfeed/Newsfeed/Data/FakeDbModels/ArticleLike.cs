using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Data.FakeDbModels
{
    public class ArticleLike
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ArticleId { get; set; }

        public string CreatedByUserName { get; set; }
    }
}
