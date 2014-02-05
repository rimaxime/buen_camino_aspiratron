using GoogleMapsApi;
using System;
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
            string fileName = @"C:\Temp\script_sql.txt";
            string fileName_csv = @"C:\Temp\albergue_csv.txt";

            var alberguesLinkList = new getAlberguesLinkList("http://caminodesantiago.consumer.es/albergues/");
            alberguesLinkList.displayList();
                // Check if file already exists. If yes, delete it. 
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                if (File.Exists(fileName_csv))
                {
                    File.Delete(fileName_csv);
                }

                // Create a new file 
                List<albergue> albergues = new List<albergue>(alberguesLinkList.hrefTags.Count());
                using (StreamWriter sw = File.CreateText(fileName))
                using (StreamWriter sw_csv = File.CreateText(fileName_csv))
                {
                    try
                    {
                        sw_csv.WriteLine(albergue.GetHeaderCsv());
                        foreach (string s in alberguesLinkList.hrefTags)
                        {


                            getAnAlbergueContent extract_albergue = new getAnAlbergueContent(string.Concat("http://caminodesantiago.consumer.es", s));
                            albergue current_albergue = extract_albergue.ExtractAlbergueContent();
                            GoogleMapsApi.Entities.Places.Request.PlacesRequest plRequest = new GoogleMapsApi.Entities.Places.Request.PlacesRequest();
                            GoogleMapsApi.Entities.Geocoding.Request.GeocodingRequest adressRequest = new GoogleMapsApi.Entities.Geocoding.Request.GeocodingRequest();
                            adressRequest.Address = current_albergue.adresse + ", " + current_albergue.ville;
                            GoogleMapsApi.Entities.Geocoding.Response.GeocodingResponse adressResponse = GoogleMapsApi.GoogleMaps.Geocode.Query(adressRequest);
                            if (adressResponse.Results.Count() > 0)
                            {
                                current_albergue.latitude = adressResponse.Results.FirstOrDefault().Geometry.Location.Latitude;
                                current_albergue.longitude = adressResponse.Results.FirstOrDefault().Geometry.Location.Longitude;
                            }
                            else
                            {
                                adressRequest.Address = current_albergue.ville;
                                adressResponse = GoogleMaps.Geocode.Query(adressRequest);
                                if (adressResponse.Results.Count() > 0)
                                {
                                    current_albergue.latitude = adressResponse.Results.FirstOrDefault().Geometry.Location.Latitude;
                                    current_albergue.longitude = adressResponse.Results.FirstOrDefault().Geometry.Location.Longitude;
                                }
                            }

                            Console.WriteLine(current_albergue.ToString());

                            
                            sw.WriteLine(current_albergue.ToSqlRequest());
                            sw_csv.WriteLine(current_albergue.ToCsv());
                        }
                    }
                    catch (Exception Ex)
                    {
                        sw.WriteLine(Ex.ToString());
                        Console.WriteLine(Ex.ToString());

                    }
                    finally
                    {
                        Console.WriteLine("Creation script finished");
                        Console.Read();
                    }
                    
                }


        }
    }
}
