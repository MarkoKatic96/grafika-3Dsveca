// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Threading;

namespace AssimpSample
{
    /// <summary>
    ///  Nabrojani tip OpenGL rezima filtriranja tekstura
    /// </summary>
    public enum TextureFilterMode
    {
        Nearest,
        Linear,
        NearestMipmapNearest,
        NearestMipmapLinear,
        LinearMipmapNearest,
        LinearMipmapLinear
    };

    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        private enum TextureObjects { Brick = 0, Floor, Candle};
        private readonly int m_textureCount = Enum.GetNames(typeof(TextureObjects)).Length;

        /// <summary>
        ///	 Identifikatori OpenGL tekstura
        /// </summary>
        private uint[] m_textures = new uint[3];

        public static int rotacijaSvece = 0;
        public static int skaliranjepodloge = 1;
        public static float crvena = 1f;
        public static float zelena = 0.5f;
        public static float plava = 0f;


        /// <summary>
        ///	 Putanje do slika koje se koriste za teksture
        /// </summary>
        private string[] m_textureFiles = {"..//..//images//brick.jpg", "..//..//images//wood.jpg", "..//..//images//wax.jpg" };

        /// <summary>
        ///  Izabrana OpenGL mehanizam za iscrtavanje.
        /// </summary>
        private TextureFilterMode m_selectedMode = TextureFilterMode.Linear;


        /// <summary>
        ///	 Ugao rotacije Meseca
        /// </summary>
        private float m_moonRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije Zemlje
        /// </summary>
        private float m_earthRotation = 0.0f;

        /// <summary>
        /// Pokreni animaciju
        /// </summary>
        /// 
        //animacija
        public static bool startAnimation = false;

