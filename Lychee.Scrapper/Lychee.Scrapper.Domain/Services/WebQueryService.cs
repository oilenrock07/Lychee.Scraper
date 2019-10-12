using System.Linq;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Enums;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Web;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Domain.Services
{
    public class WebQueryService : IWebQueryService
    {
        private readonly IHeaderRequestRepository _headerRequestRepository;

        public WebQueryService(IHeaderRequestRepository headerRequestRepository)
        {
            _headerRequestRepository = headerRequestRepository;
        }

        public HtmlDocument GetPage(string url, string method = "GET")
        {
            var webQuery = new WebQuery(url)
            {
                Method = method
            };

            AddCookies(webQuery);

            return webQuery.GetPage();
        }

        protected virtual void AddCookies(WebQuery webQuery)
        {
            var cookies = _headerRequestRepository.GetHeaderRequest(HeaderRequestType.Cookie);
            if (cookies.Any())
            {
                foreach (var headerRequest in cookies)
                {
                    webQuery.AddCookie(headerRequest.Name, headerRequest.Value);
                }
            }
        }
    }
}
