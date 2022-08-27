using System.Linq.Expressions;
using System.Reflection;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Settings.Settings.Base;

public abstract class SetingsCategoryViewModelBase : PropertyChangedBase
{
    public string Name { get; }
    protected GlobalConfigService Config { get; }

    protected SetingsCategoryViewModelBase(string name, GlobalConfigService config)
    {
        Name = name;
        Config = config;
        Config.OnPropertyChanged += OnGlobalConfigPropertyChanged;
        // ReSharper disable once VirtualMemberCallInConstructor
        OnGlobalConfigPropertyChanged(string.Empty);
    }
    
    protected virtual bool SetAndNotifyWithConfig<T1>(ref T1 field, Expression<Func<GlobalConfigService, T1>> propExpr, 
        T1 value, 
        string propertyName = "")
    {
        if (!SetAndNotify(ref field, value, propertyName)) return false;
        var expr = (MemberExpression)propExpr.Body;
        var prop = (PropertyInfo)expr.Member;
        prop.SetValue(Config, value, null);
        return true;
    }

    protected abstract void OnGlobalConfigPropertyChanged(string propName);
}