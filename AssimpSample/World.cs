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
using Tao.OpenGl;
using System.Drawing;
using System.Drawing.Imaging;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Cameras;
using SharpGL;
using System.Windows.Threading;

namespace AssimpSample
{


    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        #region Scena    
                    /// </summary>
                    private AssimpScene m_scene;

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
        private float sceneDistance1 = 600.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

                    /// <summary>
                    ///	 Visina OpenGL kontrole u pikselima.
                    /// </summary>
                    private int m_height;
            #endregion
        #region Teksture
                    private enum TextureObjects { Grass = 0, Plastic, Ball };
                    private readonly int m_textureCount = Enum.GetNames(typeof(TextureObjects)).Length;

                    /// <summary>
                    ///	 Identifikatori OpenGL tekstura
                    /// </summary>
                    private uint[] m_textures = null;

                    /// <summary>
                    ///	 Putanje do slika koje se koriste za teksture
                    /// </summary>
                    private string[] m_textureFiles = { "..//..//images//grass.jpg", "..//..//images//plastika.jpg" , "..//..//images//lopta.jpg" };
        #endregion
        #region KretanjeLopte
        private float ballHeight;
        private bool ballGoingUp;
        private DispatcherTimer timer1;
        private DispatcherTimer timer2;
        private bool jumpStop;
        private float[] pos;
        private double yRotateBall;
        #endregion
        #region SvetloPozicija
        private float xSvetlo;
        private float ySvetlo;
        private float zSvetlo;
        private bool ukljucenoTackasto;
        private bool ukljucenoReflektor;
        private bool dnevnoSvetlo;
        #endregion
        #region SkaliranjeLopte
        public double velicinaLopte;
        #endregion
        #region AmbijentalnaKomponenta
        private float[] ambijentalnaKomponenta2;
        #endregion
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
            get { return SceneDistance1; }
            set { SceneDistance1 = value; }
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

        public DispatcherTimer Timer1 { get => timer1; set => timer1 = value; }
        public DispatcherTimer Timer2 { get => timer2; set => timer2 = value; }
        public float XSvetlo { get => xSvetlo; set => xSvetlo = value; }
        public float YSvetlo { get => ySvetlo; set => ySvetlo = value; }
        public float ZSvetlo { get => zSvetlo; set => zSvetlo = value; }
        public bool UkljucenoTackasto { get => ukljucenoTackasto; set => ukljucenoTackasto = value; }
        public bool UkljucenoReflektor { get => ukljucenoReflektor; set => ukljucenoReflektor = value; }
        public bool DnevnoSvetlo { get => dnevnoSvetlo; set => dnevnoSvetlo = value; }
        public float[] AmbijentalnaKomponenta2 { get => ambijentalnaKomponenta2; set => ambijentalnaKomponenta2 = value; }
        public bool JumpStop { get => jumpStop; set => jumpStop = value; }
        public float[] Pos { get => pos; set => pos = value; }
        public double YRotateBall { get => yRotateBall; set => yRotateBall = value; }
        public float BallHeight { get => ballHeight; set => ballHeight = value; }
        public bool BallGoingUp { get => ballGoingUp; set => ballGoingUp = value; }
        public float SceneDistance1 { get => sceneDistance1; set => sceneDistance1 = value; }

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

        public void TackastiIzvorOnOff()
        {
            UkljucenoTackasto = !UkljucenoTackasto;
        }

