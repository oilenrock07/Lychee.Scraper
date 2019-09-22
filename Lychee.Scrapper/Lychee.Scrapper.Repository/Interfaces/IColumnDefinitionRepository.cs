using System.Collections.Generic;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface IColumnDefinitionRepository
    {
        Dictionary<(string, string), string> GetAllScrappedDataColumnDefinitions();
        Dictionary<(string, string), string> GetAllRelatedDataColumnDefinitions();
    }
}
