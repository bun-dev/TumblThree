using System;
using System.Text.RegularExpressions;
using System.Linq;

using TumblThree.Domain.Models;

namespace TumblThree.Applications.Crawler
{
    public class WebmshareParser : IWebmshareParser
    {
        public Regex GetWebmshareUrlRegex()
        {
            return new Regex("(http[A-Za-z0-9_/:.]*webmshare.com/([A-Za-z0-9_]*))");
        }

        public string CreateWebmshareUrl(string webshareId, string fullurl,WebmshareTypes webmshareType)
        {
            string url="";
            switch (webmshareType)
            {
                case WebmshareTypes.Mp4:
                    url = @"https://s1.webmshare.com/f/" + webshareId + ".mp4";
                    break;
                case WebmshareTypes.Webm:
                    url = @"https://s1.webmshare.com/" + webshareId + ".webm";
                    break;
				case WebmshareTypes.Any:
					url = fullurl;
					
					break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return url;
        }
    }
}