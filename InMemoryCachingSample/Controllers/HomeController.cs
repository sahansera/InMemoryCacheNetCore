using InMemoryCachingSample.Models;
using InMemoryCachingSample.Services;

namespace InMemoryCachingSample.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IUsersService usersService, ICacheService cacheService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IUsersService _usersService = usersService;
        private readonly ICacheService _cacheService = cacheService;

    public IActionResult Index()
        {
            var users = _cacheService.GetCachedUser();
            if (users == null) return View();

            var cachedEntry = users.FirstOrDefault();
            return View(nameof(Index), cachedEntry);
        }

        public async Task<IActionResult> CacheUser()
        {
            var users = await _usersService.GetUsersAsync();
            var cacheEntry = users.FirstOrDefault();

            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult ClearCache()
        {
            _cacheService.ClearCache();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}