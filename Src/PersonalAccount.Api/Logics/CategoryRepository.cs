using System;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// 
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    public IEnumerable<CategoryModel>? Get(BufferKey key)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CategoryModel>?> GetAsync(BufferKey key, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CategoryModel> GetFreshRows(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CategoryModel>> GetFreshRowsAsync(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public bool Save(IEnumerable<CategoryModel> categories)
    {
        throw new NotImplementedException();
    }

    public void Save(BufferKey key, IEnumerable<CategoryModel> values)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAsync(IEnumerable<CategoryModel> categories, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(BufferKey key, IEnumerable<CategoryModel> values, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
