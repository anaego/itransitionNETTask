using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task3Try1.Models;

namespace Task3Try1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: ChipParser
        [HttpPost]
        public ActionResult Search(string userInput)
        {
            List<Chip> resultList = new List<Chip>();
            if (userInput != "")
            {
                Parser belchipParser = new BelchipParser("http://belchip.by/");
                Parser chipdipParser = new ChipDipParser("https://www.ru-chipdip.by/");
                resultList = belchipParser.Parse("http://belchip.by/search_fuzzy/?query=" + userInput)
                    .Concat(chipdipParser.Parse("https://www.ru-chipdip.by/search?searchtext=" + userInput))
                    .ToList();
            }
            if (resultList.Count != 0)
            {
                return View("Search", resultList);
            }
            else
            {
                return View("Index");
            }
        }

        public void RedirectTo(string url)
        {
            Response.Redirect(url);
        }
    }
}