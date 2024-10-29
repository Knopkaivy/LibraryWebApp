using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryWebApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime? ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public string Description { get; set; }
        [Display(Name = "Cover Image URL")]
        public string? CoverImageUrl { get; set; }
        [NotMapped]
        public bool IsAvailable { get; set; } = true;
        [NotMapped]
        public int WaitingTime { get; set; } = 0;
    }
}
