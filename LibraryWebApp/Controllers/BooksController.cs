using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApp.Models;
using LibraryWebApp.Services.BookLending;

namespace LibraryWebApp.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private BookLendingService _bookLendingService;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _bookLendingService = new BookLendingService(_context);
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            List<Book> books = await _context.Book.ToListAsync();          
            List<LendingHistory> openHistory = await _context.LendingHistory.Where(h => h.LeaseActualEndDate == null).ToListAsync();
            foreach(Book book in books)
            {
                var bookHistory = openHistory.Find(h => h.BookId == book.Id);
                var waitingList = await _context.WaitingList.Where(w => w.BookId == book.Id).ToListAsync();
                book.IsAvailable = bookHistory == null && waitingList.Count == 0;
            }
            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            var history = await _context.LendingHistory.OrderByDescending(h => h.LeaseStartDate).FirstOrDefaultAsync(m => m.BookId == id);
            var waitingList = await _context.WaitingList.Where(w => w.BookId == book.Id).ToListAsync();
            book.IsAvailable = (history == null || history.LeaseActualEndDate != null) && waitingList.Count == 0;

            return View(book);
        }

        #region Admin Pages

        // GET: Books/Create
        [Authorize(Roles = "Administrator,Librarian")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,ReleaseDate,Genre,Description,CoverImageUrl")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ReleaseDate,Genre,Description,CoverImageUrl")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/WaitingList
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> WaitingList()
        {
            List<WaitingListItem> waitingList = await (from WL in _context.WaitingList
                                                       join B in _context.Book on WL.BookId equals B.Id
                                                       join U in _context.Users on WL.UserId equals U.Id
                                                       select new WaitingListItem
                                                       {
                                                           Id = WL.Id,
                                                           Book = B,
                                                           User = U,
                                                       }).OrderBy(w => w.Book.Title).ThenBy(w => w.Id).ToListAsync();

            return View(waitingList);
        }

        // GET: Books/OpenLeases
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> OpenLeases()
        {
            List<LendingHistory> openLeases = await (from LH in _context.LendingHistory
                                                       join B in _context.Book on LH.BookId equals B.Id
                                                       join U in _context.Users on LH.UserId equals U.Id
                                                        where LH.LeaseActualEndDate == null
                                                       select new LendingHistory
                                                       {
                                                           Id = LH.Id,
                                                           Book = B,
                                                           User = U,
                                                           LeaseStartDate = LH.LeaseStartDate,
                                                           LeaseProjectedEndDate = LH.LeaseProjectedEndDate,
                                                       }).OrderBy(w => w.Book.Title).ToListAsync();

            return View(openLeases);
        }

        // GET: Books/LendingHistory
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> LendingHistory()
        {
            List<LendingHistory> history = await (from LH in _context.LendingHistory
                                                       join B in _context.Book on LH.BookId equals B.Id
                                                       join U in _context.Users on LH.UserId equals U.Id
                                                       select new LendingHistory
                                                       {
                                                           Id = LH.Id,
                                                           Book = B,
                                                           User = U,
                                                           LeaseStartDate = LH.LeaseStartDate,
                                                           LeaseProjectedEndDate = LH.LeaseProjectedEndDate,
                                                           LeaseActualEndDate = LH.LeaseActualEndDate,
                                                       }).OrderBy(h => h.Book.Title).ThenByDescending(h => h.LeaseStartDate).ToListAsync();

            return View(history);
        }

        // POST: Books/EndLease/
        [HttpPost, ActionName("EndLease")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> EndLease([Bind("Id")] LendingHistory lease)
        {
                await _bookLendingService.EndLease(lease.Id);

            return RedirectToAction(nameof(OpenLeases));
        }

        // POST: Books/EndExpiredLeases/
        [HttpPost, ActionName("EndExpiredLeases")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> EndExpiredLeases()
        {
                await _bookLendingService.EndExpiredLeases();

            return RedirectToAction(nameof(OpenLeases));
        }

        // POST: Books/LendAllAvailableBooksToWaitingList/
        [HttpPost, ActionName("LendAllAvailableBooksToWaitingList")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<IActionResult> LendAllAvailableBooksToWaitingList()
        {
                await _bookLendingService.LendAllAvailableBooksToWaitingList();

            return RedirectToAction(nameof(WaitingList));
        }


        #endregion

        #region User Pages

        // GET: Books/Lend/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Lend(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }




        // POST: Books/Lend/5
        [HttpPost, ActionName("Lend")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Lend(int id)
        {
            var book = await _context.Book.FindAsync(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (book != null && user != null)
            {
                await _bookLendingService.LendBook(id, user.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Books/ReturnBook/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ReturnBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/ReturnBook/5
        [HttpPost, ActionName("ReturnBook")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (book != null && user != null)
            {
                await _bookLendingService.ReturnBook(id, user.Id);
            }

            return RedirectToAction(nameof(MyBooks));
        }

        // GET: Books/PlaceHold/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PlaceHold(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var history = await _context.LendingHistory.OrderByDescending(h => h.LeaseStartDate).FirstOrDefaultAsync(m => m.BookId == id);
            book.IsAvailable = history == null || history.LeaseActualEndDate != null;

            if (!book.IsAvailable) { 
                var waitingList = await _context.WaitingList.Where(m => m.BookId == id).ToListAsync();
                int waitingTimeInWeeks = waitingList == null ? 0 : waitingList.Count * 2;
                waitingTimeInWeeks += (int)Math.Ceiling(((decimal)history.LeaseStartDate.DayNumber + 14 - DateOnly.FromDateTime(DateTime.Now).DayNumber)/7); 
                book.WaitingTime = waitingTimeInWeeks;
            }

            return View(book);
        }

        // POST: Books/Lend/5
        [HttpPost, ActionName("PlaceHold")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PlaceHold(int id)
        {
            var book = await _context.Book.FindAsync(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (book != null && user != null)
            {
                await _bookLendingService.PlaceHoldOnBook(id, user.Id);
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: MyBooks
        public async Task<IActionResult> MyBooks()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            List<Book> myBooks = new List<Book>();
            List<LendingHistory> myHistory = await _context.LendingHistory.Where(h => h.UserId == user.Id && h.LeaseActualEndDate == null).ToListAsync();
            foreach (LendingHistory lendingHistory in myHistory) { 
                var book = await _context.Book.FindAsync(lendingHistory.BookId);
                if(book != null)
                {
                    myBooks.Add(book);
                }
            }

            return View(myBooks);
        }        
        
        // GET: MyHistory
        public async Task<IActionResult> MyHistory()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            List<Book> myBooksHistory = new List<Book>();
            List<LendingHistory> myHistory = await _context.LendingHistory.Where(h => h.UserId == user.Id && h.LeaseActualEndDate != null).ToListAsync();
            foreach (LendingHistory lendingHistory in myHistory) { 
                var book = await _context.Book.FindAsync(lendingHistory.BookId);
                if(book != null)
                {
                    var bookHistory = await _context.LendingHistory.OrderByDescending(h => h.LeaseStartDate).FirstOrDefaultAsync(m => m.BookId == book.Id);
                    var waitingList = await _context.WaitingList.Where(w => w.BookId == book.Id).ToListAsync();
                    book.IsAvailable = (bookHistory == null || bookHistory.LeaseActualEndDate != null) && waitingList.Count == 0;

                    myBooksHistory.Add(book);
                }
            }

            return View(myBooksHistory);
        }

        // GET: MyWishList
        public async Task<IActionResult> MyWaitingList()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            List<Book> myBooksWaiting = new List<Book>();
            List<WaitingListItem> myWaitingList = await _context.WaitingList.Where(w => w.UserId == user.Id).ToListAsync();
            foreach (WaitingListItem item in myWaitingList)
            {
                var book = await _context.Book.FindAsync(item.BookId);
                if (book != null) { 
                    item.Book = book;
                }
            }

            return View(myWaitingList);
        }


        // GET: Books/RemoveHold/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveHold(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holdItem = await _context.WaitingList.FirstOrDefaultAsync(w => w.Id == id);
            if (holdItem == null)
            {
                return NotFound();
            }
            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == holdItem.BookId);
            if (book == null)
            {
                return NotFound();
            }

            holdItem.Book = book;

            var history = await _context.LendingHistory.OrderByDescending(h => h.LeaseStartDate).FirstOrDefaultAsync(m => m.BookId == id);
            book.IsAvailable = history == null || history.LeaseActualEndDate != null;

            if (!book.IsAvailable)
            {
                var waitingList = await _context.WaitingList.Where(m => m.BookId == id).ToListAsync();
                int waitingTimeInWeeks = waitingList == null ? 0 : waitingList.Count * 2;
                waitingTimeInWeeks += (int)Math.Ceiling(((decimal)history.LeaseStartDate.DayNumber + 14 - DateOnly.FromDateTime(DateTime.Now).DayNumber) / 7);
                book.WaitingTime = waitingTimeInWeeks;
            }

            return View(holdItem);
        }

        // POST: Books/RemoveHold/5
        [HttpPost, ActionName("RemoveHold")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveHold(int id)
        {
            var holdItem = await _context.WaitingList.FindAsync(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (holdItem != null && user != null)
            {
                await _bookLendingService.RemoveHoldOnBook(id, user.Id);
            }

            return RedirectToAction(nameof(MyWaitingList));
        }
        #endregion
        
        
        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
       
    }

    }
