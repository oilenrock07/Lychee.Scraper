﻿using System.Collections.Generic;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface IColumnDefinitionRepository
    {
        Dictionary<(string, string), string> GetAllScrappedDataColumnDefinitions();
        Dictionary<(string, string), string> GetAllRelatedDataColumnDefinitions();
    }
}
