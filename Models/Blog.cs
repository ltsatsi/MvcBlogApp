using System.ComponentModel.DataAnnotations;

namespace MyBlogApplication.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }


        [Display(Name = "Title")]
        [Required(ErrorMessage = "Blog title is required.")]
        public string Title { get; set; }


        [Display(Name = "Author")]
        [Required(ErrorMessage = "Blog author is required.")]
        public string Author { get; set; }


        [Display(Name = "Content")]
        [Required(ErrorMessage = "Blog content is required.")]
        public string Content { get; set; }


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Blog category is required.")]
        public string Category { get; set; }


        [Display(Name = "Summary")]
        [Required(ErrorMessage = "Blog summary is required.")]
        public string Summary {  get; set; }


        [Display(Name = "Featured Image")]
        public string? ImageUrl { get; set; }


        [Display(Name = "Date Published")]
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}
