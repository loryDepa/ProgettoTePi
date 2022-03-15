using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Threading.Tasks;

//QUESTO PACKAGE SERVE PER PARSARE LA RISPOSTA DALL'API , NON E INCLUSO , SI DEVE INSTALLARE
//VAI IN SEARCH E SCRIVI "manage getnu packages"
//VAI SU BROWSE E CERCA newtonsoft.json , APPENA INSTALLATO POTRAI IMPORTARE IL PACKAGE
using Newtonsoft.Json;

namespace progetto_tepi
{
    class Program
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "qwertyuiopasdfghjklzxcvbnm0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        const string xmlfile = @"C:\Users\buy more BTC PLZZ\source\listaPP.xml";
        
        static void carica_notizia(string _giornalista, string _titolo, string _settore, string _argomento, string _area_geografica, string _contenuto)
        {
            var notizia = new notizia(_giornalista,_titolo,_settore, _argomento, _area_geografica, _contenuto);
            var xdoc = XDocument.Load(xmlfile);
            /*
            Random rnd = new Random();
            int num = rnd.Next(1337);
            */

            string random_string = RandomString(5);
            var xelement = new XElement("NOTIZIA",
                new XAttribute("uid", random_string),
                new XElement("giornalista", notizia.giornalista),
                new XElement("data", notizia.data),
                new XElement("titolo",notizia.titolo),
                new XElement("settore", notizia.settore),
                new XElement("argomento", notizia.argomento),
                new XElement("area_geografica", notizia.area_geografica),
                new XElement("contenuto", notizia.contenuto));
                
            xdoc.Root.Add(xelement);
            xdoc.Save(xmlfile);
            Console.WriteLine("finito");

        }

        static async Task Main(string[] args)
        {
            carica_notizia("a", "a", "a", "a", "a", "a");

            // PARTE API
            Program program = new Program();
            HttpClient client = new HttpClient();
            
            //IL PAESE PUO ESSESERE CAMBIATO, NELLA STESSA CARTELLA DELLA REPO CE UN FILE CON TUTTI I PAESI
            string paese = "it";
            string response = await client.GetStringAsync("https://newsdata.io/api/1/news?apikey=pub_550421bb8353e2a96889cbe04dd64c7f2f2f&country="+paese);
            Root converted = JsonConvert.DeserializeObject<Root>(response);
            Console.WriteLine(converted.totalResults);


            List < notizia_api > a = converted.results;
            //Console.WriteLine(a[0].title);
            //Console.WriteLine(a[0].link);
            //Console.WriteLine(a[0].content);

            //PER OGNI NOTIZIA NELLA ROOT DEL FILE JSON
            foreach (notizia_api x in converted.results)
            {
                //PRIMA DI PRINTARE SI DEVE SEMPRE VERIFICARE SE L'ELEMENTO E NULL , ALTRIMENTI SI HA UN ERRORE 
                Console.WriteLine(x.title);
                if (x.title != null)
                {
                    Console.WriteLine(x.title);
                }
                if (x.link != null)
                {
                    Console.WriteLine(x.link);
                }

                if (x.keywords != null)
                {
                    foreach(string xpp in x.keywords)
                    {
                        Console.WriteLine(xpp);
                    }
                }
                if (x.creator != null)
                {
                    foreach (string xpp in x.creator)
                    {
                        Console.WriteLine(xpp);
                    }
                }

                if (x.video_url != null)
                {
                    Console.WriteLine(x.video_url);
                }

                //CONTINUA CON ALTRI ATTRIBUTI

            }


                
        }






        
        //DA IGNORARE , E LA NOTIZIA CHE SI TROVA NELLA LISTA DI NOTIZIE NEL ROOT DELLA RISPOSTA DELL API
        public class notizia_api
        {
            public string title { get; set; }
            public string link { get; set; }
            public List<string> keywords { get; set; }

            public List<string> creator { get; set; }
            public object video_url { get; set; }
            public string description { get; set; }
            public string content { get; set; }
            public string pubDate { get; set; }
            public string full_description { get; set; }
            public string image_url { get; set; }
            public string source_id { get; set; }
            public List<string> country { get; set; }
            public List<string> category { get; set; }
            public string language { get; set; }
        }

        //DA IGNORARE, E LA PARTE ROOT DEL JSON DELLA RISPOSTA DELL API
        public class Root
        {
            public string status { get; set; }
            public int totalResults { get; set; }
            public List<notizia_api> results { get; set; }
            public int nextPage { get; set; }
        }

        //CLASSE NOTIZIA , SERVE PER AGGIUNGERE LA NOTIZIA TYPATA DAL USER
        public class notizia
        {
            public string giornalista { get; set; }
            public DateTime data { get; set; }
            public string titolo { get; set; }
            public string settore{ get; set; }

            public string argomento { get; set; }
            public string area_geografica { get; set; }

            public string contenuto { get; set; }
            public notizia(string _giornalista,string _titolo,string _settore, string _argomento , string _area_geografica, string _contenuto)
            {
                giornalista = _giornalista;
                data = DateTime.Now;
                titolo = _titolo;
                settore = _settore;
                argomento = _argomento;
                area_geografica = _area_geografica;
                contenuto = _contenuto;
            }
        }
    }}

