using System;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using JetBrains.Annotations;

namespace F1Desktop.Services;

[UsedImplicitly]
public class NewsRssService
{
    public async Task GetFeed()
    {
        var url = "https://wtf1.com/feed/";
        using var reader = XmlReader.Create(url);
        var feed = SyndicationFeed.Load(reader);

        foreach (var item in feed.Items)
        {
            Console.WriteLine(item.Content);
        }
    }
}