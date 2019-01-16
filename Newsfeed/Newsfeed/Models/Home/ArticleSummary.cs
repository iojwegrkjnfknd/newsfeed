using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Models.Home
{
    public class ArticleSummary : ArticleBase
    {
        [Display(Name = "Body")]
        public string BodyPreview { get; set; }

        [Display(Name = "Comments")]
        public int NumComments { get; set; }
    }
}
