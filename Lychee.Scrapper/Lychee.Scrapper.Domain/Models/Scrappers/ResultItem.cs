namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class ResultItem
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public bool IsMultiple { get; set; }

        public string Group { get; set; }
    }
}
