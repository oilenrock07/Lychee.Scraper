using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Enums;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;
using Moq;
using NUnit.Framework;

namespace Lychee.Scrapper.Test.ServiceTest
{
    [TestFixture]
    public class WebQueryServiceTest
    {
        private WebQueryService _webQueryService;
        private Mock<IHeaderRequestRepository> _headerRequestRepository;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _headerRequestRepository = new Mock<IHeaderRequestRepository>();

            _webQueryService = new WebQueryService(_headerRequestRepository.Object);
        }

        [Test]
        //[Ignore("MySql Service needs to be running or else this will fail. This is also highly dependent on the cookie")]
        public void CanLoadPageWithCredentialsRequired()
        {
            //Arrange
            _headerRequestRepository.Setup(x => x.GetHeaderRequest(HeaderRequestType.Cookie)).Returns(new List<HeaderRequest>
            {
                new HeaderRequest { Category = HeaderRequestType.Cookie, Name = ".AspNet.ApplicationCookie", Value = "FnilTTTvqPOprOsZV-BoyctK7Av1WSPTwWRxjpM3uLF2zgvbwhjB1tZjJMXG4QFN6TSVBJS0J1NFr0U1P7D21Yj77pw7D4iyob_n60sGMPUELH_C63FlLxT4VlAn68ejs5rc3U0SMPcLHQrlPW-L12wX_RBqVckS7YLBqE9S6UVHLGTfqbymFxVMaismDJcd3ewTC-BOuo4wjB1Bns9EUbshUXhNUNmSFJtgw8soGHwsSuPPe3t4IBRHXxc2qeIyVNoB4m1rYXuFge6XtodBoMhGiDq_laCB5spoh7XI5UZC4hKDMfF4ms0r0wZSS-EXMmzy-vACIxR8XB8u9MH9If2M0l99B_AwfXZqg7PLdMq_ZoALOsTRyPGrXnvDRcWqIm6gmSVuZC-TEG8kqPcXgp18Gm6sUr-J4LAseLJan6UO2e6_2gsuGg8hZNBoggwE"},
                new HeaderRequest { Category = HeaderRequestType.Cookie, Name = "__RequestVerificationToken", Value = "f503FWEz0K8zF21K5z5YD5YvS_5uF2QQ142IDC9toQtI--njNpXG0efNZcYamnz-A5zr0Hu9weET7lDWRt5PxW26hIgvBdUXDO-9EN3P59A1"}
            });

            //Act
            var document = _webQueryService.GetPage("http://payroll.localhost/EmployeeController/Index");

            //Asserts
            Assert.That(document, Is.Not.Null);
        }
    }
}
