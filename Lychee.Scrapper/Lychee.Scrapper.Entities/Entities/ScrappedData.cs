using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lychee.Scrapper.Entities.Entities
{
    [Table("ScrappedData")]
    public class ScrappedData
    {
        public int ScrappedDataId { get; set; }
        public string Identifier { get; set; }

        //Strings
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public string String5 { get; set; }
        public string String6 { get; set; }
        public string String7 { get; set; }
        public string String8 { get; set; }
        public string String9 { get; set; }
        public string String10 { get; set; }
        public string String11 { get; set; }
        public string String12 { get; set; }
        public string String13 { get; set; }
        public string String14 { get; set; }
        public string String15 { get; set; }
        public string String16 { get; set; }
        public string String17 { get; set; }
        public string String18 { get; set; }
        public string String19 { get; set; }
        public string String20 { get; set; }


        //Decimals
        public decimal Decimal1 { get; set; }
        public decimal Decimal2 { get; set; }
        public decimal Decimal3 { get; set; }
        public decimal Decimal4 { get; set; }
        public decimal Decimal5 { get; set; }
        public decimal Decimal6 { get; set; }
        public decimal Decimal7 { get; set; }
        public decimal Decimal8 { get; set; }
        public decimal Decimal9 { get; set; }
        public decimal Decimal10 { get; set; }

        //Ints
        public int Int1 { get; set; }
        public int Int2 { get; set; }
        public int Int3 { get; set; }
        public int Int4 { get; set; }
        public int Int5 { get; set; }
        public int Int6 { get; set; }
        public int Int7 { get; set; }
        public int Int8 { get; set; }
        public int Int9 { get; set; }
        public int Int10 { get; set; }


        //Dates
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public DateTime Date3 { get; set; }
        public DateTime Date4 { get; set; }
        public DateTime Date5 { get; set; }
        public DateTime Date6 { get; set; }
        public DateTime Date7 { get; set; }
        public DateTime Date8 { get; set; }
        public DateTime Date9 { get; set; }
        public DateTime Date10 { get; set; }


        //Bools
        public bool Bool1 { get; set; }
        public bool Bool2 { get; set; }
        public bool Bool3 { get; set; }
        public bool Bool4 { get; set; }
        public bool Bool5 { get; set; }
        public bool Bool6 { get; set; }
        public bool Bool7 { get; set; }
        public bool Bool8 { get; set; }
        public bool Bool9 { get; set; }
        public bool Bool10 { get; set; }

        public virtual ICollection<RelatedData> RelatedData { get; set; }
    }
}
