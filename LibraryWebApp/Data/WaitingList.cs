using LibraryWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryWebApp.Data
{
    public class WaitingList : BaseEntity
    {
        public Book Book { get; set; }
        public int BookId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
