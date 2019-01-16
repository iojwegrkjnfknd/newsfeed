using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Models.Home
{
    public class CommentVM
    {
        public Guid ArticleId { get; set; }

        [Required]
        [StringLength(127, ErrorMessage = "Cannot exceed 127 characters.")]
        public string Text { get; set; }
    }
}
