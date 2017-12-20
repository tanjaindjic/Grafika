using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharpGL.SceneGraph;
using SharpGL;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }


        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;
        public static float brzinaRotacije = 2;
        public ObservableCollection<float> BrzinaRotacije
        {
            get;
            set;
        }

        public static double skaliranje = 0.3;
        public ObservableCollection<double> Skaliranje
        {
            get;
            set;
        }

       
      

        public ObservableCollection<string> AmbijentalnaKomponenta
        {
            get;
            set;
        }
        #endregion Atributi

        #region Konstruktori


        //DRUGA TACKA
        public MainWindow()
        {

            BrzinaRotacije = new ObservableCollection<float>();
            BrzinaRotacije.Add(2);
            BrzinaRotacije.Add(5);
            BrzinaRotacije.Add(10);
            BrzinaRotacije.Add(15);
            BrzinaRotacije.Add(20);
            BrzinaRotacije.Add(25);
            BrzinaRotacije.Add(30);
            BrzinaRotacije.Add(35);
            BrzinaRotacije.Add(40);
            BrzinaRotacije.Add(45);

            Skaliranje = new ObservableCollection<double>();
            Skaliranje.Add(0.1);
            Skaliranje.Add(0.2);
            Skaliranje.Add(0.3);
            Skaliranje.Add(0.4);
            Skaliranje.Add(0.5);
            Skaliranje.Add(0.6);
            Skaliranje.Add(0.7);
            Skaliranje.Add(0.8);
            Skaliranje.Add(0.9);
            Skaliranje.Add(1);


            AmbijentalnaKomponenta = new ObservableCollection<string>();
            AmbijentalnaKomponenta.Add("Plava");
            AmbijentalnaKomponenta.Add("Crvena");
            AmbijentalnaKomponenta.Add("Zelena");
            AmbijentalnaKomponenta.Add("Bela");
            // Inicijalizacija komponenti
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.DataContext = this;
            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Ball"), "Ball N130816.3DS", (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
            }
            catch (Exception e)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta. Poruka greške: " + e.Message, "Poruka", MessageBoxButton.OK);
                this.Close();
            }

          
        }

       


        #endregion Konstruktori

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            m_world.Draw(args.OpenGL);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {

                case Key.F4: this.Close(); break;
                case Key.D:
                    if(!m_world.JumpStop && m_world.RotationX<5)
                        m_world.RotationX += 5.0f;
                    break;
                case Key.E:
                    if (!m_world.JumpStop && m_world.RotationX > -70)
                        m_world.RotationX -= 5.0f;
                    break;
                case Key.S:
                    if (!m_world.JumpStop)
                        m_world.RotationY -= 5.0f;
                    break;
                case Key.F:
                    if (!m_world.JumpStop)
                        m_world.RotationY += 5.0f;
                    break;
                case Key.Add:
                    if (!m_world.JumpStop)
                        if (!m_world.JumpStop) m_world.SceneDistance -= 50;
                    break;
                case Key.Subtract:
                    if (!m_world.JumpStop)
                        m_world.SceneDistance += 50;
                    break;
                case Key.V: if (!m_world.JumpStop)
                        m_world.sutniLoptu();
                    break;
                case Key.X:
                    if (!m_world.JumpStop)
                        m_world.XSvetlo += 10;
                    break;
                case Key.Y:
                    if (!m_world.JumpStop)
                        m_world.YSvetlo += 10;
                    break;
                case Key.Z:
                    if (!m_world.JumpStop)
                        m_world.ZSvetlo -= 50;
                    break;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (rotacijaCombo.SelectedIndex.Equals(-1))
                brzinaRotacije = 10;

            else if (rotacijaCombo.SelectedIndex == 0)
            {
                int idx = BrzinaRotacije.IndexOf(2);
                brzinaRotacije = BrzinaRotacije[idx];
             //   m_world.m_yRotateBall = brzinaRotacije;
            }
            else if (rotacijaCombo.SelectedIndex == 1)
            {
                int idx = BrzinaRotacije.IndexOf(5);
                brzinaRotacije = BrzinaRotacije[idx];
             //   m_world.m_yRotateBall = brzinaRotacije;
            }
            else if (rotacijaCombo.SelectedIndex==2)
            {
                int idx = BrzinaRotacije.IndexOf(10);
                brzinaRotacije = BrzinaRotacije[idx];
            //    m_world.m_yRotateBall = brzinaRotacije;
            }
            else if (rotacijaCombo.SelectedIndex == 3)
            {
                int idx = BrzinaRotacije.IndexOf(15);
                brzinaRotacije = BrzinaRotacije[idx];
             //   m_world.m_yRotateBall = brzinaRotacije;
            }
            else if (rotacijaCombo.SelectedIndex == 4)
            {
                int idx = BrzinaRotacije.IndexOf(20);
                brzinaRotacije = BrzinaRotacije[idx];
             //   m_world.m_yRotateBall = brzinaRotacije;
            }
            else if (rotacijaCombo.SelectedIndex ==5)
            {
                int idx = BrzinaRotacije.IndexOf(25);
                brzinaRotacije = BrzinaRotacije[idx];
            //    m_world.m_yRotateBall = brzinaRotacije;
            }
            else if (rotacijaCombo.SelectedIndex ==6)
            {
                int idx = BrzinaRotacije.IndexOf(30);
                brzinaRotacije = BrzinaRotacije[idx];
             //   m_world.m_yRotateBall = brzinaRotacije;
            }
            else if (rotacijaCombo.SelectedIndex ==7)
            {
                int idx = BrzinaRotacije.IndexOf(35);
                brzinaRotacije = BrzinaRotacije[idx];
           //     m_world.m_yRotateBall = brzinaRotacije;
            }
            else if (rotacijaCombo.SelectedIndex ==8)
            {
                int idx = BrzinaRotacije.IndexOf(40);
                brzinaRotacije = BrzinaRotacije[idx];
             //   m_world.m_yRotateBall = brzinaRotacije;
            }
            else if (rotacijaCombo.SelectedIndex ==9)
            {
                int idx = BrzinaRotacije.IndexOf(45);
                brzinaRotacije = BrzinaRotacije[idx];
             //   m_world.m_yRotateBall = brzinaRotacije;
            }
        }

        private void skaliranjeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
            if (skaliranjeCombo.SelectedIndex.Equals(-1))
                skaliranje = 0.3;

            else if (skaliranjeCombo.SelectedItem.Equals(0.1))
            {
                int idx = Skaliranje.IndexOf(0.1);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }
            else if (skaliranjeCombo.SelectedItem.Equals(0.2))
            {
                int idx = Skaliranje.IndexOf(0.2);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }
            else if (skaliranjeCombo.SelectedItem.Equals(0.3))
            {
                int idx = Skaliranje.IndexOf(0.3);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }
            else if (skaliranjeCombo.SelectedItem.Equals(0.4))
            {
                int idx = Skaliranje.IndexOf(0.4);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }
            else if (skaliranjeCombo.SelectedItem.Equals(0.5))
            {
                int idx = Skaliranje.IndexOf(0.5);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }
            else if (skaliranjeCombo.SelectedItem.Equals(0.6))
            {
                int idx = Skaliranje.IndexOf(0.6);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }
            else if (skaliranjeCombo.SelectedItem.Equals(0.7))
            {
                int idx = Skaliranje.IndexOf(0.7);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }
            else if (skaliranjeCombo.SelectedItem.Equals(0.8))
            {
                int idx = Skaliranje.IndexOf(0.8);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }
            else if (skaliranjeCombo.SelectedItem.Equals(0.9))
            {
                int idx = Skaliranje.IndexOf(0.9);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }
            else if (skaliranjeCombo.SelectedItem.Equals(1))
            {
                int idx = Skaliranje.IndexOf(1);
                skaliranje = Skaliranje[idx];
                m_world.velicinaLopte = skaliranje;
            }

        }



        private void ambijentCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ambijentCombo.SelectedIndex.Equals(-1))
            {
                m_world.AmbijentalnaKomponenta2 = new float[4];
                m_world.AmbijentalnaKomponenta2[0] = 0.3f;
                m_world.AmbijentalnaKomponenta2[1] = 0.3f;
                m_world.AmbijentalnaKomponenta2[2] = 0.9f;
                m_world.AmbijentalnaKomponenta2[3] = 1f;
            }
               

            else if (ambijentCombo.SelectedItem.Equals("Plava"))
            {
                m_world.AmbijentalnaKomponenta2 = new float[4];
                m_world.AmbijentalnaKomponenta2[0] = 0.1f;
                m_world.AmbijentalnaKomponenta2[1] = 0.1f;
                m_world.AmbijentalnaKomponenta2[2] = 0.5f;
                m_world.AmbijentalnaKomponenta2[3] = 1f;
            }
            else if (ambijentCombo.SelectedItem.Equals("Crvena"))
            {
                m_world.AmbijentalnaKomponenta2 = new float[4];
                m_world.AmbijentalnaKomponenta2[0] = 0.5f;
                m_world.AmbijentalnaKomponenta2[1] = 0.1f;
                m_world.AmbijentalnaKomponenta2[2] = 0.1f;
                m_world.AmbijentalnaKomponenta2[3] = 1f;
            }
            else if (ambijentCombo.SelectedItem.Equals("Zelena"))
            {
                m_world.AmbijentalnaKomponenta2 = new float[4];
                m_world.AmbijentalnaKomponenta2[0] = 0.1f;
                m_world.AmbijentalnaKomponenta2[1] = 0.5f;
                m_world.AmbijentalnaKomponenta2[2] = 0.1f;
                m_world.AmbijentalnaKomponenta2[3] = 1f;
            }
            else if (ambijentCombo.SelectedItem.Equals("Bela"))
            {
                m_world.AmbijentalnaKomponenta2 = new float[4];
                m_world.AmbijentalnaKomponenta2[0] = 0.9f;
                m_world.AmbijentalnaKomponenta2[1] = 0.9f;
                m_world.AmbijentalnaKomponenta2[2] = 0.9f;
                m_world.AmbijentalnaKomponenta2[3] = 1f;
            }
        }

        private void tackastiIzvor_Checked(object sender, RoutedEventArgs e)
        {
            bool ch;
            if (ch = tackastiIzvor.IsChecked == true ? true : false)
            {
               m_world.UkljucenoTackasto = true;
            }
            else
            {
                m_world.UkljucenoTackasto = false;
            }
        }

        private void reflektorskiIzvor_Checked(object sender, RoutedEventArgs e)
        {
            bool ch;
            if (ch = reflektorskiIzvor.IsChecked == true ? true : false)
            {
                m_world.UkljucenoReflektor = true;
            }
            else
            {
                m_world.UkljucenoReflektor = false;
            }
        }

        private void dnevnoSvetlo_Checked(object sender, RoutedEventArgs e)
        {
            bool ch;
            if (ch = dnevnoSvetlo.IsChecked == true ? true : false)
            {
                m_world.DnevnoSvetlo = true;
            }
            else
            {
                m_world.DnevnoSvetlo = false;
            }
        }

        private void sutni_Click(object sender, RoutedEventArgs e)
        {
            m_world.sutniLoptu();
        }

        private void pocetak_Click(object sender, RoutedEventArgs e)
        {
            m_world.Timer2.Stop();
            m_world.BallHeight = -100f;
            m_world.Pos[0] = 0f;
            m_world.Pos[1] = m_world.BallHeight;
            m_world.Pos[2] = -200;

            m_world.BallGoingUp = true;
            m_world.JumpStop = false;
            m_world.YRotateBall = 0;
        }
    }
}
