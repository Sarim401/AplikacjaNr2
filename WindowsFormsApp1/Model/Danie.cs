using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APkaaa
{
    public class Danie
    {
        public string Nazwa { get; set; }
        public string Opis { get; set; }
        public List<string> Skladniki { get; set; }

        public Danie(string nazwa, string opis, List<string> skladniki)
        {
            Nazwa = nazwa;
            Opis = opis;
            Skladniki = skladniki;
        }
    }
}
