using System.Diagnostics;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalAccount.Domain.Models;
using PersonalAccount.Web.Interfaces;
using PersonalAccount.Web.Models;

namespace PersonalAccount.Web.Controllers;

/// <summary>
/// Основной контроллер для веб интерфейса.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ISettingsStorage _settings;

    public HomeController(ILogger<HomeController> logger, ISettingsStorage settings)
    {
        _logger = logger;
        _settings = settings;
    }

    /// <summary>
    /// Основная страница, отображает выбранные настройки
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Страница политики конфиденциальности.
    /// </summary>
    /// <returns></returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Выбор актуального филиала.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Branch()
    {
        return View();
    }

    /// <summary>
    /// Таблица реадктирования настроек.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Settings(string branchId)
    {
        if (string.IsNullOrEmpty(branchId))
        {
            return RedirectToAction("Branch"); 
        }

        var processedId = Guid.Parse(branchId);
        var branchSettings = _settings.GetSettingsByBranchId(processedId);

        if (branchSettings == null)
        {
            branchSettings = new LoadingSettingsModel 
            { 
                SelectedBranchId = processedId,
                BatchSize = 100
            };
        }
        branchSettings.SelectedBranchId = processedId;        
        return View(branchSettings);
    }

    /// <summary>
    /// Сохранение настроек
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public IActionResult Create(LoadingSettingsModel model)
    {
        // Валидация
        if (!ModelState.IsValid && model.Validate())
        {
            return RedirectToAction("Settings");
        }
    
        // Сохранить модель
        model.Branch = new () { Id = model.SelectedBranchId };
        _settings.SaveLoadingSettings(model); 
        return RedirectToAction("Index");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
