using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Task3Try1.Models
{
    public abstract class Parser
    {
        public string Url { get; set; }
        public HtmlWeb Web { get; set; }
        public HtmlNode HtmlDocumentNode { get; set; }

        public IEnumerable<string> ResultNames { get; set; }
        public IEnumerable<string> ResultLinks { get; set; }
        public IEnumerable<string> ResultPictureLinks { get; set; }
        public IEnumerable<string> ResultAvailability { get; set; }
        public IEnumerable<string> ResultPrice { get; set; }

        public List<Chip> ChiplList { get; set; }

        public string Name { get; set; }
        public string Link { get; set; }
        public string PictureLink { get; set; }
        public string Availability { get; set; }
        public string Price { get; set; }

        public string BaseUrl { get; set; }

        public Parser(string baseUrl)
        {
            Web = new HtmlWeb();
            ChiplList = new List<Chip>();
            BaseUrl = baseUrl;
        }

        public abstract List<Chip> Parse(string url);

        internal abstract List<Chip> GetChipList();

        internal abstract IEnumerable<string> GetResultNames();

        internal abstract IEnumerable<string> GetResultLinks();

        internal abstract IEnumerable<string> GetResultPictureLinks();

        internal abstract IEnumerable<string> GetResultAvailability();

        internal abstract IEnumerable<string> GetResultPrice();
    }
}