using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Domain.Models;
using PersonalAccount.Web.Models;

namespace PersonalAccount.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var branch = new BranchModel()
        {
            Name = "Test",
            Id = Guid.NewGuid()
        };

        return View(branch);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
