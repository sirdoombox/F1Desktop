using System.Linq.Expressions;
using System.Reflection;
using F1Desktop.Models.Base;
using F1Desktop.Services;
using JetBrains.Annotations;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Base;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class FeatureBaseWithConfig<T> : FeatureBase where T : ConfigBase, new()
{
    protected T Config { get; private set; }
    private readonly ConfigService _configService;

    protected FeatureBaseWithConfig(string displayName, PackIconMaterialKind icon, ConfigService configService, byte order = 0) 
        : base(displayName, icon, order)
    {
        DisplayName = displayName;
        _configService = configService;
    }
    
    protected override async void OnActivate()
    {
        Config = await _configService.GetConfigAsync<T>();
        OnConfigLoaded();
        OnActivationComplete();
    }

    protected override async void OnClose()
    {
        await _configService.WriteConfigAsync<T>();
    }

    protected virtual bool SetAndNotifyWithConfig<T1>(ref T1 field, Expression<Func<T,T1>> propExpr, T1 value, string propertyName = "")
    {
        var hasChanged = SetAndNotify(ref field, value, propertyName);
        if (!hasChanged) return false;
        var expr = (MemberExpression) propExpr.Body;
        var prop = (PropertyInfo) expr.Member;
        prop.SetValue(Config, value, null);
        return true;
    }

    public override async void OnFeatureHidden()
    {
        await _configService.WriteConfigAsync<T>();
    }

    protected virtual void OnConfigLoaded() {}
    protected virtual void OnActivationComplete() {}
}