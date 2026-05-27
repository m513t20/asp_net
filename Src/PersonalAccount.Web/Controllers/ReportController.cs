using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using PersonalAccount.Data;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using PersonalAccount.Web.Enums;
using PersonalAccount.Web.Interfaces;
using PersonalAccount.Web.Models;

namespace PersonalAccount.Web.Controllers
{
    /// <summary>
    /// Контроллер по функционалу отчетов.
    /// </summary>
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IMemoryCache _cache;
        private readonly PersonalAccountContext _context;

        public ReportController(IReportService reportService, IMemoryCache cache, PersonalAccountContext context)
        {
            _reportService = reportService;
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// Первое открытие, получает доступные ветки и вадыет их на выборю
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var model = new ReportFilterViewModel
            {
                Branches = _context.Branches.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                }).ToList()
            };
            return View(model);
        }

        /// <summary>
        /// Формирование отчета.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(ReportFilterViewModel model)
        {
            model.Branches = _context.Branches.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                }).ToList();

            var startUtc = DateTime.SpecifyKind(model.Start, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(model.End, DateTimeKind.Utc);

            // ключ для кжша
            string cacheKey = $"transactions_{model.BranchId}_{startUtc:yyyyMMdd}_{endUtc:yyyyMMdd}";

            // проверяем закеширован ли уже такой запрос
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<TransactionModel> transactions))
            {
                transactions = _reportService.Get(model.BranchId, startUtc, endUtc);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                
                _cache.Set(cacheKey, transactions, cacheOptions);
            }

            if (transactions != null && transactions.Any())
            {
                var reportData = _reportService.Create(transactions, model.ReportType);

                switch (model.ReportType)
                {
                    case ReportTypeEnum.Revenue:
                        model.RevenueReport = reportData.Cast<RevenueDto>();
                        break;
                    case ReportTypeEnum.Selling:
                        model.SellingReport = reportData.Cast<SellingDto>();
                        break;
                    case ReportTypeEnum.WorkSchedule:
                        model.WorkScheduleReport = reportData.Cast<WorkScheduleDto>();
                        break;
                }
            }

            return View(model);
        }
    }
}
