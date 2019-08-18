using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public abstract class BaseScrapper
    {
        public virtual string Url { get; set; }

        public virtual List<ItemXPath> Items { get; set; }

        public virtual async void LoadPage(string url)
        {
            var html = new HtmlWeb();
            var document = await html.LoadFromWebAsync(url);

            if (document == null)
                throw new ScrapperException("Invalid Url");

            var documentNode = document.DocumentNode;
            var articles = documentNode.QuerySelectorAll("#Users");
        }
    }
}
