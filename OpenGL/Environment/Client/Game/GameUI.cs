using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenGL.Environment.Client.Game
{
    public class GameUI : GameWindow
    {
        Camera camera;
        Player player;

        public GameUI(int width, int height, string title)
            : base(width,height,GraphicsMode.Default,title) {
            camera = new Camera(this);
            player = new Player(camera);

            Run(60);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            var view = Matrix4.LookAt(camera.position, camera.position + camera.front, camera.up);
            GL.LoadMatrix(ref view);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            player.render();

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
