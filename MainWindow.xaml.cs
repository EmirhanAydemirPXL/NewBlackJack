using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;

namespace NewBlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        IDictionary<string, int> kaarten = new Dictionary<string, int>();
        StringBuilder sbSpeler = new StringBuilder();
        StringBuilder sbBank = new StringBuilder();
        List<String> kaartenList = new List<String>();       
        DispatcherTimer timer = new DispatcherTimer();
        StringBuilder sbHistoriek = new StringBuilder();
        List<String> historiek = new List<String>();
        
        
        int aantalKeerHitGedrukt;
        int labelOutputSpelerPunten;
        int labelOutputBankPunten;
        string getrokkenKaart;
        int ronde;
        int count;
        int startKapitaal = 100;
        

        int aasTellerSpeler = 0;
        int aasTellerBank = 0;


        //string vervangVariabel;


        public MainWindow()
        {
            InitializeComponent();

            timer.Tick += Klok_Tick;

            timer.Interval = new TimeSpan(0, 0, 0, 1);

            timer.Start();

            #region kaartenlijst

            kaarten.Add("schoppen boer", 10);
            kaarten.Add("schoppen vrouw", 10);
            kaarten.Add("schoppen heer", 10);

            kaarten.Add("harten boer", 10);
            kaarten.Add("harten vrouw", 10);
            kaarten.Add("harten heer", 10);

            kaarten.Add("klaveren boer", 10);
            kaarten.Add("klaveren vrouw", 10);
            kaarten.Add("klaveren heer", 10);

            kaarten.Add("ruiten boer", 10);
            kaarten.Add("ruiten vrouw", 10);
            kaarten.Add("ruiten heer", 10);

            kaarten.Add("klaveren aas", 11);
            kaarten.Add("harten aas", 11);
            kaarten.Add("ruiten aas", 11);
            kaarten.Add("schoppen aas", 11);

            kaarten.Add("schoppen 2", 2);
            kaarten.Add("schoppen 3", 3);
            kaarten.Add("schoppen 4", 4);
            kaarten.Add("schoppen 5", 5);
            kaarten.Add("schoppen 6", 6);
            kaarten.Add("schoppen 7", 7);
            kaarten.Add("schoppen 8", 8);
            kaarten.Add("schoppen 9", 9);
            kaarten.Add("schoppen 10", 10);

            kaarten.Add("harten 2", 2);
            kaarten.Add("harten 3", 3);
            kaarten.Add("harten 4", 4);
            kaarten.Add("harten 5", 5);
            kaarten.Add("harten 6", 6);
            kaarten.Add("harten 7", 7);
            kaarten.Add("harten 8", 8);
            kaarten.Add("harten 9", 9);
            kaarten.Add("harten 10", 10);

            kaarten.Add("klaveren 2", 2);
            kaarten.Add("klaveren 3", 3);
            kaarten.Add("klaveren 4", 4);
            kaarten.Add("klaveren 5", 5);
            kaarten.Add("klaveren 6", 6);
            kaarten.Add("klaveren 7", 7);
            kaarten.Add("klaveren 8", 8);
            kaarten.Add("klaveren 9", 9);
            kaarten.Add("klaveren 10", 10);

            kaarten.Add("ruiten 2", 2);
            kaarten.Add("ruiten 3", 3);
            kaarten.Add("ruiten 4", 4);
            kaarten.Add("ruiten 5", 5);
            kaarten.Add("ruiten 6", 6);
            kaarten.Add("ruiten 7", 7);
            kaarten.Add("ruiten 8", 8);
            kaarten.Add("ruiten 9", 9);
            kaarten.Add("ruiten 10", 10);

            

            kaartenList = kaarten.Keys.ToList();
            #endregion

            NewGame();
        }

        
        private void deelKaart_Click(object sender, RoutedEventArgs e)
        {

            TestPuntenSysteem();

        }

        private void HitKaart_Click(object sender, RoutedEventArgs e)
        {
            

            VerdeelKaartenVoorHit();

        }


        private void StandKaart_Click(object sender, RoutedEventArgs e)
        {
            

            VerdeelKaartenVoorStand();
        

        }

        private void BtnNieuweSpel_Click(object sender, RoutedEventArgs e)
        {
            

            NewGame();
        }



        private bool isAce(int card)
        {
            if (card == 11)
            {

                return true;
            }
            else
            {
                return false;
            }
        }





        private void TestPuntenSysteem()
        {
            //Hier kijkt men of de inzet minimaal 10% van de kapitaal is, en kijkt men ook of de inzet groter is dan de kapitaal of niet.
            if (Convert.ToInt64(inzetInput.Text) >= (int)kapitaalOutput.Content * 0.1 && Convert.ToInt32(inzetInput.Text) <= (int)kapitaalOutput.Content)
            {
                
                VerdeelKaarten();
                HitKaart.IsEnabled = true ;
                StandKaart.IsEnabled = true ;
                 
            }
            else
            {
                if (Convert.ToInt32(inzetInput.Text) < (int)kapitaalOutput.Content * 0.1)
                {
                    MessageBox.Show("inzet moet minimaal 10% van je kapitaal zijn");
                }
                else if (Convert.ToInt32(inzetInput.Text) > (int)kapitaalOutput.Content)
                {
                    MessageBox.Show("je hebt geen " + Convert.ToInt32(inzetInput.Text) + " punten beschikbaar");
                }
                
                
                
            }
        }



        private void VerdeelKaarten()
        {
            count = kaartenList.Count;
            bool isTrueVoorSpeler = false;
            

            //In deze for loop worden de eerst 2 kaarten van de speler en Bank verdeeld.
            
            for (int i = 1; i < 3; i++)
            {
                getrokkenKaart = kaartenList[random.Next(1, count - 1)];

                labelOutputSpelerPunten += kaarten[getrokkenKaart];



                // Hier Controleerd de methode isAce() of de getrokken kaart een aas is.

                if (isAce(kaarten[getrokkenKaart]))
                    {
                        aasTellerSpeler++;

                    }

                //hieronder wordt er gekeken of de eerste 2 kaarten asen zijn; als beide asen zijn moet ééntje de waarde 1 hebben en de andere 11.

                if (i == 1)
                    {
                        if (kaarten[getrokkenKaart] == 11)
                        {
                            isTrueVoorSpeler = true;
                        }
                    }

                if (i == 2)
                    {
                        if (isTrueVoorSpeler)
                        {
                            if (kaarten[getrokkenKaart] == 11)
                            {
                                labelOutputSpelerPunten -= 10;
                            }
                        }
                    }


                sbSpeler.AppendLine(getrokkenKaart);

                
                

                if (i == 1)
                    {
                        

                        imageCards1.Source = new BitmapImage(new Uri("kaarten/" + getrokkenKaart + ".png", UriKind.Relative));
                    }
                else if (i == 2)
                    {
                        
                        imageCards2.Source = new BitmapImage(new Uri("kaarten/" + getrokkenKaart + ".png", UriKind.Relative));
                    }


                //getrokken kaart wordt hiet verwijderd van de dek
                kaartenList.Remove(getrokkenKaart);

                
                getrokkenKaart = kaartenList[random.Next(1, count - 1)];

                if (isAce(kaarten[getrokkenKaart]))
                    {
                        aasTellerBank++;

                    }


                
                if (i == 1)
                    {
                        
                        imageCards3.Source = new BitmapImage(new Uri("kaarten/" + getrokkenKaart + ".png", UriKind.Relative));
                        labelOutputBankPunten += kaarten[getrokkenKaart];
                        sbBank.AppendLine(getrokkenKaart);
                        kaartenList.Remove(getrokkenKaart);
                    }
                else
                    {
                    
                    imageCards4.Source = new BitmapImage(new Uri("kaarten/back.jpg", UriKind.Relative));


                    }

                
            }

           

            //nadat de kaarten zijn gedeelt kan de hit button enabled worden
            deelKaart.IsEnabled = false;



            //hier worden de punten van de speler en bank toegevoegd aan xaml
            puntenSpeler.Content = labelOutputSpelerPunten;
            puntenBank.Content = labelOutputBankPunten;


            // hier worden de kaart namen toegevoegd aan de output in xaml
            KaartenTextOutputSpeler.Content += sbSpeler.ToString();
            KaartenTextOutputBank.Content += sbBank.ToString();
            
            
            if (labelOutputSpelerPunten == 21)
            {
                MessageBox.Show("speler heeft gewonnen");
                HitKaart.IsEnabled = false;
                StandKaart.IsEnabled = false;
                kapitaalOutput.Content = Convert.ToInt32(kapitaalOutput.Content) + Convert.ToInt32(inzetInput.Text);
                NewRound();
            }


            //stringbuilder wordt verwijderd om vervolgens opnieuw te gebruiken (anders staat het er dubbel)
            sbSpeler.Clear();
            sbBank.Clear();

            

            // als je op dubbel down wilt drukken moet dit ook mogelijk moeten zijn; dus als de kapitaal bv. 100 is, mag de inzet niet meer dan 50 zijn
            //als dat toch zo is, wordt de double down button niet Enabled.
            if (Convert.ToInt32(inzetInput.Text) * 2 > Convert.ToInt32(kapitaalOutput.Content))
            {
                dubbeldownBtn.IsEnabled = false;
            }
            else
            {
                dubbeldownBtn.IsEnabled = true;

            }
        }


        private void VerdeelKaartenVoorHit()
        {
            dubbeldownBtn.IsEnabled = false;


            // hier heb ik geen for loop gebruikt, dus gebruik ik hier aantalKeerHitGedrukt zodat ik de kaarten op verschillende plaatsen kan zetten.
            aantalKeerHitGedrukt++;

            count = kaartenList.Count;


            getrokkenKaart = kaartenList[random.Next(1, count - 1)];

            labelOutputSpelerPunten += kaarten[getrokkenKaart];



            //kijkt of de getrokken kaart aas is.
            if (isAce(kaarten[getrokkenKaart]))
            {
                aasTellerSpeler++;
            }

            //veranderd de waarde van aas als de punten van de speler groter dan 21 is.
            if (labelOutputSpelerPunten > 21 && aasTellerSpeler > 0)
            {
                labelOutputSpelerPunten -= 10;

                //hier maak ik de aas teller 0, anders gaat de score nooit boven 21
                aasTellerSpeler = 0;

            }


            
            if (aantalKeerHitGedrukt == 1)
            {
                imageCards5.Source = new BitmapImage(new Uri("kaarten/" + getrokkenKaart + ".png", UriKind.Relative));
            }
            else if (aantalKeerHitGedrukt == 2)
            {
                imageCards6.Source = new BitmapImage(new Uri("kaarten/" + getrokkenKaart + ".png", UriKind.Relative));
            }



            sbSpeler.AppendLine(getrokkenKaart);

            kaartenList.Remove(getrokkenKaart);

            KaartenTextOutputSpeler.Content += sbSpeler.ToString();

            sbSpeler.Clear();


            


            puntenSpeler.Content = labelOutputSpelerPunten;
            puntenBank.Content = labelOutputBankPunten;



            // als de speler meer dan 21 punten heeft, stopt de ronde en de inzet en kapitaal wordt gewijzigd.
            if (labelOutputSpelerPunten > 21)
            {

                HitKaart.IsEnabled = false;
                StandKaart.IsEnabled = false;
                MessageBox.Show("je hebt verloren tegen de bank!");

                // kapitaal wordt meer
                kapitaalOutput.Content = (int)kapitaalOutput.Content - Convert.ToInt32(inzetInput.Text)  ;


                // hier voeg ik aan de output van historiekRonde welke zonde dit spel is, de punten van de speler en de bank
                historiekRonde.Content = $"Ronde {ronde} inzet: {inzetInput.Text}\nSpeler punten: {labelOutputSpelerPunten}\nBank punten: {labelOutputBankPunten}";

                //hier voeg ik dat toe aan een lijst om later alle ronde's te kunnen zien.
                historiek.Add($"Ronde {ronde} inzet: {inzetInput.Text} Speler punten: {labelOutputSpelerPunten} Bank punten: {labelOutputBankPunten}");

                //nieuwe ronde
                NewRound();
            }
        }


        private void VerdeelKaartenVoorStand()
        {
            count = kaartenList.Count;

            string getrokkenKaart;
            HitKaart.IsEnabled = false;
            dubbeldownBtn.IsEnabled = false;
            int tellerWhile = 1;


            // in deze while lus worden er kaarten getrokken tot de punt van de bank groter dan 16 is.
            while (labelOutputBankPunten < 17)
            {

                getrokkenKaart = kaartenList[random.Next(1, count - 1)];

                labelOutputBankPunten += kaarten[getrokkenKaart];

                sbBank.AppendLine(getrokkenKaart);



                if (tellerWhile == 1)
                {
                    imageCards4.Source = new BitmapImage(new Uri("kaarten/" + getrokkenKaart + ".png", UriKind.Relative));
                }
                else if (tellerWhile == 2)
                {
                    imageCards7.Source = new BitmapImage(new Uri("kaarten/" + getrokkenKaart + ".png", UriKind.Relative));
                }
                else
                {
                    imageCards8.Source = new BitmapImage(new Uri("kaarten/" + getrokkenKaart + ".png", UriKind.Relative));
                }


                // kijkt of getrokken kaart aas is.
                if (isAce(kaarten[getrokkenKaart]))
                {
                    aasTellerBank++;
                }

                //als er aas is en de punten van de bank meer da 21 is wordt de waarde van één aas 1, als het nodig is kan ook de 2de aas als 1 gewijzigd worden.
                if (labelOutputBankPunten > 21 && aasTellerBank > 0)
                {
                    labelOutputBankPunten -= 10;

                    //hier maak ik de aas teller 0, anders gaat de score nooit boven 21
                    aasTellerBank = 0;

                }


                kaartenList.Remove(getrokkenKaart);

                tellerWhile++;


            }



            KaartenTextOutputBank.Content += sbBank.ToString();

            puntenBank.Content = labelOutputBankPunten;


            //als de punten van de bank en de spele onder of gelijk aan 21 is, wordt de punten verdeling gemaakt
            if (labelOutputBankPunten <= 21 && labelOutputSpelerPunten <= 21)
            {
                if (labelOutputBankPunten > labelOutputSpelerPunten)
                {
                    // als de bank meer punten heeft dan de speler en is niet boven 21 dan gaat de kapitaal verminderen van de speler

                    MessageBox.Show("bank heeft gewonnen");
                    
                    HitKaart.IsEnabled = false;
                    StandKaart.IsEnabled = false;

                    
                    kapitaalOutput.Content = (int)kapitaalOutput.Content - Convert.ToInt32(inzetInput.Text);
                    
                    
                    

                    historiekRonde.Content = $"Ronde {ronde} inzet: {inzetInput.Text}\nSpeler punten: {labelOutputSpelerPunten}\nBank punten: {labelOutputBankPunten}";
                    historiek.Add($"Ronde {ronde} inzet: {inzetInput.Text} Speler punten: {labelOutputSpelerPunten} Bank punten: {labelOutputBankPunten}");


                    NewRound();
                    
                }
                else if (labelOutputBankPunten < labelOutputSpelerPunten)
                {
                    // als de speler meer punten heeft dan de bank en is niet boven 21 dan gaat de kapitaal vermeerderen van de speler
                    MessageBox.Show("speler heeft gewonnen");
                    
                    

                    
                    kapitaalOutput.Content = (int)kapitaalOutput.Content + Convert.ToInt32(inzetInput.Text);
                    
                    
                    
                    


                    HitKaart.IsEnabled = false;
                    StandKaart.IsEnabled = false;
                    historiekRonde.Content = $"Ronde {ronde} inzet: {inzetInput.Text}\nSpeler punten: {labelOutputSpelerPunten}\nBank punten: {labelOutputBankPunten}";
                    historiek.Add($"Ronde {ronde} inzet: {inzetInput.Text} Speler punten: {labelOutputSpelerPunten} Bank punten: {labelOutputBankPunten}");

                    NewRound();
                }
                else
                {
                    // als de punten gelijk zijn dan krijgt de speler de ingezette geld terug (wordt geen wijziging gemaakt voor de kapitaal)
                    MessageBox.Show("niemand heeft gewonnen! je krijgt je ingezette geld terug!");
                    

                    
                    HitKaart.IsEnabled = false;
                    StandKaart.IsEnabled = false;
                    historiekRonde.Content = $"Ronde {ronde} inzet: {inzetInput.Text}\nSpeler punten: {labelOutputSpelerPunten}\nBank punten: {labelOutputBankPunten}";
                    historiek.Add($"Ronde {ronde} inzet: {inzetInput.Text} Speler punten: {labelOutputSpelerPunten} Bank punten: {labelOutputBankPunten}");

                    NewRound();
                }
            }
            // als de score van de bank groter dan 21 word, wint de speler en krijgt wijziging voor de kapitaal.
            else if (labelOutputBankPunten > 21)
            {
                MessageBox.Show("speler heeft gewonnen");
                kapitaalOutput.Content = Convert.ToInt32(kapitaalOutput.Content) + Convert.ToInt32(inzetInput.Text);
                historiekRonde.Content = $"Ronde {ronde} inzet: {inzetInput.Text}\nSpeler punten: {labelOutputSpelerPunten}\nBank punten: {labelOutputBankPunten}";
                historiek.Add($"Ronde {ronde} inzet: {inzetInput.Text} Speler punten: {labelOutputSpelerPunten} Bank punten: {labelOutputBankPunten}");

                NewRound();
            }

            // als de kapitaal 0 is start er een nieuwe ronde
            if (Convert.ToInt32(kapitaalOutput.Content) <= 0)
            {
                MessageBox.Show("je hebt geen kapitaal meer, nieuwe ronde wordt gestart");
                NewRound();
            }


            

            
            
        }



        /// <summary>
        /// alles wordt verwijderd behalve de kapitaal, de rondes, ronde informaties. 
        /// </summary>
        private void NewRound()
        {
            

            
            ronde++;
            rondeTeller.Content = ronde;

            imageCards1.Source = null;
            imageCards2.Source = null;
            imageCards3.Source = null;
            imageCards4.Source = null;
            imageCards5.Source = null;
            imageCards6.Source = null;
            imageCards7.Source = null;
            imageCards8.Source = null;

            labelOutputSpelerPunten = 0;
            labelOutputBankPunten = 0;

            puntenBank.Content = 0;
            puntenSpeler.Content = 0;

            kaartenList = kaarten.Keys.ToList();

            deelKaart.IsEnabled = true;
            HitKaart.IsEnabled = false;
            StandKaart.IsEnabled = false;

            sbSpeler.Clear();
            sbBank.Clear();

            aantalKeerHitGedrukt = 0;

            KaartenTextOutputBank.Content = null;
            KaartenTextOutputSpeler.Content = null;

            dubbeldownBtn.IsEnabled = false;

            
        }

        /// <summary>
        /// alles wordt verwijderd en op nul gezet
        /// </summary>
        private void NewGame()
        {


            historiek.Clear();
            historiekRonde.Content = "";

            dubbeldownBtn.IsEnabled = false;

            ronde = 1;
            rondeTeller.Content= ronde;

            deelKaart.IsEnabled = true;
            HitKaart.IsEnabled = false;
            StandKaart.IsEnabled = false;    

            KaartenTextOutputBank.Content = null;
            KaartenTextOutputSpeler.Content = null;

            kaartenList = kaarten.Keys.ToList();


            sbSpeler.Clear();
            sbBank.Clear();


            



            aantalKeerHitGedrukt = 0;

            labelOutputSpelerPunten = 0;
            labelOutputBankPunten = 0;


            

            

            puntenBank.Content = 0;
            puntenSpeler.Content = 0;


            imageCards1.Source = null;
            imageCards2.Source = null;
            imageCards3.Source = null;
            imageCards4.Source = null;
            imageCards5.Source = null;
            imageCards6.Source = null;  
            imageCards7.Source = null;
            imageCards8.Source = null;

            kapitaalOutput.Content = startKapitaal;
            inzetInput.Text = ( startKapitaal / 10).ToString();





        }


        /// <summary>
        /// de klok wordt hier aangemaakt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Klok_Tick(object sender, EventArgs e)
        {
            
            TxtTijd.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// informatie over alles rondes
        /// 
        /// </summary>
        /// <para> lijst word aangemaakt, wordt gereversed en wordt in de message box zichtbaar.</para>
        private void HistoriekShow()
        {
            historiek.Reverse();
            foreach (string item in historiek)
            {
                sbHistoriek.AppendLine(item);

            }
            MessageBox.Show(sbHistoriek.ToString());
            sbHistoriek.Clear();
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            HistoriekShow();
        }

        private void Klik_Doorlopend_Ronde(object sender, MouseButtonEventArgs e)
        {
            HistoriekShow();
        }

        private void HistoriekRonde_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            HistoriekShow();
        }


        /// <summary>
        /// maakt de inzet dubbel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DubbeldownBtn_Click(object sender, RoutedEventArgs e)
        {
            inzetInput.Text = (Convert.ToInt32(inzetInput.Text) * 2).ToString();
            HitKaart.IsEnabled = false;
            dubbeldownBtn.IsEnabled = false;

        }
    }
}



