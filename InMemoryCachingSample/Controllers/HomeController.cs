using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InMemoryCachingSample.Models;
using InMemoryCachingSample.Services;

namespace InMemoryCachingSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsersService _usersService;
        private readonly ICacheService _cacheService;

        public HomeController(ILogger<HomeController> logger, IUsersService usersService, ICacheService cacheService)
        {
            _logger = logger;
            _usersService = usersService;
            _cacheService = cacheService;
        }

        public IActionResult Index()
        {
            var users = _cacheService.GetCachedUser();
            if (users == null) return View();

            var cachedEntry = users.FirstOrDefault();
            return View((nameof(Index), cachedEntry));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        /* Actions to set the value in cache */
        public async Task<IActionResult> CacheUser()
        {
            var users = await _usersService.GetUsersAsync();
            var cacheEntry = users.First();

            return View(nameof(Index), cacheEntry);
        }

        private IActionResult GetCachedUser(string view)
        {
            var users = _cacheService.GetCachedUser();
            if (users == null) return View(view);

            var cachedEntry = users.FirstOrDefault();
            return View(view, cachedEntry);
        }
        
        /* Actions to clear the cache */
        public IActionResult ClearCache()
        {
            _cacheService.ClearCache();
            return RedirectToAction(nameof(Index));
        }
    }
}