using System;
using System.Collections.Generic;
using System.Linq;

namespace Task3Try1.Models
{
    public class BelchipParser : Parser
    {
        public string NotAvailableMessage = "цена по запросу";

        public BelchipParser(string baseUrl) : base(baseUrl)
        {
        }

        public override List<Chip> Parse(string url)
        {
            HtmlDocumentNode = Web.Load(url).DocumentNode;
            try
            {
                ResultNames = GetResultNames();
                ResultLinks = GetResultLinks();
                ResultPictureLinks = GetResultPictureLinks();
                ResultAvailability = GetResultAvailability();
                ResultPrice = GetResultPrice();
            }
            catch (Exception e)
            {
            }
            return GetChipList();
        }

        internal override IEnumerable<string> GetResultNames()
        {
            return HtmlDocumentNode.SelectNodes("//div[@class='cat-item']/h3/a")
                .Select(n => n.InnerText);
        }

        internal override IEnumerable<string> GetResultLinks()
        {
            return HtmlDocumentNode.SelectNodes("//div[@class='cat-item']/h3/a")
                .Select(n => n.GetAttributeValue("href", null));
        }

        internal override IEnumerable<string> GetResultPictureLinks()
        {
            return HtmlDocumentNode.SelectNodes("//div[@class='cat-pic']/a/img")
                .Select(n => n.GetAttributeValue("src", null));
        }

        internal override IEnumerable<string> GetResultAvailability()
        {
            return HtmlDocumentNode.SelectNodes("//div[@class='butt-add']/span")
                .Select(n => n.InnerText);
        }

        internal override IEnumerable<string> GetResultPrice()
        {
            return HtmlDocumentNode.SelectNodes("//div[@class='denoPrice']")
                .Select(n => n.InnerText);
        }
        
        internal override List<Chip> GetChipList()
        {
            try
            {
                using (var e1 = ResultNames.GetEnumerator())
                using (var e2 = ResultLinks.GetEnumerator())
                using (var e3 = ResultPictureLinks.GetEnumerator())
                using (var e4 = ResultAvailability.GetEnumerator())
                using (var e5 = ResultPrice.GetEnumerator())
                {
                    while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext())
                    {
                        SetChipProperties(e1, e2, e3, e4, e5);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return ChiplList;
        }

        private void SetChipProperties(IEnumerator<string> e1, IEnumerator<string> e2, IEnumerator<string> e3, IEnumerator<string> e4, IEnumerator<string> e5)
        {
            Name = e1.Current;
            Link = e2.Current;
            PictureLink = e3.Current;
            Availability = e4.Current;
            Price = e5.Current;
            ChiplList.Add(new Chip(Name, BaseUrl + Link, BaseUrl + PictureLink, Availability != NotAvailableMessage,
                Price));
        }
    }
}