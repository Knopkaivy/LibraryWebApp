using Microsoft.AspNetCore.Identity;

namespace LibraryWebApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; }
    }
}
