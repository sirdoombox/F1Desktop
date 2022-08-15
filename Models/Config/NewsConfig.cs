﻿using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.Config;

[Filename("News.cfg")]
public class NewsConfig : ConfigBase
{
    public Dictionary<string,bool> Providers { get; set; }

    public NewsConfig()
    {
        Providers = new Dictionary<string, bool>();
    }
}