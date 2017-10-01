using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Task3Try1.Models
{
    public class ChipDipParser : Parser
    {
        public HtmlNode ResultHtmlDocumentNode { get; set; }
        public HtmlNodeCollection PictureLinksContainers { get; set; }
        public List<string> ResultUnwrappedPictureLinks { get; set; }
        public IEnumerable<string> ResultGroups { get; set; }
        public string AvailableMessage = "item__avail_available";

        public ChipDipParser(string baseUrl) : base(baseUrl)
        {
        }

        public override List<Chip> Parse(string url)
        {
            HtmlDocumentNode = Web.Load(url).DocumentNode;
            try
            {
                ResultGroups = HtmlDocumentNode.SelectNodes("//a[@class='link group-header']")
                    .Select(n => n.GetAttributeValue("href", null));
            }
            catch (ArgumentNullException e)
            {
                ResultGroups = null;
            }
            return GetChipList();
        }

        internal override List<Chip> GetChipList()
        {
            if (ResultGroups != null)
            {
                foreach (var resultGroup in ResultGroups)
                {
                    GoThroughPages(resultGroup);
                }
            }
            return ChiplList;
        }

        private void GoThroughPages(string resultGroup)
        {
            var targetLink = resultGroup;
            do
            {
                ResultHtmlDocumentNode = Web.Load("https://www.ru-chipdip.by/" + targetLink).DocumentNode;
                try
                {
                    GetAllChipsProperties();
                    SetAllChipsProperties();
                }
                catch (Exception e)
                {
                    continue;
                }
                finally
                {
                    targetLink = SetNewTargetLink(targetLink);
                }
            } while (ResultHtmlDocumentNode.SelectNodes("//div[@class='pager']//span[@class='right']/a") != null);
        }

        private void GetAllChipsProperties()
        {
            ResultNames = GetResultNames();
            ResultLinks = GetResultLinks();
            ResultPictureLinks = GetResultPictureLinks();
            ResultAvailability = GetResultAvailability();
            ResultPrice = GetResultPrice();
        }

        private void SetAllChipsProperties()
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

        private void SetChipProperties(IEnumerator<string> e1, IEnumerator<string> e2, IEnumerator<string> e3, IEnumerator<string> e4, IEnumerator<string> e5)
        {
            var name = e1.Current ?? "Error";
            var link = e2.Current;
            var pictureLink = e3.Current;
            var availability = e4.Current;
            var price = e5.Current;
            ChiplList.Add(new Chip(name, BaseUrl + link, pictureLink,
                availability == AvailableMessage, price));
        }

        private string SetNewTargetLink(string targetLink)
        {
            targetLink = ResultHtmlDocumentNode.SelectSingleNode("//div[@class='pager']//span[@class='right']/a") != null
                ? ResultHtmlDocumentNode.SelectSingleNode("//div[@class='pager']//span[@class='right']/a")
                    .GetAttributeValue("href", null)
                : targetLink;
            return targetLink;
        }

        internal override IEnumerable<string> GetResultNames()
        {
            return ResultHtmlDocumentNode.SelectNodes("//div[@class='name']/a") != null
                    ? ResultHtmlDocumentNode.SelectNodes("//div[@class='name']/a")
                            .Select(n => n.InnerText)
                    : ResultHtmlDocumentNode.SelectNodes("//span[@class='link']")
                            .Select(n => n.GetAttributeValue("title", null));
        }

        internal override IEnumerable<string> GetResultLinks()
        {
            return ResultHtmlDocumentNode.SelectNodes("//div[@class='name']/a") != null
                    ? ResultHtmlDocumentNode.SelectNodes("//div[@class='name']/a")
                            .Select(n => n.GetAttributeValue("href", null))
                    : ResultHtmlDocumentNode.SelectNodes("//div[@class='item__content']/a")
                            .Select(n => n.GetAttributeValue("href", null));
        }

        internal override IEnumerable<string> GetResultPictureLinks()
        {
            PictureLinksContainers = ResultHtmlDocumentNode.SelectNodes("//td[@class='img']") != null
                    ? ResultHtmlDocumentNode.SelectNodes("//td[@class='img']")
                    : ResultHtmlDocumentNode.SelectNodes("//div[@class='item__image-wrapper']");
            ResultUnwrappedPictureLinks = new List<string>();
            UnwrapPictureLinks();
            return ResultUnwrappedPictureLinks;
        }

        private void UnwrapPictureLinks()
        {
            foreach (var pictureLinkContainer in PictureLinksContainers)
            {
                if (pictureLinkContainer.HasChildNodes
                    && pictureLinkContainer.SelectSingleNode(".//img") != null)
                {
                    ResultUnwrappedPictureLinks.Add(pictureLinkContainer
                        .SelectSingleNode(".//img").GetAttributeValue("src", null));
                }
                else
                {
                    ResultUnwrappedPictureLinks.Add("");
                }
            }
        }

        internal override IEnumerable<string> GetResultAvailability()
        {
            return ResultHtmlDocumentNode.SelectNodes("//div[@class='av_w2']/span") != null
                    ? ResultHtmlDocumentNode.SelectNodes("//div[@class='av_w2']/span")
                            .Select(n => n.GetAttributeValue("class", null))
                    : ResultHtmlDocumentNode.SelectNodes("//div[@class='item__avail']/div")
                            .Select(n => n.GetAttributeValue("class", null));
        }

        internal override IEnumerable<string> GetResultPrice()
        {
            return ResultHtmlDocumentNode.SelectNodes("//span[@class='price_mr']") != null
                    ? ResultHtmlDocumentNode.SelectNodes("//span[@class='price_mr']")
                            .Select(n => n.InnerText)
                    : ResultHtmlDocumentNode.SelectNodes("//span[@class='price__value']")
                            .Select(n => n.InnerText);
        }
    }
}