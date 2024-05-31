using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOModels
{
    public class BasicBookRequest 
    {
        [Required]
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        [Required]
        public int? AuthorId { get; set; }
    }
}
