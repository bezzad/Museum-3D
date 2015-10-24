using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using GraphicTools;
using System.Windows.Forms;

namespace Museum
{
    public class GraphicEngine
    {
        public Device device3d;
        public Vector3 CameraPosition, CameraTarget, Upvector;
        public int trainWidth = 6, trainHeight = 6, trainDepth = 6;
        private int width, height, depth = 5000;
        private InputToolBox GItb;
        private Material material;
        public int drawStep = 10;
        private bool _fullScreen;


        Mesh3D Bridge; // پل
        Mesh3D hall_interior; // موزه
        Mesh3D STUT_C_L; // کلیسا
        Mesh3D Miz; // میز
        Mesh3D ERZHER_H; // آدم زره پوش آهنی
        Mesh3D Urn; // کوزه
        Mesh3D Pottry; // کوزه رنگی
        Mesh3D Vase; // کوزه مسی
        Mesh3D Vase2; // کوزه پایه دار
        Mesh3D Urn2; // کوزه نقره ای پایه دار
        Mesh3D CANOPES; // مجسمه های فرعون
        Mesh3D CANNON1; // توپ

        SplashScreen sps;
        SplashButton[] btnStart;

        SoundBox background_Music;
        SoundBox walk_Sound;

        public GraphicEngine(Device d, bool fullScreen)
        {
            _fullScreen = fullScreen;
            d.DeviceReset += new EventHandler(device3d_DeviceReset);
            device3d = d;

            material = new Microsoft.DirectX.Direct3D.Material();
            material.Diffuse = Color.White;
            device3d.Material = material;

            width = device3d.PresentationParameters.BackBufferWidth;
            height = device3d.PresentationParameters.BackBufferHeight;
            device3d.RenderState.CullMode = Cull.None;
            device3d.RenderState.Lighting = false;
            device3d.RenderState.FillMode = FillMode.Solid;
            //
            // Declaration Camera Information
            // 
            CameraPosition = new Vector3(-3.6F, -10F, 94F);
            CameraTarget = new Vector3(-11F, -24F, 230F);
            Upvector = new Vector3(0, 1, 0);
            //
            // Note:
            //      InputToolBox must be define after declaration of camera Inforamation!
            GItb = new InputToolBox(this);

            CreateObjects();
            background_Music.playBackMusic();
            GItb.SetWalkingSound(walk_Sound);
            initialize_text(device3d);
        }

        private void device3d_DeviceReset(object sender, EventArgs e)
        {
            device3d.RenderState.Lighting = false;
            device3d.RenderState.FillMode = FillMode.Solid;
            device3d.RenderState.CullMode = Cull.None;
            SetupCamera();
            CreateObjects();
        }

        private void SetupCamera()
        {
            device3d.Transform.View = Matrix.LookAtRH(CameraPosition, CameraTarget, Upvector);
            device3d.Transform.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 2, width / height, 1f, depth);
        }

