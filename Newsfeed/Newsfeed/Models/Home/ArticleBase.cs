using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newsfeed.Models.Home
{
    public class ArticleBase
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Display(Name = "Author")]
        public string CreatedByUserName { get; set; }

        [Display(Name = "Created")]
        public DateTime CreatedDate { get; set; }

        [Display(Name ="Last edited")]
        public DateTime? LastEditedDate { get; set; }

        [Required]
        [StringLength(127, ErrorMessage ="Cannot exceed 127 characters.")]
        public string Headline { get; set; }

        [Display(Name = "Likes")]
        public int NumLikes { get; set; } = 0;
    }
}
