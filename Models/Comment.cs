using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlogApplication.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "Comment is required.")]
        public string Content { get; set; }


        [Display(Name = "Date Published")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        // Relationship navigation
        [Column("BlogId")]
        public int BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; }


        [Column("AuthorId")]
        public Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser? Author { get; set; }
    }
}
