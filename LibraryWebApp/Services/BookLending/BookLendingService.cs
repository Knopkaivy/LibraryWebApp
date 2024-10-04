
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Services.BookLending
{
    public class BookLendingService(ApplicationDbContext _context) : IBookLendingService
    {
        public async Task LendBook(int bookId, string userId)
        {
            var bookLendingHistory = await _context.LendingHistory.OrderByDescending(b => b.LeaseStartDate).FirstAsync(b => b.BookId == bookId);
            if (bookLendingHistory != null && bookLendingHistory.LeaseActualEndDate == null) {
                return;
            }
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            int leaseTerm = 14;
            LendingHistory history = new LendingHistory{
                Id = bookId,
                UserId = userId,
                LeaseStartDate = currentDate,
                LeaseProjectedEndDate = currentDate.AddDays(leaseTerm),
            };
            _context.Add(history);
            //TODO - remove book-user entry from waiting list
            await _context.SaveChangesAsync();
        }


    }
}
