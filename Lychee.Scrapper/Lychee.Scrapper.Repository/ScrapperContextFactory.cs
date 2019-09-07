using System.Data.Entity;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository
{
    public class ScrapperContextFactory : IContextFactory
    {
        private DbContext _context;
        public DbContext GetContext()
        {
            return _context ?? (_context = new ScrapperContext());
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
