using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Data.FakeDbModels
{
    public class ArticleContent
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ArticleId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string Headline { get; set; }

        public string Body { get; set; }

        public DateTime? RetiredDate { get; set; }
    }
}
