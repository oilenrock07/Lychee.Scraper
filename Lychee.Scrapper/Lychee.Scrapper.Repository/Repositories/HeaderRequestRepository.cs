using System.Collections.Generic;
using System.Linq;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class HeaderRequestRepository : IHeaderRequestRepository
    {
        private List<HeaderRequest> _headerRequests = null;

        public ICollection<HeaderRequest> GetAll()
        {
            if (_headerRequests != null) return _headerRequests;
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<HeaderRequest>(context, true);
                _headerRequests = repository.GetAll().ToList();
            }
            return _headerRequests;
        }

        public List<HeaderRequest> GetHeaderRequest(string category)
        {
            if (_headerRequests == null)
                GetAll();

            return _headerRequests.Where(x => x.Category == category).ToList();
        }
    }
}
