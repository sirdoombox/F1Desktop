using System;

namespace F1Desktop.Models.Base;

public abstract class CachedDataBase
{
    public DateTimeOffset CacheTime { get; set; }
}