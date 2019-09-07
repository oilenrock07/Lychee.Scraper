using System.Data.Entity;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface IContextFactory
    {
        DbContext GetContext();
        void Dispose();
    }
}
