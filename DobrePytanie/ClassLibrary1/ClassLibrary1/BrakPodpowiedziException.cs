using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class BrakPodpowiedziException : Exception
    {
        public BrakPodpowiedziException()
            : base("Brak kolejnych podpowiedzi!") { }
    }
}
