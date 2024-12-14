
using LibraryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Services.BookLending
{
    public class BookLendingService(ApplicationDbContext _context) : IBookLendingService
    {
        private readonly int _leaseTerm = 1;
        public async Task LendBook(int bookId, string userId)
        {
            var bookLendingHistory = await _context.LendingHistory.OrderByDescending(h => h.Id).FirstOrDefaultAsync(h => h.BookId == bookId);
            if (bookLendingHistory != null && bookLendingHistory.LeaseActualEndDate == null) {
                return;
            }
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            LendingHistory history = new LendingHistory{
                BookId = bookId,
                UserId = userId,
                LeaseStartDate = currentDate,
                LeaseProjectedEndDate = currentDate.AddDays(_leaseTerm),
            };
            _context.Add(history);
            var waitingListEntry = await _context.WaitingList.FirstOrDefaultAsync(w => w.BookId == bookId && w.UserId == userId);
            if (waitingListEntry != null) { 
                _context.Remove(waitingListEntry);
            }
            await _context.SaveChangesAsync();
        }        
        
        public async Task PlaceHoldOnBook(int bookId, string userId)
        {
            var waitingList = await _context.WaitingList.Where(w => w.BookId == bookId).ToListAsync();
            var bookLendingHistory = await _context.LendingHistory.OrderByDescending(b => b.Id).FirstOrDefaultAsync(b => b.BookId == bookId);
            if ((bookLendingHistory == null || bookLendingHistory.LeaseActualEndDate != null) && waitingList.Count == 0) {
                return;
            }

            var outstandingWaitingListEntry = await _context.WaitingList.FirstOrDefaultAsync(w => w.BookId == bookId && w.UserId == userId);
            if (outstandingWaitingListEntry != null) {
                //TODO - add error message "You already have pending hold for this book. Only one hold per book per user is allowed at the same time."
                return;
            }

            WaitingListItem newWaitingListEntry = new WaitingListItem
            {
                BookId = bookId,
                UserId = userId,
            };
            _context.Add(newWaitingListEntry);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveHoldOnBook(int holdId, string userId)
        {
            var holdItem = await _context.WaitingList.FirstOrDefaultAsync(w => w.Id == holdId && w.UserId == userId);
            if (holdItem != null) { 
                _context.Remove(holdItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReturnBook(int recordId, string userId)
        {
            var historyRecord = await _context.LendingHistory.FirstAsync(h => h.Id == recordId && h.UserId == userId);
            if(historyRecord == null)
            {
                return;
            }
            historyRecord.LeaseActualEndDate = DateOnly.FromDateTime(DateTime.Now);
            _context.Update(historyRecord);
            await _context.SaveChangesAsync();
            await LendBookToWaitingItem(historyRecord.BookId);
        }

        internal async Task EndLease(int? leaseId)
        {
            var lease = await _context.LendingHistory.Where(h => h.Id == leaseId).FirstAsync();
            if (lease != null && lease.LeaseActualEndDate == null) {
                lease.LeaseActualEndDate = DateOnly.FromDateTime(DateTime.Now);
                _context.Update(lease);
                await _context.SaveChangesAsync();
				await LendBookToWaitingItem(lease.BookId);
			}
        }

        public async Task EndExpiredLeases()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            List<LendingHistory> expiredLeaseList = await _context.LendingHistory.Where(h => h.LeaseProjectedEndDate <= today && h.LeaseActualEndDate == null).ToListAsync();
            if(expiredLeaseList.Count > 0)
            {
                foreach (var lease in expiredLeaseList) {
                    lease.LeaseActualEndDate = today;
                    _context.Update(lease);
                }
                await _context.SaveChangesAsync();
            }
        }        
        
        public async Task LendBookToWaitingItem(int bookId)
        {
            var waitingItem = await _context.WaitingList.OrderBy(w => w.Id).FirstOrDefaultAsync(w => w.BookId == bookId);
            if (waitingItem == null) {
                return;
            }

            await LendBook(bookId, waitingItem.UserId);
        }        
        public async Task LendAllAvailableBooksToWaitingList()
        {
            var potentialLeaseItems = await _context.WaitingList.OrderBy(w => w.Id).GroupBy(w => w.BookId).Select(w => w.FirstOrDefault()).ToListAsync();

            if (potentialLeaseItems.Count > 0) {
                foreach (var lease in potentialLeaseItems) {
                    if (lease != null) { 
                        var mostRecentLease = await _context.LendingHistory.OrderByDescending(h => h.Id).FirstOrDefaultAsync(h => h.BookId == lease.BookId);
                        if (mostRecentLease != null && mostRecentLease.LeaseActualEndDate != null) { 
                            await LendBookToWaitingItem(lease.BookId);
                        }
                    }
                }
            }
        }
    }
}
