using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InMemoryCachingSample.Models;
using InMemoryCachingSample.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCachingSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _cache = memoryCache;
        }

        public IActionResult Index()
        {
            return RedirectToAction("CacheGet");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        public IActionResult CacheGet()
        {
            var cacheEntry = _cache.Get<DateTime?>(CacheKeys.Entry);
            return View("Index", cacheEntry);
        }
        
        public IActionResult CacheGetOrCreate()
        {
            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out DateTime cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = DateTime.Now;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                // Save data in cache.
                _cache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }

            return View(nameof(Index), cacheEntry);
        }
        
        public IActionResult CacheRemove()
        {
            _cache.Remove(CacheKeys.Entry);
            return RedirectToAction("CacheGet");
        }
    }
}