using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class SmartInstruction
    {        
        public const string EVENT_SET_TEXT = "SET_TEXT";
        public const string EVENT_CLICK = "CLICK";

        public virtual string SmartElementName { get; set; }
        public virtual string Event { get; set; }
        public virtual string Value { get; set; }
    }
}