        public void ReflektorIzvorOnOff()
        {
            UkljucenoReflektor = !UkljucenoReflektor;
        }

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.8f, 0.8f, 0.8f, 1.0f);
            gl.Color(1f, 0f, 0f);
           
            //PRVA TACKA    
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            //DRUGA TACKA
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            gl.Enable(OpenGL.GL_NORMALIZE);///NE ZNAM KAKO AUTOMATSKO GENERISANJE NORMALA
            #region Teksture
                m_textures = new uint[3];
                gl.Enable(OpenGL.GL_TEXTURE_2D);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);
                gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);

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
                    gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);		// Linear Filtering

                    image.UnlockBits(imageData);
                    image.Dispose();
                }
    #endregion          
            #region PomeranjeLopte
            Timer1 = new DispatcherTimer();
                        Timer1.Interval = TimeSpan.FromMilliseconds(10);
                        Timer1.Tick += new EventHandler(UpdateAnimation1);
                        Timer1.Start();
                        Timer2 = new DispatcherTimer();
                        Pos = new float[3];
                        BallHeight = -100f;
                        Pos[0] = 0f;
                        Pos[1] = BallHeight;
                        Pos[2] = -SceneDistance1 + 200;
                        
                        BallGoingUp = true;
                        JumpStop = false;
                        YRotateBall = 0;
                       
            #endregion
            #region SvetloPozicija
                XSvetlo = 0;
                YSvetlo = 100;
                ZSvetlo = -SceneDistance1 + 250;
                UkljucenoTackasto = true;
                UkljucenoReflektor = true;
            DnevnoSvetlo = false;
            #endregion
            #region SkaliranjeLopte
            velicinaLopte = MainWindow.skaliranje;
            #endregion
            #region AmbijentalnaKomponenta
                AmbijentalnaKomponenta2 = new float[4];
                AmbijentalnaKomponenta2[0] = 0.3f;
                AmbijentalnaKomponenta2[1] = 0.3f;
                AmbijentalnaKomponenta2[2] = 0.9f;
                AmbijentalnaKomponenta2[3] = 1f;
            #endregion
            m_scene.LoadScene();
            m_scene.Initialize();
        }

        #region PomeranjeLopte

        public void sutniLoptu()
        {
            //RADI NA DUGME V
            BallHeight = -100f;
            Pos[0] = 0f;
            Pos[1] = BallHeight;
            Pos[2] = -SceneDistance1 + 200;
            JumpStop = true;

            Timer1.Stop();
            Timer2 = new DispatcherTimer();
            Timer2.Interval = TimeSpan.FromMilliseconds(1);
            Timer2.Tick += new EventHandler(UpdateAnimation2);
            Timer2.Start();

        }

        private void UpdateAnimation1(object sender, EventArgs e)
                {

                   

           
               
                }

                private void UpdateAnimation2(object sender, EventArgs e)
                {
                    YRotateBall = 0;
                    if (Pos[2] < 100)
                    {
                        Pos[0] += 4;
                        Pos[1] += 5;
                        Pos[2] += 10;
                    }
                    else
                    {
                        BallHeight = -100f;
                        Pos[0] = 0f;
                        Pos[1] = BallHeight;
                        Pos[2] = -SceneDistance1 + 200;


                    }


                }
                #endregion

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
    #region Generalno
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            Glu.gluPerspective(50.0, (double)m_width / (double)m_height, 0.5, 1000.0);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PushMatrix();
            gl.Rotate(m_xRotation, 1, 0, 0);
            gl.Rotate(m_yRotation, 0, 1, 0);
            gl.PushMatrix();
           
            //KAMERA
            Glu.gluLookAt(0f, 0f, -SceneDistance1 - 400, 0f, 0f, -SceneDistance1, 0f, 1f, 0f);
                gl.PushMatrix();
                    gl.Viewport(0, 0, m_width, m_height);
                    Glu.gluLookAt(0f, 0f, -SceneDistance1 - 400, 0f, 0f, -SceneDistance1, 0f, 1f, 0f);
            #endregion

            //DRUGA TACKA

            #region Osvetljenje
            gl.Enable(OpenGL.GL_LIGHTING);

            gl.PushMatrix();
            #region TackastiIzvorSvetlosti
                    float[] ambijentalnaKomponenta = { 0.7f, 0.2f, 0.3f, 1.0f };
                    float[] spekularnaKomponenta = { 0.5f, 0.5f, 0.5f, 1 };
                    float[] difuznaKomponenta = { 0.7f, 0.2f, 0.3f, 1.0f };
 
                     gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, ambijentalnaKomponenta);
                     gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, difuznaKomponenta);
                     gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, spekularnaKomponenta);

                    float[] smer1 = { -1f, 0.0f, -1.0f };
                    gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_DIRECTION, smer1);

                    gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f);
                    // Pozicioniraj svetlosni izvor
           
                    float[] pozicija = { 600f, 600.0f, -SceneDistance1 + 200, 1.0f };
                            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, pozicija);

            
                    gl.PopMatrix();
                        #endregion

            #region Reflektor
                             gl.PushMatrix();
              
                                float[] difuznaKomponenta2 = { 0.7f, 0.8f, 0.9f, 1.0f };
                                float[] smer = { 0.0f, 0.0f, -1.0f };
                                float[] light0specular = new float[] { 0.5f, 0.6f, 0.8f, 1.0f };

            
                                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, light0specular);
                                // Pridruži komponente svetlosnom izvoru 1
                                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT,
                                 AmbijentalnaKomponenta2);
                                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE,
                                difuznaKomponenta2);
                                // Podesi parametre reflektorkskog izvora
                                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, smer);
                                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 35.0f);
                                // Ukljuci svetlosni izvor
           
                                gl.Rotate(-45, 1, 0, 0);
         
                                // Pozicioniraj svetlosni izvor
                                float[] pozicija2 = { XSvetlo, YSvetlo, ZSvetlo, 1.0f };
                                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, pozicija2);

                     gl.PopMatrix();

                    #region DnevnoSvetlo
                    gl.PushMatrix();

                        float[] difuznaKomponenta3 = { 1f, 0.3f, 0.4f, 1.0f };
                        float[] smer3 = { 0.0f, 0.0f, -1.0f };
                        float[] light0specular3 = new float[] { 0.5f,0.5f,0.5f,1};
                        float[] ambijentalnaKomponenta3 = { 1f, 0.3f, 0.4f, 1.0f };

                    gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPECULAR, light0specular3);
                        // Pridruži komponente svetlosnom izvoru 1
                        gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_AMBIENT,
                         ambijentalnaKomponenta3);
                        gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_DIFFUSE,
                        difuznaKomponenta3);
                        // Podesi parametre reflektorkskog izvora
                        gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPOT_DIRECTION, smer3);
                        gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPOT_CUTOFF, 180);
                        // Ukljuci svetlosni izvor

                        gl.Rotate(45, 1, 0, 0);
           
                        float[] pozicija3 = { 0, 600, -SceneDistance1-300, 1.0f };
                        gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_POSITION, pozicija3);

                    gl.PopMatrix();

                    #endregion


                    if (UkljucenoTackasto)
                        gl.Enable(OpenGL.GL_LIGHT0);
                    else
                        gl.Disable(OpenGL.GL_LIGHT0);

                    if (UkljucenoReflektor)
                        gl.Enable(OpenGL.GL_LIGHT1);
                    else
                        gl.Disable(OpenGL.GL_LIGHT1);

                    if (DnevnoSvetlo)
                        gl.Enable(OpenGL.GL_LIGHT2);
                    else
                        gl.Disable(OpenGL.GL_LIGHT2);
                    #endregion



            #endregion

            #region Podloga
            //PODLOGA
            gl.PushMatrix();
                        gl.Translate(0.0f, -0f, -0);
                        gl.Color(0.22f, 0.61f, 0.47f);
            //BINDOVANJE TEKSTURE TRAVE
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Grass]);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.MatrixMode(OpenGL.GL_TEXTURE);      // selektuj Projection Matrix
                gl.LoadIdentity();
                gl.Scale(4,4,4);
                gl.MatrixMode(OpenGL.GL_MODELVIEW);

                gl.Begin(OpenGL.GL_QUADS);
                            gl.TexCoord(0.0f, 0.0f);
                            gl.Vertex4f(400f, -100f, SceneDistance1, 1);
                            gl.TexCoord(1.0f, 0.0f);
                            gl.Vertex4f(400f, -100f, -SceneDistance1-400, 1);
                            gl.TexCoord(1.0f, 1.0f);
                            gl.Vertex4f(-400f, -100f, -SceneDistance1-400, 1);
                            gl.TexCoord(0.0f, 1.0f);
                            gl.Vertex4f(-400f, -100f, SceneDistance1, 1);
                        gl.End();
            
                    gl.PopMatrix();
                #endregion
            
    #region Gol

      

                //GORE
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Plastic]);
                gl.PushMatrix();
            //NISAM SIGURNA
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_ADD);


                        gl.Translate(-150, 105.0f, -SceneDistance1 - 100);
                        gl.Rotate(90f, 90f, 0f);


            Cylinder cylinder = new Cylinder();
            cylinder.BaseRadius = 7;
                        cylinder.TopRadius = 7;
                        cylinder.Height = 300;
            //NE RADI OVO
            //gl.QuadricNormals(cylinder, OpenGL.GLU_SMOOTH);
            cylinder.TextureCoords = true;
                cylinder.CreateInContext(gl);

                        cylinder.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

                gl.PopMatrix();


                //LEVA STATIVA
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Plastic]);
                gl.PushMatrix();
                        gl.Translate(-150f, -100.0f, -SceneDistance1-100);
                        gl.Rotate(-90f, 0f, 0f);
     
                        SharpGL.SceneGraph.Quadrics.Cylinder cylinder2 = new SharpGL.SceneGraph.Quadrics.Cylinder() { Name = "Cylinder2" };
                        cylinder2.BaseRadius = 7;
                        cylinder2.TopRadius = 7;
                        cylinder2.Height = 210;
                cylinder2.TextureCoords = true;
                cylinder2.CreateInContext(gl);
                        cylinder2.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
             
                    gl.PopMatrix();

                //DESNA STATIVA
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Plastic]);
                gl.PushMatrix();
                         gl.Translate(150f, -100.0f, -SceneDistance1 - 100);
                         gl.Rotate(-90f, 0f, 0f);
           
                         SharpGL.SceneGraph.Quadrics.Cylinder cylinder3 = new SharpGL.SceneGraph.Quadrics.Cylinder() { Name = "Cylinder3" };
                         cylinder3.BaseRadius = 7;
                         cylinder3.TopRadius = 7;
                         cylinder3.Height = 210;
                cylinder3.TextureCoords = true;
                cylinder3.CreateInContext(gl);
                         cylinder3.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
                  
                    gl.PopMatrix();

                //DESNA STATIVA IZA
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Plastic]);
                gl.PushMatrix();
                        gl.Translate(150f, -100.0f, -SceneDistance1 - 300);
                        gl.Rotate(-45f, 0f, 0f);
     
                        SharpGL.SceneGraph.Quadrics.Cylinder cylinder4 = new SharpGL.SceneGraph.Quadrics.Cylinder() { Name = "Cylinder4" };
                        cylinder4.BaseRadius = 7;
                        cylinder4.TopRadius = 7;
                        cylinder4.Height = 285;
                cylinder4.TextureCoords = true;
                cylinder4.CreateInContext(gl);
                        cylinder4.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render); 

                    gl.PopMatrix();

                //LEVA STATIVA IZA
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Plastic]);
                gl.PushMatrix();
                        gl.Translate(-150f, -100.0f, -SceneDistance1 - 300);
                        gl.Rotate(-45f, 0f, 0f);
          
                        SharpGL.SceneGraph.Quadrics.Cylinder cylinder5 = new SharpGL.SceneGraph.Quadrics.Cylinder() { Name = "Cylinder5" };
                        cylinder5.BaseRadius = 7;
                        cylinder5.TopRadius = 7;
                        cylinder5.Height = 285;
                cylinder5.TextureCoords = true;
                cylinder5.CreateInContext(gl);
                        cylinder5.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
                   
                    gl.PopMatrix();

                //DOLE
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Plastic]);
                gl.PushMatrix();
                        gl.Translate(-150, -95f, -SceneDistance1 - 290);
                        gl.Rotate(90f, 90f, 0f);

                        SharpGL.SceneGraph.Quadrics.Cylinder cylinder6 = new SharpGL.SceneGraph.Quadrics.Cylinder() { Name = "Cylinder6" };
                        cylinder6.BaseRadius = 7;
                        cylinder6.TopRadius = 7;
                        cylinder6.Height = 300;
                cylinder6.TextureCoords = true;
                cylinder6.CreateInContext(gl);
                        cylinder6.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
  
                    gl.PopMatrix();

                //DONJA DESNA STRANA
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Plastic]);
                gl.PushMatrix();
                        gl.Translate(150f, -95.0f, -SceneDistance1 - 290);
                        gl.Rotate(180f, 180f, 0f);

                        SharpGL.SceneGraph.Quadrics.Cylinder cylinder7 = new SharpGL.SceneGraph.Quadrics.Cylinder() { Name = "Cylinder7" };
                        cylinder7.BaseRadius = 7;
                        cylinder7.TopRadius = 7;
                        cylinder7.Height = 190;
                cylinder7.TextureCoords = true;
                cylinder7.CreateInContext(gl);
                        cylinder7.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
                   
                    gl.PopMatrix();

                //DONJA LEVA STRANA
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Plastic]);
                gl.PushMatrix();
                        gl.Translate(-150f, -95.0f, -SceneDistance1 - 290);
                        gl.Rotate(180f, 180f, 0f);
           
            SharpGL.SceneGraph.Quadrics.Cylinder cylinder8 = new SharpGL.SceneGraph.Quadrics.Cylinder() { Name = "Cylinder8" };
                        cylinder8.BaseRadius = 7;
                        cylinder8.TopRadius = 7;
                        cylinder8.Height = 190;
                cylinder8.TextureCoords = true;
                cylinder8.CreateInContext(gl);
                        cylinder8.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

                    gl.PopMatrix();

            #endregion

    #region Lopta
            
            gl.PushMatrix();


                Glu.gluLookAt(0f, 0f, -SceneDistance1 - 400, 0f, 0f, -SceneDistance1, 0f, 1f, 0f);
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Ball]);
                gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);

            if (!JumpStop)
            {
                if (BallGoingUp)
                {
                    BallHeight += 40f;
                    Pos[0] = 0;
                    Pos[1] = BallHeight;
                    if (BallHeight >= -30f)
                        BallGoingUp = false;
                }

                else
                {
                    BallHeight -= 40f;
                    Pos[0] = 0;
                    Pos[1] = BallHeight;
                    if (BallHeight == -100f)
                        BallGoingUp = true;
                }

                 gl.Translate(Pos[0], Pos[1], Pos[2]);


                if (YRotateBall > 45)
                {
                    YRotateBall = 0.0f;
                }
                else
                {
                    YRotateBall += MainWindow.brzinaRotacije;
                }

            gl.Rotate(YRotateBall, 0, 1, 0);
            }
            else
            {
                gl.Translate(Pos[0], Pos[1], Pos[2]);
            }

            


           

                gl.Scale(velicinaLopte, velicinaLopte, velicinaLopte);
                m_scene.Draw();
                

               gl.PopMatrix();
    #endregion
                gl.PopMatrix();
            gl.PopMatrix();
            gl.PopMatrix(); 
