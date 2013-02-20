using System;
using System.Net;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevTyr.Gullap.Rss2Gullap
{
	public static class RssReceiver
	{
		public static SyndicationFeed LoadRss (Uri uri)
		{
			Rss20FeedFormatter feedFormatter = new Rss20FeedFormatter();
			XmlReader rssReader = XmlReader.Create(uri.OriginalString);
			if (feedFormatter.CanRead(rssReader))
			{
				feedFormatter.ReadFrom(rssReader);
				rssReader.Close();
			}

			return feedFormatter.Feed;
		}
	}
}

