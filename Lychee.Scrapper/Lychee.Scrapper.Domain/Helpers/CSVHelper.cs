using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using CsvHelper;
using Lychee.Scrapper.Domain.Models.Scrappers;

namespace Lychee.Scrapper.Domain.Helpers
{
    public static class CSVHelper
    {
        public static void WriteObject(string filename, ResultCollection<ResultItemCollection> result)
        {
            var data = PrepareData(result);
            using (var writer = new StreamWriter(filename))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(data);
            }
        }

        private static List<dynamic> PrepareData(ResultCollection<ResultItemCollection> result)
        {
            var records = new List<dynamic>();
            foreach (var resultItem in result)
            {
                var record = new ExpandoObject() as IDictionary<string, object>;
                foreach (var item in resultItem.Items)
                {
                    record.Add(item.Name, item.Value);
                }
                records.Add(record);
            }

            return records;
        }
    }
}
