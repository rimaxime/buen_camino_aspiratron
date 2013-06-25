using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace getEuroski
{
    class albergue
    {
        public string chemin { get; set; }
        public string nom { get; set; }
        public string adresse { get; set; }
        public string ville { get; set; }
        public float prix { get; set; }
        public int places { get; set; }
        public bool machine { get; set; }
        public bool seche_linge { get; set; }
        public bool cuisine { get; set; }
        public bool internet { get; set; }
        public bool wifi { get; set; }
        public int type {get; set;}


        public override string ToString()
        {
            return string.Format("|nom : {0} | adresse : {1} | ville : {2} | prix : {3} | places : {4} | machine :{5} | seche linge : {6} |Chemin : {7}"+
                "| Cuisine : {8} | internet : {9} | wifi : {10} | type : {11}"
                , nom,
                adresse,
                ville, 
                prix, 
                places,
                machine,
                seche_linge,
                chemin,
                cuisine,
                internet,
                wifi, type);
        }

        public string uploadInDatabase()
        {
            return string.Format("INSERT INTO Albergues (nom, ville,places,prix,type,cuisine,lave_linge,seche_linge,internet,wifi,chemin, adresse) VALUES (\"{0}\"," +
                "\"{1}\", {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, \"{10}\", \"{11}\");",
                nom,
                ville,
                places,
                prix,
                type,
                Convert.ToInt32(cuisine),
                Convert.ToInt32(machine),
                Convert.ToInt32(seche_linge),
                Convert.ToInt32(internet),
                Convert.ToInt32(wifi),
                chemin,
                adresse);

        }

    }
}