        private void CreateObjects()
        {
            try
            {
                #region Mesh3Ds

                Bridge = new Mesh3D(device3d, "Bridge.x", new Vector3(1f, 1f, 0.5f),
                    CustomVertex.PositionColoredTextured.Format);

                hall_interior = new Mesh3D(device3d, "hall-interior.x", new Vector3(0.07f, 0.09f, 0.07f),
                    CustomVertex.PositionColoredTextured.Format);

                STUT_C_L = new Mesh3D(device3d, "STUT_C_L.x", new Vector3(0.07f, 0.09f, 0.07f),
                    CustomVertex.PositionColoredTextured.Format);

                Miz = new Mesh3D(device3d, "Miz2.x", new Vector3(0.4f, 0.4f, 0.3f),
                    CustomVertex.PositionColoredTextured.Format);

                ERZHER_H = new Mesh3D(device3d, "ERZHER_H.x", new Vector3(0.2f, 0.2f, 0.2f),
                    CustomVertex.PositionColoredTextured.Format);

                Urn = new Mesh3D(device3d, "urn.x", new Vector3(1f, 1f, 1f),
                    CustomVertex.PositionColoredTextured.Format);

                Urn2 = new Mesh3D(device3d, "urn2.x", new Vector3(1f, 1f, 1f),
                    CustomVertex.PositionColoredTextured.Format);

                Pottry = new Mesh3D(device3d, "POTTRY2.x", new Vector3(1f, 1f, 1f),
                    CustomVertex.PositionColoredTextured.Format);

                Vase = new Mesh3D(device3d, "Vase.x", new Vector3(1f, 1f, 1f),
                    CustomVertex.PositionColoredTextured.Format);

                Vase2 = new Mesh3D(device3d, "vase2.x", new Vector3(0.03f, 0.03f, 0.03f),
                    CustomVertex.PositionColoredTextured.Format);

                CANOPES = new Mesh3D(device3d, "CANOPES_.x", new Vector3(0.00035f, 0.00035f, 0.00035f),
                    CustomVertex.PositionColoredTextured.Format);

                CANNON1 = new Mesh3D(device3d, "CANNON1.x", new Vector3(0.02f, 0.02f, 0.02f),
                    CustomVertex.PositionColoredTextured.Format);

                #endregion
                #region SplashScreen objects
                //
                // SplashScreen
                //
                btnStart = new SplashButton[1];
                btnStart[0] = new SplashButton((Form)device3d.CreationParameters.FocusWindow, device3d, new Point((width / 2) - 125, height - 300), new Size(250, 100),
                         @"SplashScreen\START_Normal.png", @"SplashScreen\START_MouseOver.png",
                         @"SplashScreen\START_MouseDown.png", @"SplashScreen\START_Disable.png");
                sps = new SplashScreen(device3d, @"SplashScreen\SplashScreen.png", btnStart);
                #endregion
                #region SoundBox
                //
                // sounds
                //
                walk_Sound = new SoundBox((System.Windows.Forms.Form)device3d.CreationParameters.FocusWindow,
                                    @"mySounds\walk.wav");
                background_Music = new SoundBox((System.Windows.Forms.Form)device3d.CreationParameters.FocusWindow);
                background_Music.SetBackMusic(@"mySounds\backMusic.mp3");
                background_Music.backMusicVolume = -700;
                #endregion
                //
                //
                //
                device3d.SamplerState[0].MinFilter = TextureFilter.Anisotropic;
                device3d.SamplerState[0].MagFilter = TextureFilter.Anisotropic;
                device3d.SamplerState[0].AddressU = TextureAddress.Mirror;
                device3d.SamplerState[0].AddressV = TextureAddress.Mirror;
                //
                // 
                //
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Textures or Mesh3Ds are not OK.");
                System.Environment.Exit(0);
            }
        }