#region Tekst
            gl.PushMatrix();
                gl.LoadIdentity();
                Glu.gluOrtho2D(-m_width / 2.0, m_width / 2.0, -m_height / 2.0, m_height / 2.0);
               // gl.Ortho2D(-m_width / 2.0, m_width / 2.0, -m_height / 2.0, m_height / 2.0);
                gl.MatrixMode(Gl.GL_MODELVIEW);
                gl.Viewport(m_width / 2, m_height / 2, m_width, m_height);
                gl.DrawText(m_width / 2 - 100, m_height / 2 - 30, 0, 0, 0, "Arial Bold", 10, "Sk.god: 2017/18");
                gl.DrawText(m_width / 2 - 100, m_height / 2 - 50, 0, 0, 0, "Arial Bold", 10, "Ime: Tanja ");
                gl.DrawText(m_width / 2 - 100, m_height / 2 - 70, 0, 0, 0, "Arial Bold", 10, "Prezime: Indjic ");
                gl.DrawText(m_width / 2 - 100, m_height / 2 - 90, 0, 0, 0, "Arial Bold", 10, "Sifra zad: 9.2 ");
            gl.PopMatrix();

            gl.PushMatrix();
                gl.Viewport(0, 0, m_width, m_height);
                gl.LoadIdentity();
                gl.MatrixMode(Gl.GL_MODELVIEW);
            gl.PopMatrix();
#endregion
            gl.Flush();
        }


        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        /// 
        //PRVA TACKA   
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.Viewport(0, 0, m_width, m_height);
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            Glu.gluPerspective(50.0, (double)m_width / (double)m_height, 0.5, 1000.0);
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
