﻿using System;
using System.Text.RegularExpressions;

using TumblThree.Domain.Models;

namespace TumblThree.Applications.Crawler
{
    public class MixtapeParser : IMixtapeParser
    {
        public Regex GetMixtapeUrlRegex()
        {
            return new Regex("(http[A-Za-z0-9_/:.]*mixtape.moe/([A-Za-z0-9_]*))");
        }

        public string CreateMixtapeUrl(string mixtapeId, MixtapeTypes mixtapeType)
        {
            string url;
            switch (mixtapeType)
            {
                case MixtapeTypes.Mp4:
                    url = @"https://my.mixtape.moe/" + mixtapeId + ".mp4";
                    break;
                case MixtapeTypes.Webm:
                    url = @"https://my.mixtape.moe/" + mixtapeId + ".webm";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return url;
        }
    }
}