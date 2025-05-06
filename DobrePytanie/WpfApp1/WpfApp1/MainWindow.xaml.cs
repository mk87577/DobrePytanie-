using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private QuizKrajow quiz;

        public MainWindow()
        {
            InitializeComponent();
            quiz = new QuizKrajow();
            quiz.RozpocznijNowaGre();
            RozpocznijRunde();
        }

        private void RozpocznijRunde()
        {
            textBoxOdpowiedz.Clear();
            textBlockNotka.Text = $"Zgadnij kraj na podstawie podpowiedzi ({quiz.AktualnaRunda.AktualnePunkty} pkt):\n{quiz.AktualnaRunda.AktualnaPodpowiedź}";
            textBlockNumerRundy.Text = $"Runda: {quiz.NumerRundy}";
            textBlockWynik.Text = $"Wynik: {quiz.Punkty}";
            textBlockZycia.Inlines.Clear();
            textBlockZycia.Inlines.Add(new Run("Życia: "));
            for (int i = 0; i < quiz.Zycia; i++)
            {
                var serce = new Run("❤");
                serce.Foreground = new SolidColorBrush(Colors.Red);
                textBlockZycia.Inlines.Add(serce);
            }
            buttonNastepne.IsEnabled = false;
        }

        private void buttonZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            string odp = textBoxOdpowiedz.Text.Trim();
            try
            {
                if (quiz.SprawdzOdpowiedz(odp))
                {
                    MessageBox.Show("Poprawna odpowiedź!");
                    buttonNastepne.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Niepoprawna odpowiedź.");
                    textBlockZycia.Inlines.Clear();
                    textBlockZycia.Inlines.Add(new Run("Życia: "));
                    for (int i = 0; i < quiz.Zycia; i++)
                    {
                        var serce = new Run("❤");
                        serce.Foreground = new SolidColorBrush(Colors.Red);
                        textBlockZycia.Inlines.Add(serce);
                    }
                }
                textBlockNumerRundy.Text = $"Runda: {quiz.NumerRundy}";
                textBlockWynik.Text = $"Wynik: {quiz.Punkty}";
            }
            catch (BrakZyciaException)
            {
                MessageBox.Show($"Koniec gry! Zdobyłeś: {quiz.Punkty} pkt! Kliknij 'Nowa gra', aby spróbować ponownie.");
                buttonZatwierdz.IsEnabled = false;
                buttonNieWiem.IsEnabled = false;
                buttonNastepne.IsEnabled = false;
                textBlockZycia.Text = $"Życia: {new string('❤', quiz.Zycia)}";
            }
        }


        private void buttonNieWiem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                quiz.AktualnaRunda.PrzejdzDoLatwiejszegoEtapu();
                textBlockNotka.Text = $"Zgadnij kraj na podstawie podpowiedzi ({quiz.AktualnaRunda.AktualnePunkty} pkt):\n{quiz.AktualnaRunda.AktualnaPodpowiedź}";

                if (quiz.AktualnaRunda.CzyKoniecPodpowiedzi)
                {
                    MessageBox.Show("Koniec podpowiedzi! Ta jest ostatnią! Kliknij 'Następne' jeśli nie wiesz albo stracisz jedno życie!");
                    buttonNastepne.IsEnabled = true;
                }
            }
            catch (BrakPodpowiedziException)
            {
                MessageBox.Show($"Brak kolejnych podpowiedzi! Poprawna odpowiedź to: {quiz.AktualnaRunda.Kraj.Nazwa}. Tracisz jedno życie!");
                try
                {
                    quiz.PrzeslijOdpowiedz("", out _);
                }
                catch (BrakZyciaException)
                {
                    MessageBox.Show($"Koniec gry! Zdobyłeś: {quiz.Punkty} pkt! Kliknij 'Nowa gra', aby spróbować ponownie.");
                    buttonZatwierdz.IsEnabled = false;
                    buttonNieWiem.IsEnabled = false;
                    buttonNastepne.IsEnabled = false;
                    textBlockZycia.Text = "Pozostałe życia: 0";
                    return;
                }
                textBlockZycia.Inlines.Clear();
                textBlockZycia.Inlines.Add(new Run("Życia: "));
                for (int i = 0; i < quiz.Zycia; i++)
                {
                    var serce = new Run("❤");
                    serce.Foreground = new SolidColorBrush(Colors.Red);
                    textBlockZycia.Inlines.Add(serce);
                }

                if (quiz.CzyPrzegrana)
                {
                    MessageBox.Show($"Koniec gry! Zdobyłeś: {quiz.Punkty} pkt! Kliknij 'Nowa gra', aby spróbować ponownie.");
                    buttonZatwierdz.IsEnabled = false;
                    buttonNieWiem.IsEnabled = false;
                    buttonNastepne.IsEnabled = false;
                }
                buttonNastepne.IsEnabled = true;
            }
        }


        private void buttonNastepne_Click(object sender, RoutedEventArgs e)
        {
            if (!quiz.CzyPrzegrana)
            {
                quiz.RozpocznijNowaRunde();

                if (quiz.CzyWygrana)
                {
                    MessageBox.Show($"Gratulacje, wygrałeś! Udało Ci się odgadnąć wszystkie kraje i zdobyłeś {quiz.Punkty} pkt!");
                    buttonZatwierdz.IsEnabled = false;
                    buttonNieWiem.IsEnabled = false;
                    buttonNastepne.IsEnabled = false;
                    return;
                }
                RozpocznijRunde();
                buttonZatwierdz.IsEnabled = true;
                buttonNieWiem.IsEnabled = true;
            }
        }

        private void buttonNowaGra_Click(object sender, RoutedEventArgs e)
        {
            quiz = new QuizKrajow();
            quiz.RozpocznijNowaGre();
            RozpocznijRunde();
            buttonZatwierdz.IsEnabled = true;
            buttonNieWiem.IsEnabled = true;
        }
    }
}