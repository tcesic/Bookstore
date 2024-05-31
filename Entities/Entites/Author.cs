
using System.ComponentModel.DataAnnotations;

namespace Model.Entites
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 100 characters.")]
        public string? Name { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}

