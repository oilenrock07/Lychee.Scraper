using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Models.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public abstract class BaseScrapper
    {
        public virtual string Url { get; set; }

        public virtual List<ItemSetting> Items { get; set; }

        public virtual async Task<HtmlNode> LoadPage(string url)
        {
            var html = new HtmlWeb();
            var document = await html.LoadFromWebAsync(url);

            if (document == null)
                throw new ScrapperException("Invalid Url");

            return document.DocumentNode;      
        }

        public abstract Task<ResultCollection<ResultItemCollection>> Scrape();
        
        protected virtual void AddMultipleValues(HtmlNode node, ItemSetting item, List<ResultItem> list)
        {
            var itemNodes = node.QuerySelectorAll(item.Selector).ToList();
            if (itemNodes.Any())
            {
                foreach (var itemNode in itemNodes)
                {
                    if (!string.IsNullOrEmpty(item.AttributeName))
                    {
                        var value = GetByAttribute(itemNode, item);
                        list.Add(new ResultItem
                        {
                            Name = item.Key,
                            Value = value,
                            IsMultiple = true
                        });
                    }
                    else
                    {
                        var value = GetByInnerHtml(itemNode, item);
                        list.Add(new ResultItem
                        {
                            Name = item.Key,
                            Value = !string.IsNullOrEmpty(value) ? value : item.DefaultValue,
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
        /// <param name="item"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        protected virtual string AddSingleValue(HtmlNode node, ItemSetting item, List<ResultItem> list)
        {
            string value;
            var itemNode = node.QuerySelector(item.Selector);
            if (!string.IsNullOrEmpty(item.AttributeName))
            {
                value = GetByAttribute(itemNode, item);
                list.Add(new ResultItem
                {
                    Name = item.Key,
                    Value = value
                });
            }
            else
            {
                value = GetByInnerHtml(itemNode, item);
                list.Add(new ResultItem
                {
                    Name = item.Key,
                    Value = !string.IsNullOrEmpty(value) ? value : item.DefaultValue
                });
            }

            return value;
        }

        protected virtual string GetByAttribute(HtmlNode itemNode, ItemSetting item)
        {
            if (item.ValueRequired && !itemNode.HasAttributes)
                throw new ScrapperException($"Attribute {item.Key} does not exists");

            return itemNode.GetAttributeValue(item.AttributeName, item.DefaultValue);
        }

        protected virtual string GetByInnerHtml(HtmlNode itemNode, ItemSetting item)
        {
            var value = itemNode.InnerHtml;
            if (item.ValueRequired && string.IsNullOrEmpty(value))
                throw new ScrapperException($"Value {item.Key} does not exists");

            return !string.IsNullOrEmpty(value) ? value : item.DefaultValue;
        }
    }
}
