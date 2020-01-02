using Lychee.Scrapper.Entities.Entities;
using System.Collections.Generic;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface IHeaderRequestRepository
    {
        ICollection<HeaderRequest> GetAll();

        List<HeaderRequest> GetHeaderRequest(string category);
    }
}
