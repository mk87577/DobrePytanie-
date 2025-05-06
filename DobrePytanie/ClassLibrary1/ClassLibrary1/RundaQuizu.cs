using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class RundaQuizu : IRundaQuizu
    {
        private int indeksPodpowiedzi = 0;
        public IKraj Kraj { get; }
        public string AktualnaPodpowiedź => Kraj.Podpowiedzi[indeksPodpowiedzi];
        public int AktualnePunkty => 5 - indeksPodpowiedzi;
        public bool CzyMoznaPokazacWiecejPodpowiedzi => indeksPodpowiedzi < 4;
        public int NumerRundy { get; set; }

        public RundaQuizu(IKraj kraj)
        {
            Kraj = kraj;
            NumerRundy = 0;
        }

        public void PokazNastepnaPodpowiedz()
        {
            if (!CzyMoznaPokazacWiecejPodpowiedzi)
            {
                throw new BrakPodpowiedziException();
            }
            indeksPodpowiedzi++;
        }

        public bool SprawdzOdpowiedz(string odpowiedz)
        {
            return string.Equals(odpowiedz.Trim(), Kraj.Nazwa, StringComparison.OrdinalIgnoreCase);
        }

        public bool CzyKoniecPodpowiedzi => !CzyMoznaPokazacWiecejPodpowiedzi;

        public void PrzejdzDoLatwiejszegoEtapu()
        {
            PokazNastepnaPodpowiedz();
        }
    }
}
