using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlogApplication.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }


        [Display(Name = "Article Title")]
        [Required(ErrorMessage = "Blog title is required.")]
        [StringLength(maximumLength: 200, MinimumLength = 3, 
            ErrorMessage = "Blog title cannot exceed 200 characters and must be atleast 3 characters long.")]
        public string Title { get; set; }


        [Display(Name = "Content")]
        [Required(ErrorMessage = "Blog content is required.")]
        public string Content { get; set; }


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Blog category is required.")]
        [StringLength(maximumLength: 15, MinimumLength = 3,
            ErrorMessage = "Category cannot exceed 15 characters and must be atleast 3 characters long.")]
        public string Category { get; set; }


        [Display(Name = "Summary")]
        [Required(ErrorMessage = "Blog summary is required.")]
        public string Summary {  get; set; }


        [Display(Name = "Featured Image")]
        public string? ImageUrl { get; set; }


        [Display(Name = "Date Published")]
        public DateTime CreatedAt { get; private set; } = DateTime.Now;


        // Relationship navigation
        [Column("AuthorId")]
        public Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser? Author { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
