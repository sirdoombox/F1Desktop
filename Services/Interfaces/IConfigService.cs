using System.Threading.Tasks;
using F1Desktop.Models.Base;

namespace F1Desktop.Services.Interfaces;

public interface IConfigService
{
    public Task<T> TryGetConfigAsync<T>() where T : ConfigBase;
    public Task WriteConfigToDisk<T>(T config) where T : ConfigBase;
}