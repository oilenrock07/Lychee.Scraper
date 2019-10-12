using System;
using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace Lychee.Scrapper.Domain.Models.Web
{
    public class WebQuery
    {
        //The cookies will be here.
        private CookieContainer _cookies = new CookieContainer();
        public string Method { get; set; }

        public Uri Uri { get; set; }

        public WebQuery(string url)
        {
            Method = "GET";
            Uri = new Uri(url);
        }

        public virtual void ClearCookies()
        {
            _cookies = new CookieContainer();
        }

        public virtual void AddCookie(string name, string value)
        {
            _cookies.Add(new Cookie(name, value) { Domain = Uri.Host });
        }

        public virtual HtmlDocument GetPage()
        {
            var request = (HttpWebRequest)WebRequest.Create(Uri);
            request.Method = Method;
            
            //Set more parameters here...
            //...

            //This is the important part.
            request.CookieContainer = _cookies;

            using (var response = (HttpWebResponse) request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream == null) return null;
                    using (var reader = new StreamReader(stream))
                    {
                        var html = reader.ReadToEnd();
                        var doc = new HtmlDocument();
                        doc.LoadHtml(html);
                        return doc;
                    }
                }
            }
        }
    }
}
