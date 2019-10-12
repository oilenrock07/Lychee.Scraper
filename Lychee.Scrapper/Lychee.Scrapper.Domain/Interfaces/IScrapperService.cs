using System.Threading.Tasks;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IScrapperService
    {
        Task Scrape();
    }
}
