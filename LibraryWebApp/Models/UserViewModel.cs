namespace LibraryWebApp.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
