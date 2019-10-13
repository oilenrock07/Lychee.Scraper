namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class SmartScrapperTableObjectMapping
    {
        public string ColumnName { get; set; }
        public int Index { get; set; }
        public string Accessor { get; set; }
        public string ElementSelector { get; set; }
        public string ElementAttribute { get; set; }
    }
}
