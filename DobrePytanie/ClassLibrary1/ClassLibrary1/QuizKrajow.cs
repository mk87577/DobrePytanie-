using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class QuizKrajow : IQuizKrajow
    {
        private readonly List<IKraj> kraje;
        private List<IKraj> nieodgadnieteKraje;
        private static readonly Random losowy = new Random();
        public int Zycia { get; private set; }
        public int Wynik { get; private set; }
        public IRundaQuizu AktualnaRunda { get; private set; }
        public int NumerRundy { get; private set; } = 0;

        public event UtraconoZycieDelegat UtraconoZycie;
        public event PoprawnaOdpowiedzDelegat PoprawnaOdpowiedz;

        public QuizKrajow() : this(new List<IKraj>
        {
            new Kraj("Polska", new List<string> { "W tym kraju znajduje się największy zamek średniowieczny w Europie.", "Znajduje się w UE od 2004 roku",
                "Jest to kraj, który graniczy m.in. z Rosją i Niemcami.", "Flaga tego kraju składa się z dwóch kolorów: biały i czerwony.", "Jego stolicą jest Warszawa." }),
            new Kraj("Francja", new List<string> { "Ten kraj graniczy z Belgią, Niemcami i Hiszpanią.", "Znajduje się tam region Prowansja, znany z lawendy i wina.",
                "Leży nad Oceanem Atlantyckim i Morzem Śródziemnym.", "Znany jest z wieży Eiffla i kuchni haute cuisine.", "Jego stolicą jest Paryż." }),
            new Kraj("Brazylia", new List<string> { "Leży na półkuli południowej i ma klimat głównie tropikalny.", "Przez jego terytorium przepływa Amazonka.",
                "Jest to największy kraj w Ameryce Południowej.", "Jego największe miasto to nie stolica, ale finansowe centrum Ameryki Południowej.", "Słynie z karnawału i piłki nożnej." }),
            new Kraj("Japonia", new List<string> { "Ten kraj składa się z ponad 6000 wysp.", "Gospodarka tego kraju opiera się w dużej mierze na technologii i przemyśle motoryzacyjnym.",
                "W tym kraju znajduje się góra Fuji.", "Stolicą tego kraju jest Tokio.", "Znany jest z sushi i ceremonii parzenia herbaty." }),
            new Kraj("Niemcy", new List<string> { "Kraj ten jest jednym z najważniejszych członków Unii Europejskiej, mającym silną gospodarkę opartą na przemyśle i technologii.", "We wrześniu i październiku odbywa się tu jedno z najbardziej znanych wydarzeń piwnych na świecie.", 
                "Podzielony był kiedyś na dwa kraje: RFN i NRD.", "Jest domem dla znanych marek samochodowych, takich jak Mercedes-Benz, Audi.", "Jego stolicą jest Berlin" }),
            new Kraj("Kanada", new List<string> { "Kraj ten jest domem dla największego na świecie systemu jezior słodkowodnych, który znajduje się na jego terytorium.", "Ma niezwykłą różnorodność klimatyczną – od arktycznych obszarów na północy po umiarkowane regiony na południu.",
                "Jest drugim największym państwem na świecie pod względem powierzchni.", "Najpopularniejszym sportem jest hokej na lodzie a ogromną organizacją jest Toronto Maple Leafs.", "Słynie z syropu klonowego." }),
            new Kraj("Egipt", new List<string> { "Jest jednym z głównych producentów bawełny na świecie.", "Kultura tego kraju rozwinęła się wzdłuż jednej z najstarszych cywilizacji świata.",
                "Dużą część tego kraju pokrywa pustynia.", "Przepływa przez niego najdłuższa rzeka świata, Nil.", "Znajdujące się tam piramidy są jednym z siedmiu cudów świata." }),
            new Kraj("Chiny", new List<string> { "Słynie z jednego z najstarszych systemów pisma, który jest zupełnie inny niż w zachodnich językach.", "Jest to kraj o jednym z najwyższych wzrostów gospodarczych na świecie.",
                "Drugi najludniejszy kraj na świecie.", "Na jego terenie znajduje się widziany z kosmosu Wielki Mur.", "Jedne z największych miast to Szanghaj i Pekin." }),
            new Kraj("Włochy", new List<string> { "Jest jednym z największych producentów wina na świecie, z regionami takimi jak Toskania.", "Miejsce narodzin renesansu, gdzie żyli tacy artyści jak Michał Anioł.",
                "Ma dostęp do Morza Adriatyckiego.", "Ma kształt \"buta\", co czyni go jednym z najbardziej rozpoznawalnych krajów na mapie.", "Zjesz tam na pewno dobrą pizzę." }),
            new Kraj("Tajlandia", new List<string> {"Ten kraj jest jednym z głównych producentów ryżu na świecie.", "Zachował swoje tradycje buddyjskie, a buddyzm jest dominującą religią.",
                "W kraju tym znajduje się słynna wyspa Phuket, znana z turystyki.", "Jest znany z wyjątkowej kuchni, gdzie królują dania takie jak Pad Thai.", "Stolicą jest Bangkok."})
        })
        { }

        public QuizKrajow(List<IKraj> listaKrajow)
        {
            kraje = listaKrajow;
        }

        public void RozpocznijNowaGre()
        {
            Zycia = 3;
            Wynik = 0;
            nieodgadnieteKraje = new List<IKraj>(kraje);
            RozpocznijNowaRunde();
        }

        public int AktualnaRundaNumer
        {
            get
            {
                return AktualnaRunda != null ? ((RundaQuizu)AktualnaRunda).NumerRundy : 0;
            }
        }

        public void RozpocznijNowaRunde()
        {
            if (nieodgadnieteKraje.Count == 0)
            {
                AktualnaRunda = null;
                return;
            }
            var kraj = nieodgadnieteKraje.OrderBy(k => losowy.Next()).First();
            AktualnaRunda = new RundaQuizu(kraj);
            NumerRundy++;
        }

        public bool PrzeslijOdpowiedz(string odpowiedz, out int punkty)
        {
            if (AktualnaRunda.SprawdzOdpowiedz(odpowiedz))
            {
                punkty = AktualnaRunda.AktualnePunkty;
                Wynik += punkty;
                PoprawnaOdpowiedz?.Invoke(punkty);
                nieodgadnieteKraje.Remove(((RundaQuizu)AktualnaRunda).Kraj);
                return true;
            }
            else
            {
                Zycia--;
                if (Zycia <= 0)
                    throw new BrakZyciaException();
                UtraconoZycie?.Invoke();
                punkty = 0;
                return false;
            }
        }

        public int Punkty => Wynik;
        public bool CzyPrzegrana => Zycia <= 0;
        public bool CzyWygrana => nieodgadnieteKraje.Count == 0 && Zycia > 0;

        public bool SprawdzOdpowiedz(string odpowiedz)
        {
            return PrzeslijOdpowiedz(odpowiedz, out _);
        }
    }
}
