using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Models.Home
{
    public class IndexVM
    {
        public IEnumerable<ArticleSummary> ArticleSummaries { get; set; } = new List<ArticleSummary>();
    }
}
