﻿
using LibraryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Services.BookLending
{
    public class BookLendingService(ApplicationDbContext _context) : IBookLendingService
    {
        public async Task LendBook(int bookId, string userId)
        {
            var bookLendingHistory = await _context.LendingHistory.OrderByDescending(b => b.LeaseStartDate).FirstOrDefaultAsync(b => b.BookId == bookId);
            if (bookLendingHistory != null && bookLendingHistory.LeaseActualEndDate == null) {
                return;
            }
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            int leaseTerm = 14;
            LendingHistory history = new LendingHistory{
                BookId = bookId,
                UserId = userId,
                LeaseStartDate = currentDate,
                LeaseProjectedEndDate = currentDate.AddDays(leaseTerm),
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
            var bookLendingHistory = await _context.LendingHistory.OrderByDescending(b => b.LeaseStartDate).FirstOrDefaultAsync(b => b.BookId == bookId);
            if ((bookLendingHistory == null || bookLendingHistory.LeaseActualEndDate != null) && waitingList.Count == 0) {
                return;
            }

            var outstandingWaitingListEntry = await _context.WaitingList.FirstOrDefaultAsync(w => w.BookId == bookId && w.UserId == userId);
            if (outstandingWaitingListEntry != null) {
                //TODO - add error message "You already have pending hold for this book. Only one hold per book per user is allowed at the same time."
                return;
            }

            WaitingList newWaitingListEntry = new WaitingList{
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

        public async Task ReturnBook(int bookId, string userId)
        {
            var bookLendingHistory = await _context.LendingHistory.OrderByDescending(b => b.LeaseStartDate).FirstAsync(b => b.BookId == bookId && b.UserId == userId);
            if(bookLendingHistory == null)
            {
                return;
            }
            bookLendingHistory.LeaseActualEndDate = DateOnly.FromDateTime(DateTime.Now);
            _context.Update(bookLendingHistory);
            await _context.SaveChangesAsync();
        }

    }
}
