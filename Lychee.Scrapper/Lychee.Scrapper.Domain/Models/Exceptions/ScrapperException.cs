using System;

namespace Lychee.Scrapper.Domain.Models.Exceptions
{
    public class ScrapperException : System.Exception
    {
        public ScrapperException()
        {

        }
        public ScrapperException(string message) : base(message)
        {

        }
    }
}
