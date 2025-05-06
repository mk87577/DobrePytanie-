namespace ClassLibrary1
{
    public interface IRundaQuizu
    {
        IKraj Kraj { get; }
        string AktualnaPodpowiedź { get; }
        int AktualnePunkty { get; }
        bool CzyMoznaPokazacWiecejPodpowiedzi { get; }

        void PokazNastepnaPodpowiedz();
        bool SprawdzOdpowiedz(string odpowiedz);
        bool CzyKoniecPodpowiedzi { get; }
        void PrzejdzDoLatwiejszegoEtapu();
    }
}
