using System.Threading.Tasks;
using F1Desktop.Models.Base;

namespace F1Desktop.Services.Interfaces;

public interface IDataCacheService
{
    public Task<(T cache, bool isValid)> TryGetCacheAsync<T>() where T : CachedDataBase;
    public Task WriteCacheToDisk<T>(T cache) where T : CachedDataBase;
}