using System.Collections.Generic;
using System.Linq;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class ColumnDefinitionRepository : IColumnDefinitionRepository
    {
        /// <summary>
        /// (tablename, definition), columnname
        /// example. ("ScrappedData", "Image"), "String1"
        /// </summary>
        /// <returns></returns>
        public Dictionary<(string, string), string> GetAllScrappedDataColumnDefinitions()
        {
            var result = new Dictionary<(string, string), string>();
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ColumnDefinition>(context);
                var columns = repository.Find(x => x.TableName == nameof(ScrappedData)).ToList();

                if (!columns.Any()) return result;

                foreach (var column in columns)
                    result.Add((column.TableName, column.Definition), column.ColumnName);
            }

            return result;
        }

        /// <summary>
        /// (tablename, columnname), definition
        /// example. ("ScrappedData", "String1"), "Image"
        /// </summary>
        /// <returns></returns>
        public Dictionary<(string, string), string> GetAllRelatedDataColumnDefinitions()
        {
            var result = new Dictionary<(string, string), string>();
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ColumnDefinition>(context);
                var columns = repository.Find(x => x.TableName == nameof(RelatedData)).ToList();

                if (!columns.Any()) return result;

                foreach (var column in columns)
                    result.Add((column.TableName, column.ColumnName), column.Definition);
            }

            return result;
        }
    }
}
