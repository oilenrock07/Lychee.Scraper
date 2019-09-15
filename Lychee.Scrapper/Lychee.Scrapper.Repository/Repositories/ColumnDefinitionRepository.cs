using System.Collections.Generic;
using System.Linq;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class ColumnDefinitionRepository : IColumnDefinitionRepository
    {
        public Dictionary<(string, string), string> GetAllColumnDefinitions()
        {
            var result = new Dictionary<(string, string), string>();
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ColumnDefinition>(context);
                var columns = repository.GetAll().ToList();

                if (!columns.Any()) return result;

                foreach (var column in columns)
                    result.Add((column.TableName, column.Definition), column.ColumnName);
            }

            return result;
        }
    }
}
