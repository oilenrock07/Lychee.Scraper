using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Scrapper.Repository.Entities;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;

namespace Lychee.Scrapper.Domain.Helpers
{
    public static class PuppeteerHelper
    {
        public static async Task<string> GetTextboxValue(Page page, string selector)
        {
            JToken value = await page.EvaluateFunctionAsync(@"(selector) => {
                return document.querySelector(selector).value;
            }", selector);

            return value.Value<string>();
        }

        public static async Task<JToken> GetTableData(Page page, string dataSelector, List<ScrapeItemSetting> mappings)
        {
            var tData = await page.EvaluateFunctionAsync(@"(dataSelector, tableMapping) => {
                const data = Array.from(document.querySelectorAll(dataSelector));
                var items = [];
                data.forEach((item, index) => {
                    var obj = {};
                    tableMapping.forEach((i, idx) => {

                        if (i.accessor === 'Html') {
                            obj[i.key] = getHtml($(item), i.selector);
                        }
                        else if (i.accessor === 'Value') {
                            obj[i.key] = getValue($(item), i.selector);
                        }
                        else if (i.accessor === 'Attribute') {
                            obj[i.key] = getAttribute($(item), i.selector, i.attributeName);
                        }
                        else if (i.accessor === 'Text') {
                            obj[i.key] = getText($(item), i.selector);
                        }
                    });
                    items.push(obj);
                });

                return items;
            }", dataSelector, mappings);

            return tData;
        }
    }
}
