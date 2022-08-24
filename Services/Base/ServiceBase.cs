using JetBrains.Annotations;
using Serilog;
using StyletIoC;

namespace F1Desktop.Services.Base;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class ServiceBase : IInjectionAware
{
    [Inject]
    protected ILogger Logger { get; set; }
    
    public void ParametersInjected() => OnPropertiesInjected();

    protected virtual void OnPropertiesInjected()
    {
    }
}