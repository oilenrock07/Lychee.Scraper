using System;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Models.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Web;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public abstract class BaseScrapper
    {
        private readonly IWebQueryService _webQueryService;
        public virtual string Url { get; set; }

        public virtual List<ScrapeItemSetting> Items { get; set; }

        protected BaseScrapper(IWebQueryService webQueryService)
        {
            _webQueryService = webQueryService;
        }

        /// <summary>
        /// Basic loading of website's page. This assumes that all the data from the website are public. If you want more complex scrapping setup. Use AdvancedLoadPage
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual async Task<HtmlNode> LoadPage(string url)
        {
            var html = new HtmlWeb();
            var document = await html.LoadFromWebAsync(url);

            if (document == null)
                throw new ScrapperException("Invalid Url");

            return document.DocumentNode;      
        }

        /// <summary>
        /// If the page you are trying to load require cookies from the request header, use this.
        /// </summary>
        /// <returns></returns>
        public virtual HtmlNode AdvancedLoadPage()
        {
            var document = _webQueryService.GetPage(Url);

            if (document == null)
                throw  new ScrapperException("Error making request. Please check all the passed parameters");

            return document.DocumentNode;
        }

        public abstract Task<ResultCollection<ResultItemCollection>> Scrape();
        
        protected virtual void AddMultipleValues(HtmlNode node, ScrapeItemSetting scrapeItem, List<ResultItem> list)
        {
            var itemNodes = node.QuerySelectorAll(scrapeItem.Selector).ToList();
            if (itemNodes.Any())
            {
                foreach (var itemNode in itemNodes)
                {
                    if (!string.IsNullOrEmpty(scrapeItem.AttributeName))
                    {
                        var value = GetByAttribute(itemNode, scrapeItem);
                        list.Add(new ResultItem
                        {
                            Name = scrapeItem.Key,
                            Value = value,
                            IsMultiple = true
                        });
                    }
                    else
                    {
                        var value = GetByInnerHtml(itemNode, scrapeItem);
                        list.Add(new ResultItem
                        {
                            Name = scrapeItem.Key,
                            Value = !string.IsNullOrEmpty(value) ? value : scrapeItem.DefaultValue,
                            IsMultiple = true
                        });
                    }
                        
                }
            }
        }

        /// <summary>
        /// Single Value could potentially contains the "key" value, so we return the value that we add here
        /// </summary>
        /// <param name="node"></param>
        /// <param name="scrapeItem"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        protected virtual string AddSingleValue(HtmlNode node, ScrapeItemSetting scrapeItem, List<ResultItem> list)
        {
            string value;
            var itemNode = node.QuerySelector(scrapeItem.Selector);
            if (!string.IsNullOrEmpty(scrapeItem.AttributeName))
            {
                value = GetByAttribute(itemNode, scrapeItem);
                list.Add(new ResultItem
                {
                    Name = scrapeItem.Key,
                    Value = value
                });
            }
            else
            {
                value = GetByInnerHtml(itemNode, scrapeItem);
                list.Add(new ResultItem
                {
                    Name = scrapeItem.Key,
                    Value = !string.IsNullOrEmpty(value) ? value : scrapeItem.DefaultValue
                });
            }

            return value;
        }

        protected virtual string GetByAttribute(HtmlNode itemNode, ScrapeItemSetting scrapeItem)
        {
            if (scrapeItem.ValueRequired && !itemNode.HasAttributes)
                throw new ScrapperException($"Attribute {scrapeItem.Key} does not exists");

            return itemNode.GetAttributeValue(scrapeItem.AttributeName, scrapeItem.DefaultValue);
        }

        protected virtual string GetByInnerHtml(HtmlNode itemNode, ScrapeItemSetting scrapeItem)
        {
            var value = itemNode.InnerHtml;
            if (scrapeItem.ValueRequired && string.IsNullOrEmpty(value))
                throw new ScrapperException($"Value {scrapeItem.Key} does not exists");

            return !string.IsNullOrEmpty(value) ? value : scrapeItem.DefaultValue;
        }

        //protected virtual string GetByValue(HtmlNode itemNode, ScrapeItemSetting scrapeItem)
        //{
        //    var value = itemNode;
        //    if (scrapeItem.ValueRequired && string.IsNullOrEmpty(value))
        //        throw new ScrapperException($"Value {scrapeItem.Key} does not exists");

        //    return !string.IsNullOrEmpty(value) ? value : scrapeItem.DefaultValue;
        //}
    }
}
