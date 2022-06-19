using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MSEB_19753_Ampel
{
    public partial class Form1 : Form
    {

        //deklarieren
        private int sek;    //um die Ampel in Abhängikeit von Sekunden zu timen
        int zufallszahl;    //Zufallszahl für zufälliges einschalten der grünen Ampel
        private bool buttonDruck;   //zum überprüfen ob der throttle Button gedrückt worden ist
        private int reactTime;  //um dem Computer eine künstliche Reaktionszeit zu geben
        private int react = 600;   //um Prozess nach einer gewissen Zeit zurückzusetzen
        int counterRedbull, counterFerrari; //Siegerehrungsvariablen 


        //Ampelvariablen
        private int hoehe = 65;
        private int breite = 65;
        private int xKoordinate1 = 900;
        private int xKoordinate2 = 700;
        private int xKoordinate3 = 500;
        private int xKoordinate4 = 300;
        private int xKoordinate5 = 100;
        private int yKoordinate1 = 50;
        private int yKoordinate2 = 130;
        private int yKoordinate3 = 210;

        //Reaktionszeit
        bool reaktion;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = "Du willst wissen ob du es drauf hast F1 Fahrer zu sein? \nSetze dich ins Auto und drücke Throttle wenn alle Ampeln grün aufleuchten \nSchlage Leclerc in einem 1 vs 1 um dir die erste Kurve zu sichern!";
            MessageBox.Show(message);

            //Bild Anfangswert setzen und picturebox Bild zuweisen
            //12 279
            pictureBox1.Location = new Point(-200, 279);
            pictureBox1.Image = Properties.Resources.FerrariSketch_F1;

            // 12 417 Startposition
            pictureBox2.Location = new Point(-200, 417);
            pictureBox2.Image = Properties.Resources.RedbullSketch_F1;

            //Zufallszahl generieren für Startsequenz (grünes Leuchten)
            var rand = new Random();
            zufallszahl = rand.Next(4, 10);
            ////zufallszahl testen
            //label1.visible = true;
            //label1.Text = zufallszahl.ToString();


            //Autos in Position bringen 
            Foramationlap.Start();
            Ampeltimer.Stop();
            Racemode.Stop();
            reactTime = 0;

            buttonDruck = false;

            reaktion = false;

            //Am Anfang im Rennen alle Lichter rot an
            rotesLichtan();

            //sek wieder auf null setzen um Prozess neu zu starten
            sek = 0;

            //Siegerehrungsvariablen wieder auf null setzen
            counterRedbull = 0;
            counterFerrari = 0;

            //Timer konfigurieren 
            Racemode.Interval = 1;

            Ampeltimer.Interval = 1000;

            Foramationlap.Interval = 1;

            Reaktionszeit.Interval = 200;
        }

        private void Ampeltimer_Tick(object sender, EventArgs e)
        {
            sek++;

            //macht Licht in Abhängigkeit von Sekunden aus und grünes Licht an
            if (sek == 1) Lichtaus();
            else if (sek == 2) Lichtaus();
            else if (sek == 3) Lichtaus();
            else if (sek == zufallszahl)
            {
                gruenesLichtan();
                Ampeltimer.Stop();
                Racemode.Start();
                Reaktionszeit.Start();
            }
        }

        private void Foramationlap_Tick(object sender, EventArgs e)
        {
            //prüft mit jedem Tick die Position der PictureBox
            Control control = this.pictureBox1;
            Point controllocation = control.Location;

            if (controllocation.X == 12)
            {
                Foramationlap.Stop();
                Ampeltimer.Start();
            }
            else
            {
                pictureBox1.Location = new System.Drawing.Point(pictureBox1.Location.X + 1, pictureBox1.Location.Y);
                pictureBox2.Location = new System.Drawing.Point(pictureBox2.Location.X + 1, pictureBox2.Location.Y);
            }
        }

        private void Racemode_Tick(object sender, EventArgs e)
        {

            reactTime++;

            //Wenn Throttle gedrückt wird bewegt sich das Spieler Auto
            if (buttonDruck)
            {
                pictureBox2.Location = new System.Drawing.Point(pictureBox2.Location.X + 7, pictureBox2.Location.Y);
                counterRedbull++;
            }

            //Computergesteuertes Auto fährt los; 200 ms ist die durchschnittliche Reaktionszeit eines F1 Fahrers
            if (reaktion == true)
            {
                pictureBox1.Location = new System.Drawing.Point(pictureBox1.Location.X + 7, pictureBox1.Location.Y);
                Reaktionszeit.Stop();
                counterFerrari++;
            }

            //react = 700
            if (reactTime == react)
            {
                Racemode.Stop();
                if (counterRedbull > counterFerrari)
                {
                    MessageBox.Show("Gewonnen, das restliche Rennen hälst du die Führung mithilfe des stärksten F1 Boxenstopp Teams der Welt!");
                }
                else MessageBox.Show("Verloren, Christian Horner ist nicht begeistert");

                Lichtaus();
                //Bild Anfangswert setzen 
                pictureBox1.Location = new Point(-200, 279);
                pictureBox1.Image = Properties.Resources.FerrariSketch_F1;

                pictureBox2.Location = new Point(-200, 417);
                pictureBox2.Image = Properties.Resources.RedbullSketch_F1;

            }

        }

        //Tickt mit 200 ms -> Reaktionszeit F1 Fahrer
        private void Reaktionszeit_Tick(object sender, EventArgs e)
        {
            reaktion = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            buttonDruck = true;
        }



        /// <summary>
        /// Ampel konfigurieren 
        /// </summary>


        //rotes Licht konfigurieren 
        public void rotesLichtan()
        {

            SolidBrush Pinsel = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            System.Drawing.Graphics Kreis;
            Kreis = this.CreateGraphics();
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate3, breite, hoehe));

        }

        //gruenes Licht konfigurieren
        public void gruenesLichtan()
        {
            SolidBrush Pinsel = new System.Drawing.SolidBrush(System.Drawing.Color.LawnGreen);
            System.Drawing.Graphics Kreis;
            Kreis = this.CreateGraphics();
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate1, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate2, breite, hoehe));
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate3, breite, hoehe));
        }

        //Licht aus konfigurieren
        public void Lichtaus()
        {
            SolidBrush Pinsel = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.Graphics Kreis;
            Kreis = this.CreateGraphics();
            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate1, breite, hoehe));
            if (sek == 2) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate2, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate2, breite, hoehe));
            if (sek == 3) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate3, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate1, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate1, breite, hoehe));
            if (sek == 2) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate2, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate2, breite, hoehe));
            if (sek == 3) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate3, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate2, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate1, breite, hoehe));
            if (sek == 2) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate2, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate2, breite, hoehe));
            if (sek == 3) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate3, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate3, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate1, breite, hoehe));
            if (sek == 2) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate2, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate2, breite, hoehe));
            if (sek == 3) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate3, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate4, yKoordinate3, breite, hoehe));

            Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate1, breite, hoehe));
            if (sek == 2) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate2, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate2, breite, hoehe));
            if (sek == 3) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate3, breite, hoehe));
            else if (reactTime == react) Kreis.FillEllipse(Pinsel, new Rectangle(xKoordinate5, yKoordinate3, breite, hoehe));
        }

    }
}
