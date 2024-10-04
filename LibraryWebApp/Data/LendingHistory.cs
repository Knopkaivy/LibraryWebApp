using LibraryWebApp.Models;

namespace LibraryWebApp.Data
{
    public class LendingHistory : BaseEntity
    {
        public Book Book { get; set; }
        public int BookId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateOnly LeaseStartDate { get; set; }
        public DateOnly LeaseProjectedEndDate { get; set; }
        public DateOnly? LeaseActualEndDate { get; set; }
    }
}
