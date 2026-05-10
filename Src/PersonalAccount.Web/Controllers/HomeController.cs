using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Domain.Models;
using PersonalAccount.Web.Models;

namespace PersonalAccount.Web.Controllers;

public class HomeController : Controller
{
    /// <summary>
    /// Настройки
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        return View(new BranchSettingsModel() { Branches = new List<BranchModel>()});
    }

    /// <summary>
    /// Продажи
    /// </summary>
    /// <returns></returns>
    public IActionResult SallingReport()
    {
        return View();
    }

    /// <summary>
    /// Выручка
    /// </summary>
    /// <returns></returns>
    public IActionResult RevenueReport()
    {
        return View();
    }

    /// <summary>
    /// График работы
    /// </summary>
    /// <returns></returns>
    public IActionResult WorkScheduleReport()
    {
        return View();
    }

    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult SaveSettings(BranchSettingsModel model)
    {
        var validate = model.Branch.Validate();
        if(!validate)
            throw new InvalidDataException($"Некорректно указаны параметры!\n{model.Branch.ErrorText}");

        return View(model);
    }
}
