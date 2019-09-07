﻿namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class ItemSetting
    {
        /// <summary>
        /// If the key is the identifier of the row, then set this to true.
        /// Example: Key = Id/ProductName/GUID/Url
        /// Ideally 1 row should have 1 identifier
        /// </summary>
        public bool IsIdentifier { get; set; }

        /// <summary>
        /// Name of the item you want to scrape. Example productname, image, price
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Jquery/css selector of the item value
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// If AttributeName is present, we try to parse it from attribute, otherwise we get the inner html
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// If value is not required you can specify the default value if parsed value is null or empty
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Throws exception if value from attribute/inner html does not exists
        /// </summary>
        public bool ValueRequired { get; set; }

        /// <summary>
        /// If you are expecting multiple value from the selector, you can set this to true
        /// </summary>
        public bool MultipleValue { get; set; }

        /// <summary>
        /// This is used for exporting the data to CSV.
        /// This will become the header of your data
        /// Example if the Images is expected to have multiple values and you set this to 3
        /// The columns will be Image1, Image2, Image3
        /// </summary>
        public int EstimatedMultipleValueCount { get; set; }
    }
}
