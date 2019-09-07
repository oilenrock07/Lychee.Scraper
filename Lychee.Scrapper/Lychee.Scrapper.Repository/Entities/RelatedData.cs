using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lychee.Scrapper.Repository.Entities
{
    [Table("RelatedData")]
    public class RelatedData
    {
        public int RelatedDataId { get; set; }
        public int ScrappedDataId { get; set; }
        public string Description { get; set; }

        //Strings
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public string String5 { get; set; }
        
        //Decimals
        public decimal Decimal1 { get; set; }
        public decimal Decimal2 { get; set; }
        public decimal Decimal3 { get; set; }
        public decimal Decimal4 { get; set; }
        public decimal Decimal5 { get; set; }
       
        //Ints
        public int Int1 { get; set; }
        public int Int2 { get; set; }
        public int Int3 { get; set; }
        public int Int4 { get; set; }
        public int Int5 { get; set; }
        
        //Dates
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public DateTime Date3 { get; set; }
        public DateTime Date4 { get; set; }
        public DateTime Date5 { get; set; }
       
        //Bools
        public bool Bool1 { get; set; }
        public bool Bool2 { get; set; }
        public bool Bool3 { get; set; }
        public bool Bool4 { get; set; }
        public bool Bool5 { get; set; }
       
    }
}
