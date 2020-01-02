using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Scrapper.Entities.Entities;
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
                        setObject(obj, i, item);
                    });
                    items.push(obj);
                });

                return items;
            }", dataSelector, mappings);

            return tData;
        }

        public static async Task<JToken> GetElements(Page page, List<ScrapeItemSetting> mappings)
        {
            var tData = await page.EvaluateFunctionAsync(@"(mappings) => {
                var items = [];

                mappings.forEach((i, idx) => {
                    var obj = {};
                    var element = $(document);
                    setObject(obj, i, element);
                    items.push(obj);
                });

                return items;
            }", mappings);

            return tData;
        }
    }
}
