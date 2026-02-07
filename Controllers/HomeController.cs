using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ApodMvcApp.Models;
using ApodMvcApp.Services;
using ApodMvcApp.Repositories;

namespace ApodMvcApp.Controllers;

public class HomeController : Controller
{
    private readonly IApodService _apodService;
    private readonly IApodRepository _apodRepository;

    public HomeController(IApodService apodService, IApodRepository apodRepository)
    {
        _apodService = apodService;
        _apodRepository = apodRepository;
    }

    public async Task<IActionResult> Index(string? startDate = null, string? endDate = null, int page = 1)
    {
        const int PageSize = 12; // Items per page
        
        // Get all saved APODs from database
        var apods = await _apodRepository.GetAllApodsAsync();
        
        // Filter by date range if provided
        if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
        {
            var start = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);
            apods = apods.Where(a => {
                var date = DateTime.Parse(a.Date);
                return date >= start && date <= end;
            }).ToList();
            
            ViewBag.FilteredRange = $"{startDate} to {endDate}";
        }
        
        // Pagination logic
        int totalItems = apods.Count;
        int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
        page = Math.Max(1, Math.Min(page, totalPages)); // Ensure page is within valid range
        
        var pagedApods = apods.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        
        return View(pagedApods);
    }

    [HttpPost]
    public async Task<IActionResult> FetchToday()
    {
        // Fetch today's APOD from NASA API
        var apod = await _apodService.GetTodayApodAsync();
        
        if (apod != null)
        {
            // Check if already exists
            var exists = await _apodRepository.ExistsAsync(apod.Date);
            if (!exists)
            {
                await _apodRepository.InsertApodAsync(apod);
                TempData["Message"] = $"Successfully saved: {apod.Title}";
            }
            else
            {
                TempData["Message"] = "Today's APOD already exists in database.";
            }
            
            // Redirect with today's date filter
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            return RedirectToAction(nameof(Index), new { startDate = today, endDate = today });
        }
        else
        {
            TempData["Error"] = "Failed to fetch APOD from NASA API.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> FetchRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            TempData["Error"] = "Start date must be before end date.";
            return RedirectToAction(nameof(Index));
        }

        // Limit range to 30 days to avoid API limits
        if ((endDate - startDate).Days > 30)
        {
            TempData["Error"] = "Date range cannot exceed 30 days.";
            return RedirectToAction(nameof(Index));
        }

        var apods = await _apodService.GetApodsByDateRangeAsync(startDate, endDate);
        int saved = 0;

        foreach (var apod in apods)
        {
            var exists = await _apodRepository.ExistsAsync(apod.Date);
            if (!exists)
            {
                await _apodRepository.InsertApodAsync(apod);
                saved++;
            }
        }

        TempData["Message"] = $"Saved {saved} new APOD(s) out of {apods.Count} fetched.";
        
        // Redirect with date filter to show only fetched range
        return RedirectToAction(nameof(Index), new { 
            startDate = startDate.ToString("yyyy-MM-dd"), 
            endDate = endDate.ToString("yyyy-MM-dd") 
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
