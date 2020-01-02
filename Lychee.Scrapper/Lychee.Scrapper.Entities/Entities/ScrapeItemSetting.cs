using System.ComponentModel.DataAnnotations;

namespace Lychee.Scrapper.Entities.Entities
{
    public class ScrapeItemSetting
    {
        [Key]
        public int ScrapeItemSettingId { get; set; }

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
        /// How we want to access the data we want to scrape. See enum ScrappingAccessor
        /// Default accessor is InnerHtml
        /// </summary>
        public string Accessor { get; set; }

        /// <summary>
        /// If Accessor is by Attribute, then we use this property to set the attribute name we want to get
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

        //public virtual ScrapeSetting ScrapeSetting { get; set; }
    }
}
