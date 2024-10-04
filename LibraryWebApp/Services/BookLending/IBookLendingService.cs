namespace LibraryWebApp.Services.BookLending
{
    public interface IBookLendingService
    {
        Task LendBook(int bookId, string userId);
    }
}