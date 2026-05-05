using System.Text.Json;
using PersonalAccount.Common.Core;
using PersonalAccount.Data;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using Serilog;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Реализация репозитория <see cref="ITransactionRepository"/>
/// </summary>
public class TransactionRepository(
     IBufferedRepository<JournalRowDto, CategoryModel> categoryRepository,
     IBufferedRepository<JournalRowDto, EmploeeModel> emploeeRepository,
     IBufferedRepository<JournalRowDto, NomenclatureModel> nomenclaturePorository,
     PersonalAccountContext context
    ) : ITransactionRepository
{
    // Репозиторий для работы с категориями
    private readonly IBufferedRepository<JournalRowDto, CategoryModel> _categoryRepository = categoryRepository;

    // Репозиторий для работы с сотрудниками
    private readonly IBufferedRepository<JournalRowDto, EmploeeModel> _emploeeRepository = emploeeRepository;

    // Репозиторий для работы с номенклатурой
    private readonly IBufferedRepository<JournalRowDto, NomenclatureModel> _nomenclatureRepository = nomenclaturePorository;

    // Контекст для работы с базой данных
    private readonly PersonalAccountContext _context = context;


    /// <inheritdoc/>
    public bool Push(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options)
    {
        try
        {
            // Выделяю и сохраняю составляющие
            _categoryRepository.Save(
                _categoryRepository.GetRows(transactions, options), options ); 
            _emploeeRepository.Save(
                _emploeeRepository.GetRows(transactions, options), options );
            _nomenclatureRepository.Save(
                _nomenclatureRepository.GetRows(transactions, options), options);


            // Составляющие пачки
            var emplooes = _emploeeRepository.Get( new BufferKey(options, typeof(EmploeeModel)) );
            var nomenclatures = _nomenclatureRepository.Get( new BufferKey(options, typeof(NomenclatureModel)) );

            // Формируем список транзакций
            var rows = transactions.Select( x => new Transaction()
            {
                TransactionType = (int)x.TypeCode,
                ChangePeriod = x.Period.ToUniversalTime(),
                NomenclatureId = nomenclatures?.FirstOrDefault( y => y.ExternalCode == x.ProductCode)?.Id,
                EmloeeId = emplooes?.FirstOrDefault( y => y.ExternalCode == x.EmploeeCode)?.Id,
                Price = (decimal)x.Price,
                Quantity = (decimal)x.Quantity,
                Discount = (decimal)x.Discount,
                BranchId = options.Branch.Id,
                ExternalCode = x.Code
            }).ToList();

            // Записываем
            _context.Transactions.AddRange(rows);
            _context.SaveChanges();

            return true;
        }
        catch(Exception ex)
        {
            Log.Error($"Невозможно обработать пачку данных: \n{JsonSerializer.Serialize(options)}\n{ex.Message}{ex.InnerException?.Message}");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> PushAsync(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options, CancellationToken token)
        => await Task.Run( () => Push(transactions, options), token);
}
