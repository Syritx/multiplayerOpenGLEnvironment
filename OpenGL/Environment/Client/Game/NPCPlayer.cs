using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL.Environment.Client.Game
{
    public class NPCPlayer
    {
        public static float size = 2;
        public static float yOffset = -5;

        public Vector3 position = new Vector3(0,0,0);

        public NPCPlayer(Vector3 position) {
            this.position = position;
        }

        public void render() {
            GL.Begin(BeginMode.Quads);

            GL.Color3(1.0, 0.5, 0);
            // FRONT
            GL.Vertex3(-size + position.X,  size + yOffset + position.Y, -size + position.Z);
            GL.Vertex3( size + position.X,  size + yOffset + position.Y, -size + position.Z);
            GL.Vertex3( size + position.X, -size + yOffset + position.Y, -size + position.Z);
            GL.Vertex3(-size + position.X, -size + yOffset + position.Y, -size + position.Z);

            GL.Color3(1.0, 1.0, 1.0);
            // BACK
            GL.Vertex3(-size + position.X,  size + yOffset + position.Y, size + position.Z);
            GL.Vertex3( size + position.X,  size + yOffset + position.Y, size + position.Z);
            GL.Vertex3( size + position.X, -size + yOffset + position.Y, size + position.Z);
            GL.Vertex3(-size + position.X, -size + yOffset + position.Y, size + position.Z);

            GL.Color3(1.0, 0.0, 0.0);
            // LEFT
            GL.Vertex3(-size + position.X,  size + yOffset + position.Y, -size + position.Z);
            GL.Vertex3(-size + position.X,  size + yOffset + position.Y,  size + position.Z);
            GL.Vertex3(-size + position.X, -size + yOffset + position.Y,  size + position.Z);
            GL.Vertex3(-size + position.X, -size + yOffset + position.Y, -size + position.Z);

            GL.Color3(0.0, 1.0, 0.0);
            // RIGHT
            GL.Vertex3( size + position.X,  size + yOffset + position.Y, -size + position.Z);
            GL.Vertex3( size + position.X,  size + yOffset + position.Y,  size + position.Z);
            GL.Vertex3( size + position.X, -size + yOffset + position.Y,  size + position.Z);
            GL.Vertex3( size + position.X, -size + yOffset + position.Y, -size + position.Z);

            GL.Color3(0.0, 0.0, 1.0);
            // TOP
            GL.Vertex3(-size + position.X,  size + yOffset + position.Y, -size + position.Z);
            GL.Vertex3(-size + position.X,  size + yOffset + position.Y,  size + position.Z);
            GL.Vertex3( size + position.X,  size + yOffset + position.Y,  size + position.Z);
            GL.Vertex3( size + position.X,  size + yOffset + position.Y, -size + position.Z);

            GL.Color3(1.0, 0.0, 1.0);
            // BOTTOM
            GL.Vertex3(-size + position.X, -size + yOffset + position.Y, -size + position.Z);
            GL.Vertex3(-size + position.X, -size + yOffset + position.Y,  size + position.Z);
            GL.Vertex3( size + position.X, -size + yOffset + position.Y,  size + position.Z);
            GL.Vertex3( size + position.X, -size + yOffset + position.Y, -size + position.Z);

            GL.End();
        }
    }
}
