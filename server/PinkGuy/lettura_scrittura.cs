using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Net.Http;
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
//il file xml deve essere formattato correttamente , usa il file xml in questa repo per incominciare
        const string xmlfile = @"C:\Users\buy more BTC PLZZ\source\listaPP.xml";


        //funzinoe che serve per aggiungere una notizia
        static void add(string id, string giornalista, string d, string _titolo, string _settore, string _argomento, string _area_geografica, string _contenuto)
        {
            var notizia = new Notizia(id, giornalista, d, _titolo, _settore, _argomento, _area_geografica, _contenuto);
            var xdoc = XDocument.Load(xmlfile);
            var xelement = new XElement("notizia", new XAttribute("uid", notizia.uid),
                new XElement("giornalista", notizia.giornalista),
                new XElement("date", notizia.date),
                new XElement("titolo", notizia.titolo),
                new XElement("settore", notizia.settore),
                new XElement("argomento", notizia.argomento),
                new XElement("areaG", notizia.areaG),
                new XElement("contenuto", notizia.contenuto)
                );
            xdoc.Root.Add(xelement);
            xdoc.Save(xmlfile);

            Console.WriteLine("finito boss, ho creato l'elemento e lo ho scritto nel file xml");

        }
        //funzione che serve per luppare [non so come si scrive] le notizie del file xml
        static void get()
        {
            var xdoc = XDocument.Load(xmlfile);
            var notizie = xdoc.Root.Descendants("notizia").Select(x => new Notizia(x.Attribute("uid").Value,
                x.Element("giornalista").Value,
                x.Element("date").Value,
                x.Element("titolo").Value,
                x.Element("settore").Value,
                x.Element("argomento").Value,
                x.Element("areaG").Value,
                x.Element("contenuto").Value


               )
            );
            //nadia, te dovrai lavorare su questa funzione, (tipo vedere vedere notizia secondo schema e altra roba)
            foreach (var notizia in notizie)
            {
                Console.WriteLine(notizia.uid);
                Console.WriteLine(notizia.giornalista);
            }
        }


        static async Task Main(string[] args)
        {
            add(RandomString(5), "giornalista", DateTime.Now.ToString(), "titolo", "settore", "argomento", "areaG", "contenutoooooooooo");
            get();
            #region parte EXTRA , API

            // PARTE API --EXTRA--
            Program program = new Program();
            HttpClient client = new HttpClient();
            //IL PAESE PUO ESSESERE CAMBIATO, NELLA STESSA CARTELLA DELLA REPO CE UN FILE CON TUTTI I PAESI
            string paese = "it";
            string response = await client.GetStringAsync("https://newsdata.io/api/1/news?apikey=pub_550421bb8353e2a96889cbe04dd64c7f2f2f&country=" + paese);
            Root converted = JsonConvert.DeserializeObject<Root>(response);
            Console.WriteLine(converted.totalResults);


            List<notizia_api> a = converted.results;
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
                    foreach (string xpp in x.keywords)
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
            #endregion
        }





        #region parte extra, classi API

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
        #endregion

        //CLASSE NOTIZIA , SERVE PER AGGIUNGERE LA NOTIZIA TYPATA DAL USER
        public class Notizia
        {
            public string uid { get; set; }
            //giornalista = giornalista
            public string giornalista { get; set; }
            //date = data
            public string date { get; set; }
            public string titolo { get; set; }
            public string settore { get; set; }
            public string argomento { get; set; }
            public string areaG { get; set; }

            public string contenuto { get; set; }

            public Notizia(string _id, string _giornalista, string d, string _titolo, string _settore, string _argomento, string _area_geografica, string _contenuto)
            {
                uid = _id;
                giornalista = _giornalista;
                date = d;
                titolo = _titolo;
                settore = _settore;
                argomento = _argomento;
                areaG = _area_geografica;
                contenuto = _contenuto;
            }
        }
    }
}
