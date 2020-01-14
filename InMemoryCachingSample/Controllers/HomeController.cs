using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InMemoryCachingSample.Models;
using InMemoryCachingSample.Services;
using InMemoryCachingSample.Utils;
using Microsoft.Extensions.Caching.Memory;

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
            return View();
        }
        
        public IActionResult BasicSample()
        {
            return GetCachedUser(nameof(BasicSample));
        }
        
        public IActionResult AsyncSample()
        {
            return GetCachedUser(nameof(AsyncSample));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private IActionResult GetCachedUser(string view)
        {
            var users = _cacheService.GetCachedUser();
            if (users == null) return View(view);
            var cachedEntry = users.FirstOrDefault();
            return View(view, cachedEntry);
        }

        public async Task<IActionResult> CacheUser()
        {
            var users = await _usersService.GetUsers();
            var cacheEntry = users.First();
            return View(nameof(BasicSample), cacheEntry);
        }
        
        public async Task<IActionResult> CacheUserAsyncSample()
        {
            var users = await _usersService.GetUsers();
            var cacheEntry = users.First();
            return View(nameof(AsyncSample), cacheEntry);
        }

        public IActionResult CacheRemove()
        {
            _cacheService.ClearCache();
            return RedirectToAction(nameof(BasicSample));
        }
        
        public IActionResult CacheRemoveAsyncSample()
        {
            _cacheService.ClearCache();
            return RedirectToAction(nameof(AsyncSample));
        }
    }
}