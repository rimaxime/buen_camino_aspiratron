using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace getEuroski
{
    class getAnAlbergueContent
    {
        private albergue al;

        private string url;

        public getAnAlbergueContent(string url)
        {
            this.url = url;
        }

        public albergue ExtractAlbergueContent()
        {
            HtmlWeb hw = new HtmlWeb();
            //Console.WriteLine(url);
            HtmlDocument doc = new HtmlDocument();
            doc = hw.Load(url);
            if (doc != null && doc.DocumentNode != null)
            {
                al = new albergue();
                //chemin
                al.chemin = doc.DocumentNode.SelectSingleNode("//h1").InnerHtml.Split('<')[0].Trim();

                //nom
                al.nom = doc.DocumentNode.SelectSingleNode("//*[@class=\"titulo-principal\"]").InnerText.Split(':')[1].Trim();
                if (al.nom.Contains("Hostel"))
                {
                    al.type = 3;
                }
                else if (al.nom.Contains("Casa"))
                {
                    al.type = 2;
                }
                else
                    al.type = 1;

                //ville & adresse
                foreach (HtmlNode n in doc.DocumentNode.SelectNodes("//*[@id=\"bloque-ficha-albergue\"]").Elements("ul"))
                {
                    
                    var r_ville = 
                        from n_ville in n.Elements("li")
                        let values = n_ville.Elements("span").Select(c => c.InnerText).ToArray()
                        select values;
         
                    al.adresse = r_ville.ElementAt(0)[0];
                    al.ville = r_ville.ElementAt(1)[0].Split('(')[0].Trim();
                    
                    
                    
                }
                //prix et places
                int inc = 0;
                foreach(HtmlNode n in doc.DocumentNode.SelectNodes("//*[@id=\"descripcion-contenido\"]"))
                {
                    var infos_contenu =
                        from n_contenu in n.Elements("ul")
                        let values = n_contenu.Elements("li").Select(c => c.InnerText).ToArray()
                        select values;
                    foreach(string[] n12 in infos_contenu)
                        foreach (string il in n12)
                    {
                        string test_int = Regex.Match(il, @"[-+]?[0-9]*\.?[0-9]+").Value;
                        if (test_int != "")
                        {
                            Console.WriteLine(test_int);
                            if (inc == 2)
                               al.prix=float.Parse(test_int.Replace('.',','));
                            else if(inc == 3)
                               al.places = Int32.Parse(test_int);
                        }
                        inc++;
                    }                    
                }

                //machine a laver + seche linge
                foreach (HtmlNode n in doc.DocumentNode.SelectNodes("//*[@id=\"equipamiento-contenido\"]/ul[@class=\"listado-contenido\"]"))
                {
                    foreach(HtmlNode n1 in n.SelectNodes("li"))
                    {
                        if(n1.SelectSingleNode("span").InnerText.Split(':')[0].Trim().Contains("Lavadora"))
                        {
                            if (n1.InnerText.Split(':')[1].Trim().Contains("Sí"))
                                al.machine = true;
                            else
                                al.machine = false;
                            if (n1.InnerText.Split(':')[1].Trim().Contains("secadora "))
                                al.seche_linge = true;
                            else
                                al.machine = false;
                        }
                    }
  
                        
                }

                //cuisine
                foreach (HtmlNode n in doc.DocumentNode.SelectNodes("//*[@id=\"infraestructura-contenido\"]/ul[@class=\"listado-contenido\"]"))
                {
                    foreach (HtmlNode n1 in n.SelectNodes("li"))
                    {
                        if (n1.SelectSingleNode("span").InnerText.Split(':')[0].Trim().Contains("Cocina"))
                        {
                            if (n1.InnerText.Split(':')[1].Trim().Contains("Sí"))
                                al.cuisine = true;
                            else
                                al.cuisine = false;
                        }
                    }
                }

                //internet + wifi
                foreach (HtmlNode n in doc.DocumentNode.SelectNodes("//*[@id=\"servicios-contenido\"]/ul[@class=\"listado-contenido\"]"))
                {
                    foreach (HtmlNode n1 in n.SelectNodes("li"))
                    {
                        if (n1.SelectSingleNode("span").InnerText.Split(':')[0].Trim().Contains("Internet"))
                        {
                            if (n1.InnerText.Split(':')[1].Trim().Contains("Sí") && n1.InnerText.Split(':')[1].Trim().Contains("albergue"))
                                al.internet = true;
                            else
                                al.internet = false;
                            if (n1.InnerText.Split(':')[1].Trim().Contains("Wi-Fi") && n1.InnerText.Split(':')[1].Trim().Contains("albergue"))
                                al.wifi = true;
                            else
                                al.wifi = false;
                        }


                    }
                }
                //Console.Read();
                //get the content wished.
                // Nom : class titulo-principal
                // ville : dans le bloc id="bloque-ficha-albergue"
                //id="descripcion-contenido" class="listado-contenido"
                //id="equipamiento-contenido"
                //id="infraestructura-contenido"
                //id="servicios-contenido"
                
            }
            else
                Console.WriteLine("Open problem");
            return al;
        }

    }
}
