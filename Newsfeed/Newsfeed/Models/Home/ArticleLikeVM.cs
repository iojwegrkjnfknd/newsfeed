using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Models.Home
{
    public class ArticleLikeVM
    {
        [Required]
        public Guid ArticleId { get; set; }
    }
}
