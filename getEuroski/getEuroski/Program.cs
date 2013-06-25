﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace getEuroski
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            string fileName = @"C:\Temp\script_sql.txt";
            getAlberguesLinkList test = new getAlberguesLinkList("http://caminodesantiago.consumer.es/albergues/");
            test.displayList();
                // Check if file already exists. If yes, delete it. 
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file 
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    try
                    {
                        foreach (string s in test.hrefTags)
                        {


                            getAnAlbergueContent extract_albergue = new getAnAlbergueContent(string.Concat("http://caminodesantiago.consumer.es", s));
                            //getAnAlbergueContent extract_albergue = new getAnAlbergueContent(string.Concat("http://caminodesantiago.consumer.es", test.hrefTags[i]));
                            sw.WriteLine(extract_albergue.al.uploadInDatabase());
                            Console.WriteLine(i++);
                        }
                    }
                    catch (Exception Ex)
                    {
                        sw.WriteLine(Ex.ToString());

                    }  
                    
                }


        }
    }
}