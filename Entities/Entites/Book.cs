
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace Model.Entites
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title length must be between 3 and 100 characters.")]
        public string? Title { get; set; }
        public string? SubTitle { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public Author? Author { get; set; }
    }
}

