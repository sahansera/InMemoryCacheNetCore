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
            return RedirectToAction(nameof(CacheGet));
        }
        
        public IActionResult AsyncSample()
        {
            return RedirectToAction(nameof(CacheGetAsyncSample));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        public IActionResult CacheGet()
        {
            var users = _cacheService.GetCachedUser();
            if (users == null) return View(nameof(BasicSample));
            var cachedEntry = users.FirstOrDefault();
            return View(nameof(BasicSample), cachedEntry);
        }

        public IActionResult CacheGetAsyncSample()
        {
            var users = _cacheService.GetCachedUser();
            if (users == null) return View(nameof(AsyncSample));
            var cachedEntry = users.FirstOrDefault();
            return View(nameof(AsyncSample), cachedEntry);
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
            return RedirectToAction("CacheGet");
        }
        
        public IActionResult CacheRemoveAsyncSample()
        {
            _cacheService.ClearCache();
            return RedirectToAction("CacheGetAsyncSample");
        }
    }
}