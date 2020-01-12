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
        private readonly IMemoryCache _cache;
        private readonly IUsersService _usersService;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache, IUsersService usersService)
        {
            _logger = logger;
            _cache = memoryCache;
            _usersService = usersService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Basic()
        {
            return RedirectToAction("CacheGet");
        }
        
        public IActionResult CacheAsync()
        {
            return RedirectToAction("CacheGet");
        }

        public IActionResult Privacy()
        {
            return View("CacheAsync");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        public async Task<IActionResult> CacheGet()
        {
            var users = await _usersService.GetUsers();
            var cacheEntry = users.First();
            return View(nameof(Basic), cacheEntry);
        }
        
        public async Task<IActionResult> CacheUsersAsync()
        {
            var users = await _usersService.GetUsersAsync();
            var cacheEntry = users.First();
            return View(nameof(Basic), cacheEntry);
        }
        
        public async Task<IActionResult> CacheGetOrCreate()
        {
            var users = await _usersService.GetUsers();
            var cacheEntry = users.First();
            return View(nameof(Basic), cacheEntry);
        }
        
        public IActionResult CacheRemove()
        {
            _cache.Remove(CacheKeys.Entry);
            return RedirectToAction("CacheGet");
        }
    }
}