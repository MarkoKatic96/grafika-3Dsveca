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
using System.Text.RegularExpressions;

namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int rotacijaSvece = 0;
        int skaliranjepodloge = 0;
        String bojaSvetla = "";

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!World.startAnimation)
            {
                var textBox = sender as TextBox;
                if (Regex.IsMatch(textBox.Text, @"^[0-9]+$"))
                {
                    this.RotacijaSvece = Int32.Parse(textBox.Text);
                }
            }
        }

        private void TextBox_TextChangedS(object sender, TextChangedEventArgs e)
        {
            if (!World.startAnimation)
            {
                // ... Get control that raised this event.
                var textBox = sender as TextBox;
                // ... Change Window Title.
                if (Regex.IsMatch(textBox.Text, @"^[0-9]+$"))
                {
                    this.Skaliranjepodloge = Int32.Parse(textBox.Text);
                }
            }
        }

        private void podesiBojuSvetla(object sender, SelectionChangedEventArgs e)
        {
            if (!World.startAnimation)
            {
                // ... Get control that raised this event.
                //var textBox = sender as ComboBox;
                // ... Change Window Title.
                String s = ColorKutija.Text;
                //ComboBoxItem selectedColor = (ComboBoxItem)textBox.SelectedItem;

                this.BojaSvetla = s;

            }
        }

        public int RotacijaSvece
        {
            get { return rotacijaSvece; }
            set
            {
                rotacijaSvece = value;
                m_world.RotacijaSvece = value;
            }
        }


        public int Skaliranjepodloge
        {
            get { return skaliranjepodloge; }
            set
            {
                skaliranjepodloge = value;
                m_world.Skaliranjepodloge = value;
            }
        }

        public string BojaSvetla
        {
            get { return bojaSvetla; }
            set
            {
                bojaSvetla = value;
                if (bojaSvetla.Equals("Narandzasta"))
                {
                    m_world.Crvena = 1;
                    m_world.Zelena = 0.5f;
                    m_world.Plava = 0f;
                }
                if (bojaSvetla.Equals("Crvena"))
                {
                    m_world.Crvena = 1f;
                    m_world.Zelena = 0f;
                    m_world.Plava = 0f;
                }
                else if (bojaSvetla.Equals("Zelena"))
                {
                    m_world.Crvena = 0f;
                    m_world.Zelena = 1f;
                    m_world.Plava = 0f;
                }
                else if (bojaSvetla.Equals("Plava"))
                {
                    m_world.Crvena = 0f;
                    m_world.Zelena = 0f;
                    m_world.Plava = 1f;
                }
            }
        }



        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;

        #endregion Atributi

        #region Konstruktori

        public MainWindow()
        {
            // Inicijalizacija komponenti
            InitializeComponent();

            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\CandleWithPlate"), "CandleWithPlate.3DS", (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
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
            if (!World.startAnimation)
            {
                switch (e.Key)
                {
                    case Key.F5:
                        this.Close();
                        break;
                    case Key.T:
                        Console.WriteLine(m_world.RotationX);
                        if (m_world.RotationX - 5.0f >= 0f && m_world.RotationX - 5.0f <= 90f)
                        {
                            m_world.RotationX -= 5.0f;
                        }
                        else
                        {
                            m_world.RotationX = m_world.RotationX;
                        }
                        break;
                    case Key.G:
                        Console.WriteLine(m_world.RotationX);
                        if (m_world.RotationX + 5.0f >= 0f && m_world.RotationX + 5.0f <= 90f)
                        {
                            m_world.RotationX += 5.0f;
                        }
                        else
                        {
                            m_world.RotationX = m_world.RotationX;
                        }
                        break;
                    case Key.F:
                        m_world.RotationY -= 5.0f;
                        break;
                    case Key.H:
                        m_world.RotationY += 5.0f;
                        break;
                    case Key.Add:
                        m_world.SceneDistance -= 700.0f;
                        break;
                    case Key.Subtract:
                        m_world.SceneDistance += 700.0f;
                        break;
                    case Key.F2:
                        OpenFileDialog opfModel = new OpenFileDialog();
                        bool result = (bool)opfModel.ShowDialog();
                        if (result)
                        {

                            try
                            {
                                World newWorld = new World(Directory.GetParent(opfModel.FileName).ToString(), Path.GetFileName(opfModel.FileName), (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
                                m_world.Dispose();
                                m_world = newWorld;
                                m_world.Initialize(openGLControl.OpenGL);
                            }
                            catch (Exception exp)
                            {
                                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta:\n" + exp.Message, "GRESKA", MessageBoxButton.OK);
                            }
                        }
                        break;

                    case Key.C:
                        if (World.startAnimation)
                        {
                            World.timer.Stop();
                            World.timer = null;
                            World.startAnimation = false;
                        }
                        else
                        {
                            World.startAnimation = true;
                            World.timer = new System.Windows.Threading.DispatcherTimer();
                            World.timer.Tick += new EventHandler(World.Animate);
                            World.timer.Interval = TimeSpan.FromMilliseconds(1);
                            World.timer.Start();
                        }
                        break;
                }
            }
        }
    }
}
