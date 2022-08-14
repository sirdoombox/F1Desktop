﻿using System.Linq.Expressions;
using System.Reflection;
using F1Desktop.Models.Base;
using F1Desktop.Services.Interfaces;
using JetBrains.Annotations;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Base;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class FeatureBaseWithConfig<TConfig> : FeatureBase where TConfig : ConfigBase, new()
{
    protected TConfig Config { get; private set; }
    private readonly IConfigService _configService;

    protected FeatureBaseWithConfig(string displayName, PackIconMaterialKind icon, IConfigService configService, byte order = 0) 
        : base(displayName, icon, order)
    {
        DisplayName = displayName;
        _configService = configService;
    }
    
    protected override async void OnActivate()
    {
        Config = await _configService.GetConfigAsync<TConfig>();
        OnConfigLoaded();
        OnActivationComplete();
    }
    
    protected virtual bool SetAndNotifyWithConfig<T1>(ref T1 field, Expression<Func<TConfig,T1>> propExpr, T1 value, string propertyName = "")
    {
        if (!SetAndNotify(ref field, value, propertyName)) return false;
        var expr = (MemberExpression) propExpr.Body;
        var prop = (PropertyInfo) expr.Member;
        prop.SetValue(Config, value, null);
        return true;
    }
    
    protected override async void OnClose() => 
        await _configService.WriteConfigToDiskAsync<TConfig>();
    
    protected override async void OnFeatureHidden() => 
        await _configService.WriteConfigToDiskAsync<TConfig>();

    protected virtual void OnConfigLoaded() {}
    protected virtual void OnActivationComplete() {}
}