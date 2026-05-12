using System.Diagnostics;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalAccount.Domain.Models;
using PersonalAccount.Web.Interfaces;
using PersonalAccount.Web.Models;

namespace PersonalAccount.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ISettingsStorage _settings;

    public HomeController(ILogger<HomeController> logger, ISettingsStorage settings)
    {
        _logger = logger;
        _settings = settings;
    }

    public IActionResult Index()
    {
        return View(_settings.Settings);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Settings()
    {


        if (_settings.Settings.Branch != null)
        {
            _settings.Settings.SelectedBranchId = _settings.Settings.Branch.Id;
        }

        ViewBag.BranchesList = new SelectList(_settings.Branches, "Id", "Name");

        return View(_settings.Settings);
    }

    public IActionResult Create(LoadingSettingsModel model)
    {
        // Валидация
        // if (!ModelState.IsValid /* && model.Validate()*/)
        // {
        //     return RedirectToAction("Settings");
        // }
    
        // Сохранить модель
        model.Branch = _settings.Branches.First(x=> x.Id == model.SelectedBranchId); 
        _settings.Settings = model;
        return RedirectToAction("Index");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
