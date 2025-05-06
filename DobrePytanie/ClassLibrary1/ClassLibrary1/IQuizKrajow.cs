using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IQuizKrajow
    {
        int Zycia { get; }
        int Wynik { get; }
        IRundaQuizu AktualnaRunda { get; }
        void RozpocznijNowaGre();
        void RozpocznijNowaRunde();
        bool PrzeslijOdpowiedz(string odpowiedz, out int punkty);
    }
}
