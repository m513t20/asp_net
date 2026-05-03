using Microsoft.EntityFrameworkCore;
using PersonalAccount.Common.Core;
using PersonalAccount.Data;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Реализация интерфейса <see cref="ILoadingFromClientService"/>
/// </summary>
public class LoadingFromClientService(
        PersonalAccountContext context,
        ICompanySettingsRepository settingsRepository,
        IServerRepository<JournalRowDto> writerRepository) : ILoadingFromClientService
{
    // Репозиторий для работы с настройками загрузки данных
    private readonly  ICompanySettingsRepository _settingReposity = settingsRepository;

    // Репозиторий для скоростной записи данных в журнал
    private readonly IServerRepository<JournalRowDto> _writerRepository = writerRepository;

    // Контекст для работы с базой данных
    private readonly PersonalAccountContext _context = context;


    /// <InhericDoc/>
    private bool Push(BranchModel branch, IEnumerable<JournalRowDto> transactions)
    {
        // 1 Получаем настройки
        var settings = _settingReposity.Load( branch ) 
                        ?? new LoadingSettingsModel()
                        {
                            Branch = branch, StartPosition = 1, BatchSize = 1000
                        };

        var firstTransaction = transactions.FirstOrDefault();
        if(firstTransaction is null) return false;
        
        // Отбрасываем лишние
        var innerTransactions = transactions.Where(x => x.Code >= settings.StartPosition);

        // Сохраняем 
        var connect = _context.Database.GetDbConnection();
        var task = _writerRepository.SaveRows(connect, innerTransactions, settings );
        
        // Обновляем настройки
        var lastCode = innerTransactions.OrderByDescending(x => x.Code).First().Code;
        settings.StartPosition = lastCode;
        _settingReposity.Save( settings );

        Task.WaitAll( task );
        return true;
    }

    /// <InhericDoc/>
    public bool Push(Guid branchId, IEnumerable<JournalRowDto> transactions)
    {
        var branch = _context.Branches.FirstOrDefault( x => x.Id == branchId) ?? throw new InvalidOperationException($"Невозможно получить карточку филиала по коду {branchId}!");
        return Push(new BranchModel() { Id = branchId}, transactions);
    }

    /// <InhericDoc/>
    public async Task<bool> PushAsync(Guid branchId, IEnumerable<JournalRowDto> transactions, CancellationToken token)
        => await Task.Run( () => Push( branchId, transactions), token);

    /// <InhericDoc/>
    public LoadingSettingsModel GetSettings(Guid branchId)
    {
        var entity = _context.Branches.FirstOrDefault( x => x.Id ==  branchId) ?? throw new InvalidOperationException($"Невозможно получить карточку филиала по коду {branchId}!");
        
        // Конвертируем в модель
        var branch = new BranchModel()
        { 
            Id = branchId, 
            Name = entity.Name ?? string.Empty,
            Owner = new CompanyModel()
            {
                Id = entity.CompanyId
            }
        };

        // Внимание! Для обновления настроек необходимо в базе данных выполнить SQL запрос
        // update branches set load_options = null
        // Тогда мы получим пустые настройки и заново их свормируем
        var settings = _settingReposity.Load( branch ) ;

        if (settings is null)
        {
            // Сформируем новый набор настроек по умолчанию
            settings = new LoadingSettingsModel()  {  Branch = branch, StartPosition = 1, BatchSize = 1000   };
            _settingReposity.Save( settings );            
        }

        return settings;
    }

    /// <InhericDoc/>
    public async Task<LoadingSettingsModel> GetSettingsAsync(Guid companyId, CancellationToken token)
        => await Task.Run( () => GetSettings(companyId), token);
}
