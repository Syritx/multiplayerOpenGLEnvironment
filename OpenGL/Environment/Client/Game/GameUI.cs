using System;
using System.Collections.Generic;
using OpenGL.Environment.Game.Landscape;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenGL.Environment.Client.Game
{
    public class GameUI : GameWindow
    {
        List<Vector3> heightMaps;
        int length = 300;
        int seed = 8239234;

        Camera camera;
        Player player;
        ImprovedNoise noise = new ImprovedNoise();

        public static List<NPCPlayer> players;

        public GameUI(int width, int height, string title)
            : base(width,height,GraphicsMode.Default,title) {

            heightMaps = new List<Vector3>();
            camera = new Camera(this);
            player = new Player(camera);

            players = new List<NPCPlayer>();
            double frequency = 30;

            for (int x = 0; x < length; x++) {
                for (int z = 0; z < length; z++) {

                    double x1 = (double)x / length * frequency;
                    double z1 = (double)z / length * frequency;

                    double h = noise.noise(x1, z1, seed)*50;
                    heightMaps.Add(new Vector3(x * 10, (float)h, z * 10));
                }
            }

            for (int i = 0; i < 10; i++) {
                players.Add(null);
                Console.WriteLine("created player npc");
            }
            Run(60);
        }

        public static void CreatePlayer(Vector3 position, int id) {
            players[id] = new NPCPlayer(position);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            var view = Matrix4.LookAt(camera.position, camera.position + camera.front, camera.up);
            GL.LoadMatrix(ref view);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            player.render();

            try { 
                foreach (NPCPlayer NPC in players) {
                    if (NPC != null) 
                        NPC.render();
                }
            }catch(InvalidOperationException e1) {}

            GL.Enable(EnableCap.Fog);

            float[] colors = { 230, 230, 230 };
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Hint(HintTarget.FogHint, HintMode.Nicest);
            GL.Fog(FogParameter.FogColor, colors);

            GL.Fog(FogParameter.FogStart, (float)1000 / 100.0f);
            GL.Fog(FogParameter.FogEnd, 550.0f);

            for (int i = 0; i < heightMaps.Count; i++)
            {
                GL.Begin(BeginMode.Quads);
                GL.Color3((double)114 / 255, (double)179 / 255, (double)29 / 255);
                GL.Vertex3(heightMaps[i]);
                try {

                    int x = (int)heightMaps[i + 1].X;
                    if ((int)heightMaps[i].X == x)
                        GL.Vertex3(heightMaps[i + 1]);

                    int x1 = (int)heightMaps[i+length+1].X,
                        x2 = (int)heightMaps[i+length].X;

                    if (x1 == x2) {
                        GL.Vertex3(heightMaps[i+length+1]);
                        GL.Vertex3(heightMaps[i+length]);
                    }
                }
                catch(Exception e1) {}
                GL.End();
            }

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Matrix4 perspectiveMatrix =
                Matrix4.CreatePerspectiveFieldOfView(1, Width / Height, 1.0f, 2000.0f);

            GL.LoadMatrix(ref perspectiveMatrix);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.End();
            base.OnResize(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0, 0, 0, 0);
            base.OnLoad(e);
        }

        public void CommunicateWithClient(string Command) {
            Client.sendMessage(Command);
        }
    }
}
