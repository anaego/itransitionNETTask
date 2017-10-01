using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task3Try1.Models
{
    public class Chip
    {
        public Chip(string name, string url, string pictureUrl, bool available, string price)
        {
            Name = name;
            Url = url;
            PictureUrl = pictureUrl;
            Available = available;
            Price = price;
        }

        public string Name { get; set; }
        public string Url { get; set; }
        public string PictureUrl { get; set; }
        public bool Available { get; set; }
        public string Price { get; set; }
    }
}