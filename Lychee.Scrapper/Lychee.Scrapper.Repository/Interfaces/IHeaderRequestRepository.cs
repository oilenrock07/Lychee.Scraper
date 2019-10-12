using System.Collections.Generic;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface IHeaderRequestRepository
    {
        ICollection<HeaderRequest> GetAll();

        List<HeaderRequest> GetHeaderRequest(string category);
    }
}