        public void DrawWorld()
        {
            SetupCamera();
            device3d.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Blue, 1.0f, 0);
            if (!btnStart[0].Enable)
            {
                SplashScreen.Enable = false;

                device3d.BeginScene();
                if (_fullScreen)
                {
                    device3d.SetCursor(System.Windows.Forms.Cursors.No, true);
                    device3d.ShowCursor(false);
                }
                
                #region Mesh3Ds
                device3d.VertexFormat = CustomVertex.PositionColoredTextured.Format;

                if (Bridge != null)
                    Bridge.Draw(new Vector3(0, -100, -600));

                if (hall_interior != null)
                    hall_interior.Draw(new Vector3(0, -28, 290));

                if (STUT_C_L != null)
                {
                    device3d.Transform.World = Matrix.RotationY((float)Math.PI);
                    STUT_C_L.Draw(new Vector3(-5, -67, -450));
                }

                if (Miz != null)
                {
                    // Left Side of Museum
                    device3d.Transform.World = Matrix.RotationY((float)Math.PI / 2);
                    Miz.Draw(new Vector3(22, -35, 316));
                    device3d.Transform.World = Matrix.RotationY((float)Math.PI / 2);
                    Miz.Draw(new Vector3(22, -35, 273));
                    device3d.Transform.World = Matrix.RotationY((float)Math.PI / 2);
                    Miz.Draw(new Vector3(22, -35, 228));
                    //
                    // Right Side of Museum
                    device3d.Transform.World = Matrix.RotationY(-(float)Math.PI / 2);
                    Miz.Draw(new Vector3(-22, -35, 305));
                    device3d.Transform.World = Matrix.RotationY(-(float)Math.PI / 2);
                    Miz.Draw(new Vector3(-22, -35, 262));
                    device3d.Transform.World = Matrix.RotationY(-(float)Math.PI / 2);
                    Miz.Draw(new Vector3(-22, -35, 217));
                }

                if (ERZHER_H != null)
                {
                    ERZHER_H.Draw(new Vector3(0, -20, 385));
                }

                if (CANNON1 != null)
                {
                    device3d.Transform.World = Matrix.RotationY(-100 * (float)Math.PI / 180);
                    CANNON1.Draw(new Vector3(10, -12, 343));
                }

                if (Urn != null) Urn.Draw(new Vector3(26, -16, 311));

                if (Pottry != null) Pottry.Draw(new Vector3(27, -15, 268));

                if (Urn2 != null) Urn2.Draw(new Vector3(26, -16, 223));

                if (Vase != null) Vase.Draw(new Vector3(-27, -15.5f, 310));

                if (Vase2 != null) Vase2.Draw(new Vector3(-27, -15.5f, 267));

                if (CANOPES != null) CANOPES.Draw(new Vector3(-27, -14.5f, 222.5f));


                device3d.SamplerState[0].MinFilter = TextureFilter.Anisotropic;
                device3d.SamplerState[0].MagFilter = TextureFilter.Anisotropic;
                device3d.SamplerState[0].AddressU = TextureAddress.Mirror;
                device3d.SamplerState[0].AddressV = TextureAddress.Mirror;
                //
                // 
                //
                #endregion
                //
                // runTime Method for Text
                //
                draw_text();
                //
                device3d.EndScene();
                device3d.Present();
                //
                GItb.Poll(); // setCurrent State for Mouse and Keyboard
            }
            else
            {
                //
                // SplashScreen
                //
                sps.Draw();
            }
        }

        

        #region Text
        private Microsoft.DirectX.Direct3D.Font text_Resulation;
        private Microsoft.DirectX.Direct3D.Font text_Framerate;
        private Microsoft.DirectX.Direct3D.Font text_Position;
        private Microsoft.DirectX.Direct3D.Font text_Target;
        private Microsoft.DirectX.Direct3D.Font text_exit;
        /// <summary>
        /// initialize tex
        /// </summary>
        /// <param name="device"></param>
        public void initialize_text(Device device)
        {
            //
            // Create a Font for any Text in Display
            //
            System.Drawing.Font sysFont = new System.Drawing.Font("tahoma", 14f, FontStyle.Bold);
            //
            //initialize text Resulation
            text_Resulation = new Microsoft.DirectX.Direct3D.Font(device, sysFont);
            //initialize text Framerate
            text_Framerate = new Microsoft.DirectX.Direct3D.Font(device, sysFont);
            //initialize text Camera Position
            text_Position = new Microsoft.DirectX.Direct3D.Font(device, sysFont);
            //initialize text Camera Target
            text_Target = new Microsoft.DirectX.Direct3D.Font(device, sysFont);
            //initialize text exit ESC
            text_exit = new Microsoft.DirectX.Direct3D.Font(device, sysFont);
        }
        /// <summary>
        /// draw text
        /// </summary>
        /// <param name="device"></param>
        public void draw_text()
        {
            //
            // Set Graphic Resulations
            //
            text_Resulation.DrawText(null, string.Format("Resulation ({0} , {1})", this.width, this.height), new Point(5, 5), Color.Red);
            //
            // Display ESC key's
            //
            text_exit.DrawText(null, "Please press ESC key's for exit.", new Point(5, 30), Color.LightBlue);
            //
            // Display FPS number
            //
            text_Framerate.DrawText(null, string.Format("Framerate : {0:0.00} fps", Framerate.UpdateFramerate()), new Point(5, 55), Color.LightGreen);
            //
            // Display Camera Position and Target
            //
            text_Position.DrawText(null, string.Format("Camera Position = ({0:0.0}x , {1:0.0}y , {2:0.0}z)",
                CameraPosition.X, CameraPosition.Y, CameraPosition.Z), new Point(5, 80), Color.Aqua);
            text_Target.DrawText(null, string.Format("Camera Target = ({0:0.0}x , {1:0.0}y , {2:0.0}z)",
                CameraTarget.X, CameraTarget.Y, CameraTarget.Z), new Point(5, 105), Color.Aqua);
        }
        #endregion  
    }

}