        /// <summary>
        /// Tajmer
        /// </summary>
        public static DispatcherTimer timer;
        public static float xSveca;
        public static float zSveca;
        public static float xPodloga;
        public static float zPodloga;
        public static float xKamera;
        public static float zKamera;
        public static bool napred;
        public static bool levo;
        public static bool desno;
        public static bool nazad;
        public static bool kraj;
        public static float rSvece;
        public static float rPodloge;
        public static float rKamereZ;
        public static float rKamereX;
        public static int rSveceAnimacija = 0;


        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene;//sveca

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 150.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Scene
        {
            get { return m_scene; }
            set { m_scene = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public int RotacijaSvece
        {
            get { return rotacijaSvece; }
            set
            {
                rotacijaSvece = value;

            }
        }

        public int Skaliranjepodloge
        {
            get { return skaliranjepodloge; }
            set { skaliranjepodloge = value; }
        }

        public float Crvena
        {
            get { return crvena; }
            set { crvena = value; }
        }

        public float Zelena
        {
            get { return zelena; }
            set { zelena = value; }
        }

        public float Plava
        {
            get { return plava; }
            set { plava = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePath, String sceneFileName, int width, int height, OpenGL gl)
        {
            this.m_scene = new AssimpScene(scenePath, sceneFileName, gl);
            this.m_width = width;
            this.m_height = height;
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {   //animacija
            nazad = false;
            napred = true;
            levo = false;
            desno = false;
            kraj = false;
            xSveca = 0.0f;
            zSveca = -m_sceneDistance;
            zPodloga = 0f;
            xPodloga = 0f;
            xKamera = 1.0f;
            zKamera = 4.0f;
            rKamereZ = 0f;
            rKamereX = 0f;
            rPodloge = 0f;
            rSvece = 0f;
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 0f, 0f);
            // Model sencenja na flat (konstantno)
            gl.ShadeModel(OpenGL.GL_FLAT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            SetupLighting(gl);
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);//ukljucen color tracking
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            //stapanje sa materijalom
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);

            // Ucitaj slike i kreiraj teksture
            gl.GenTextures(m_textureCount, m_textures);
            for (int i = 0; i < m_textureCount; ++i)
            {
                // Pridruzi teksturu odgovarajucem identifikatoru
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[i]);

                // Ucitaj sliku i podesi parametre teksture
                Bitmap image = new Bitmap(m_textureFiles[i]);
                // rotiramo sliku zbog koordinantog sistema opengl-a
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                // RGBA format (dozvoljena providnost slike tj. alfa kanal)
                BitmapData imageData = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                      System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA8, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, imageData.Scan0);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);		// Linear Filtering
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);      // Linear Filtering
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);

                image.UnlockBits(imageData);
                image.Dispose();
            }

            m_scene.LoadScene();
            m_scene.Initialize();
        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>

        private void SetupLighting(OpenGL gl)
        {
            float[] global_ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, global_ambient);

            float[] light0pos = new float[] { 0.0f, 100.0f, 0.0f, 1.0f };
            float[] light0ambient = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
            float[] light0diffuse = new float[] { 1.0f, 1.0f, 0.0f, 1.0f };
            float[] light0specular = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };

            float[] light1pos = new float[] { 0.0f, 100.0f, 0.0f, 1.0f };
            float[] light1ambient = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
            float[] light1diffuse = new float[] { 1f, 0.5f, 0f, 1.0f };
            float[] light1specular = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };


            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, light0specular);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f);

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, light1pos);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, light1ambient);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, light1diffuse);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, light1specular);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 180.0f);


            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);
            gl.Enable(OpenGL.GL_LIGHT1);

            // Definisemo belu spekularnu komponentu materijala sa jakim odsjajem
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SPECULAR, light0specular);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SHININESS, 128.0f);

            //Uikljuci color tracking mehanizam
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);

            // Podesi na koje parametre materijala se odnose pozivi glColor funkcije
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);

            // Ukljuci automatsku normalizaciju nad normalama
            gl.Enable(OpenGL.GL_NORMALIZE);

            //m_normals = LightingUtilities.ComputeVertexNormals(m_vertices);

            //gl.ShadeModel(OpenGL.GL_SMOOTH);
        }

        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);//ukljuci depth
            gl.Enable(OpenGL.GL_CULL_FACE);//ukljuci cull
            gl.LoadIdentity();

            gl.PushMatrix();
                gl.LookAt(xKamera, 15.0f, zKamera + 50f, xSveca, 0f, zSveca, 0.0f, 1.0f, 0.0f);
                gl.Translate(0f, -1.0f, 0f);
                gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
                gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);     
            gl.PushMatrix();//sveca 
                gl.Translate(xSveca, 0.0f, zSveca + 150f);
                gl.Rotate(0f, -rSvece + rotacijaSvece, 0f);
                float[] light1pos = new float[] { xSveca, 0.0f, zSveca, 1.0f };
                //gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, new float[] { 0f, 0f, 0f, 1f });//svetlost na vrhu svece
                float[] light1diffuse = new float[] { Crvena, Zelena, Plava, 1.0f };
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, light1diffuse);
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, light1pos);
                gl.Scale(Skaliranjepodloge, Skaliranjepodloge, Skaliranjepodloge);
                gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
                m_scene.Draw();
            gl.PopMatrix();
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);
            //CRTANJE ZIDOVA
            gl.PushMatrix(); //podloga

         
                gl.Begin(OpenGL.GL_QUADS);
                    gl.Rotate(0f, -rPodloge, 0f);
                    gl.Normal(Lighting.LightingUtilities.FindFaceNormal(50f, 0f, 50f, 50f, 0f, -50f, -50f, 0f, -50f));
                    gl.Vertex(50f + xPodloga, 0f, 50f + zPodloga);
                    gl.Vertex(50f + xPodloga, 0f, -50f + zPodloga);
                    gl.Vertex(-50f + xPodloga, 0f, -50f + zPodloga);
                    gl.Vertex(-50f + xPodloga, 0f, 50f + zPodloga);
                gl.End();

            gl.FrontFace(OpenGL.GL_CW);//kontrolise koja je strana prednja

                gl.Begin(OpenGL.GL_QUADS);//roof           
                    gl.Color(1.0f, 1.0f, 1.0f);
                    gl.Normal(Lighting.LightingUtilities.FindFaceNormal(-500f, 500f, 500f, 500f, 500f, 500f, 500f, 500f, -500f));
                    gl.Vertex(-500f,500f, 500f);
                    gl.Vertex(500f, 500f, 500f);
                    gl.Vertex(500f, 500f, -500f);
                    gl.Vertex(-500f, 500f, -500f);
                gl.End();

                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Floor]);
                gl.Begin(OpenGL.GL_QUADS);//floor
                    gl.Color(1.0f, 0.0f, 1.0f);
                    gl.Normal(Lighting.LightingUtilities.FindFaceNormal(500f, -500f, 500f, -500f, -500f, 500f, -500f, -500f, -500f));
                    gl.TexCoord(0, 0);
                    gl.Vertex(500f, -500f, 500f);
                    gl.TexCoord(0, 5);
                    gl.Vertex(-500f, -500f, 500f);
                    gl.TexCoord(5, 5);
                    gl.Vertex(-500f, -500f, -500f);
                    gl.TexCoord(5, 0);
                    gl.Vertex(500f, -500f, -500f);
                gl.End();

            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Brick]);
            gl.Begin(OpenGL.GL_QUADS);//front
                    gl.Color(1.0f, 1.0f, 1.0f);
                    gl.Normal(Lighting.LightingUtilities.FindFaceNormal(500f, -500f, 500f, 500f, 500f, 500f, -500f, 500f, 500f));
                    gl.TexCoord(0, 0);
                    gl.Vertex(500f, -500f, 500f);
                    gl.TexCoord(0, 5);
                    gl.Vertex(500f, 500f, 500f);
                    gl.TexCoord(5, 5);
                    gl.Vertex(-500f, 500f, 500f);
                    gl.TexCoord(5, 0);
                    gl.Vertex(-500f, -500f, 500f);
                gl.End();

               
                gl.Begin(OpenGL.GL_QUADS);//right
                    gl.Color(0.0f, 0.0f, 1.0f);
                    gl.Normal(Lighting.LightingUtilities.FindFaceNormal(500f, 500f, -500f, 500f, 500f, 500f, 500f, -500f, 500f));
                    gl.TexCoord(0, 0);
                    gl.Vertex(500f, 500f, -500f);
                    gl.TexCoord(0, 5);
                    gl.Vertex(500f, 500f, 500f);
                    gl.TexCoord(5, 5);
                    gl.Vertex(500f, -500f, 500f);
                    gl.TexCoord(5, 0);
                    gl.Vertex(500f, -500f, -500f);
                gl.End();

                
                gl.Begin(OpenGL.GL_QUADS);//left
                    gl.Color(1.0f, 1.0f, 0.0f);
                    gl.Normal(Lighting.LightingUtilities.FindFaceNormal(-500f, -500f, 500f, -500f, 500f, 500f, -500f, 500f, -500f));
                    gl.TexCoord(0, 0);
                    gl.Vertex(-500f, -500f, 500f);
                    gl.TexCoord(0, 5);
                    gl.Vertex(-500f, 500f, 500f);
                    gl.TexCoord(5, 5);
                    gl.Vertex(-500f, 500f, -500f);
                    gl.TexCoord(5, 0);
                    gl.Vertex(-500f, -500f, -500f);
                gl.End();

                gl.Begin(OpenGL.GL_QUADS);//back
                    gl.Color(0.0f, 1.0f, 1.0f);
                    gl.Normal(Lighting.LightingUtilities.FindFaceNormal(500f, 500f, -500f, 500f, -500f, -500f, -500f, -500f, -500f));
                    gl.TexCoord(0, 0);
                    gl.Vertex(500f, 500f, -500f);
                    gl.TexCoord(0, 5);
                    gl.Vertex(500f, -500f, -500f);
                    gl.TexCoord(5, 5);
                    gl.Vertex(-500f, -500f, -500f);
                    gl.TexCoord(5, 0);
                    gl.Vertex(-500f, 500f, -500f);
                gl.End();

                gl.FrontFace(OpenGL.GL_CCW);//kontrolise koja je strana prednja

            gl.PopMatrix();

            gl.PushMatrix();
                gl.Viewport(m_width/2, 0, m_width/2, m_height/2);
                gl.DrawText(m_width - 450, 130, 1f, 0f, 0f, "Arial bold", 14, "Predmet: Racunarska grafika");
                gl.DrawText(m_width - 450, 100, 1f, 0f, 0f, "Arial bold", 14, "Sk.god: 2017/18");
                gl.DrawText(m_width - 450, 70, 1f, 0f, 0f, "Arial bold", 14, "Ime: Marko");
                gl.DrawText(m_width - 450, 40, 1f, 0f, 0f, "Arial bold", 14, "Prezme: Katic");
                gl.DrawText(m_width - 450, 10, 1f, 0f, 0f, "Arial bold", 14, "Sifra zad: 14.1");
            gl.PopMatrix();

            gl.Viewport(0, 0, m_width, m_height);

            gl.PopMatrix();

            gl.Flush();
        }

        public static void Animate(Object sender, EventArgs e)
        {
            Console.WriteLine("napred: " +napred);
            Console.WriteLine("nazad: " + nazad);
            Console.WriteLine("levo: " + levo);
            Console.WriteLine("desno: " + desno);

            if (napred == true)
            {
                if (zPodloga > -200)
                {
                    zPodloga -= 1f;
                    zKamera -= 1f;
                    zSveca -= 1f;
                }
                else
                {
                    if (rSveceAnimacija < 90f)
                    {
                        rSvece += 1f;
                        rPodloge += 1f;
                        rSveceAnimacija++;
                    }
                    else
                    {
                        rSveceAnimacija = 0;
                        napred = false;
                        desno = true;
                    }
                }
            }
            else if (desno == true)
            {
                if (xPodloga < 200f)
                {

                    xSveca += 1f;
                    xPodloga += 1f;
                    xKamera += 1f;
                }
                else
                {
                    if (rSveceAnimacija < 90f)
                    {
                        rSvece += 1f;
                        rPodloge += 1f;
                        rSveceAnimacija++;


                    }
                    else
                    {

                        rSveceAnimacija = 0;
                        desno = false;
                        nazad = true;

                    }
                }

            }
            else if (nazad == true)
            {
                if (zPodloga < 0)
                {
                    zPodloga += 1f;
                    zKamera += 1f;
                    zSveca += 1f;
                }
                else
                {
                    if (rSveceAnimacija < 90)
                    {
                        rSvece += 1f;
                        rPodloge += 1f;
                        rSveceAnimacija++;
                    }
                    else
                    {
                        rSveceAnimacija = 0;
                        nazad = false;
                        levo = true;

                    }
                }
                
            }
            else if (levo == true)
            {
                if (xPodloga > 0f)
                {
                    xSveca -= 1f;
                    xPodloga -= 1f;
                    xKamera -= 1f;
                }
                else
                {
                    if (rSveceAnimacija < 90)
                    {
                        rSvece += 1f;
                        rPodloge += 1f;
                        rSveceAnimacija++;

                    }
                    else
                    {
                        rSveceAnimacija = 0;
                        levo = false;
                        kraj = true;
                    }
                }
            }
            else if (kraj == true)
            {
                timer.Stop();
                timer = null;
                startAnimation = false;
                napred = true;
            }
        }


        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.Viewport(0, 0, width, height);
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(50f, (double)width / height, 1f, 20000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();                // resetuj ModelView Matrix
        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene.Dispose();
            }
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}
