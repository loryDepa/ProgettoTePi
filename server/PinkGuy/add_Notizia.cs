using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Globalization;


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
        //prima si deve creare il file xml e si deve formattarlo (il file gia formattato lo trovi in PinkGuy/lista_notizie.xml
        const string xmlfile = @"C:\Users\sheesh\source\lista_notizie.xml";
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

         static void Main(string[] args)
        {
            carica_notizia("a", "a", "a", "a", "a", "a");
        }

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
    }
}
