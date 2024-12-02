
using LibraryWebApp.Services.BookLending;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Services
{
    public class PeriodicHostedService : BackgroundService, IHostedService
    {
        private readonly ApplicationDbContext _context;
        private BookLendingService _bookLendingService;
        private readonly ILogger<PeriodicHostedService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromHours(24);
        public PeriodicHostedService(ApplicationDbContext context, ILogger<PeriodicHostedService> logger)
        {
            _context = context;
            _bookLendingService = new BookLendingService(_context);
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await _bookLendingService.EndExpiredLeases();
                    _logger.LogInformation($"Executed PeriodicHostedService - Count: {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(
                        $"Failed to execute PeriodicHostedService with exception message {ex.Message}.");
                }
            }
        }
    }
}
