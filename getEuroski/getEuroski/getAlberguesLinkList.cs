using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace getEuroski
{
    class getAlberguesLinkList
    {
        public List<string> hrefTags { get; set; }
        //public List<string> list_possible_name { get; set; }
        public getAlberguesLinkList(string url)
        {
            HtmlWeb hw = new HtmlWeb();
            Console.WriteLine(url);
            HtmlDocument doc = new HtmlDocument();
            doc = hw.Load(url);
            if (doc != null && doc.DocumentNode != null)
            {
                hrefTags = new List<string>();
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//li/a[@href]"))
                {   
                    HtmlAttribute att = link.Attributes["href"];
                    if (!att.Value.Contains("http") && att.Value.Split('/').Length - 1 == 1 && att.Value.Length > 1)
                        hrefTags.Add(att.Value);
                }
            }
            else
                Console.WriteLine("Open problem");
        }


        public void displayList()
        {
            foreach (string s in hrefTags)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("Number of items : " + hrefTags.Count);
        }


        /*public List<string> possibleNames()
        {
            list_possible_name = new List<string>();
            list_possible_name.Add("albergue");
            list_possible_name.Add("refugio");
            list_possible_name.Add("")
        }*/
    }
}
