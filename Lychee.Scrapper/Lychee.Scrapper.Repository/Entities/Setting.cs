namespace Lychee.Scrapper.Repository.Entities
{
    public class Setting
    {
        public int SettingId { get; set; }

        //naming convention: Core.Schema.IsMultipleRow
        public string Key { get; set; }
        public string Value { get; set; }

        public string Description { get; set; }
    }
}
