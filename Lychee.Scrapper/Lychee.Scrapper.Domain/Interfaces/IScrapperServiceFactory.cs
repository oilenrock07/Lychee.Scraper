using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IScrapperServiceFactory
    {
        IScrapperService GetScrapper();
    }
}
