using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Kraj : IKraj
    {
        public string Nazwa { get; }
        public List<string> Podpowiedzi { get; }

        public Kraj(string nazwa, List<string> podpowiedzi)
        {
            Nazwa = nazwa;
            Podpowiedzi = podpowiedzi;
        }
    }
}
