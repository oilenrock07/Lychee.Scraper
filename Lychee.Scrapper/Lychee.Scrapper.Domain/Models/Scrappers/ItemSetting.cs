namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class ItemSetting
    {
        public bool IsIdentifier { get; set; }
        public string Key { get; set; }
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
    }
}
