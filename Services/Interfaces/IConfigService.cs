using System.Threading.Tasks;
using F1Desktop.Models.Base;

namespace F1Desktop.Services.Interfaces;

public interface IConfigService
{
    public Task<T> GetConfigAsync<T>() where T : ConfigBase, new();
    public Task WriteConfigToDiskAsync<T>() where T : ConfigBase;
}