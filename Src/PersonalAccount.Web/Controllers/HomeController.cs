using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Common.Core;
using PersonalAccount.Web.Models;

namespace PersonalAccount.Web.Controllers;

public class HomeController(IBranchRepository branchRepository) : Controller
{
    // Репозиторий для работы с филиалами
    private readonly IBranchRepository _branchRepository = branchRepository;
    
    /// <summary>
    /// Настройки
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        var branches = _branchRepository.GetBranches().ToList();
        var viewModel = new BranchSettingsModel() 
        { 
            Branches = branches,
            Branch = branches.First()
        };
        return View(viewModel);
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

        _branchRepository.Update(model.Branch);    
        return View(model);
    }
}
