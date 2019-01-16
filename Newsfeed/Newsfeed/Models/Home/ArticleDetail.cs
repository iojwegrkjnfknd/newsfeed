using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Models.Home
{
    public class ArticleDetail : ArticleBase
    {
        [Required]
        [StringLength(4000, ErrorMessage ="Cannot exceed 4000 characters.")]
        public string Body { get; set; }

        public IEnumerable<ArticleComment> Comments { get; set; } = new List<ArticleComment>();
    }
}
