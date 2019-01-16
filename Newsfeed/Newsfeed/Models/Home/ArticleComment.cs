using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Models.Home
{
    public class ArticleComment
    {
        public Guid Id { get; set; }

        [Display(Name = "Created")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Author")]
        public string CreatedByUserName { get; set; }

        [StringLength(127, ErrorMessage = "Cannot exceed 127 characters.")]
        public string Text { get; set; }

        [Display(Name = "Likes")]
        public int NumLikes { get; set; }
    }
}
