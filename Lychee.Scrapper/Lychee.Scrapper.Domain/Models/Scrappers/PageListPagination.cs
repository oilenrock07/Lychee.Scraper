using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class PageListPagination
    {
        public virtual string PaginationXPath { get; set; }

        /// <summary>
        /// If the pagination has ... then it has indefinite pagination
        /// We do not know what is the last page will be unless ShowLastPagination is true
        /// </summary>
        public virtual bool CanHaveIndefinitePagination { get; set; }

        /// <summary>
        /// If we have pagination with indefinite page, we may want to move the last page 
        /// visible and use NextPageXPath to move to next batch of pagination
        /// </summary>
        public virtual string NextPageXPath { get; set; }

        /// <summary>
        /// If the pagination display the last page then we don't care about CanHaveIndefinitePagination
        /// </summary>
        public virtual bool ShowLastPagination { get; set; }
    }
}
