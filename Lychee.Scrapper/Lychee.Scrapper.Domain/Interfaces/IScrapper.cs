﻿using System.Collections.Generic;
using HtmlAgilityPack;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IScrapper
    {
        string Url { get; set; }
        List<ScrapeItemSetting> Items { get; set; }
        HtmlNode GetLoadedHtmlNode();
    }
}
