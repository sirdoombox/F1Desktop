using System;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using F1Desktop.Misc;
using JetBrains.Annotations;

namespace F1Desktop.Services;

[UsedImplicitly]
public class NewsRssService
{
    public Task<SyndicationFeed> GetFeedAsync()
    {
        return Task.Run(GetFeed);
    }

    public Task<IEnumerable<SyndicationFeed>> GetFeedsAsync()
    {
        return Task.Run(GetFeeds);
    }

    private SyndicationFeed GetFeed()
    {
        var url = "https://wtf1.com/feed/";
        using var reader = XmlReader.Create(url);
        return SyndicationFeed.Load(reader);
    }

    private IEnumerable<SyndicationFeed> GetFeeds()
    {
        var feeds = new List<SyndicationFeed>();
        foreach (var item in Constants.BaseNewsFeeds)
        {
            using var reader = XmlReader.Create(item.Value);
            feeds.Add(SyndicationFeed.Load(reader));
        }

        return feeds;
    }
}