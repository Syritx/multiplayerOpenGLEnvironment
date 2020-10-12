using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL.Environment.Client.Game
{
    public class Player
    {
        // player is just going to be a cube
        Camera camera;
        Vector3 position;

        float size = NPCPlayer.size;
        float yOffset = NPCPlayer.yOffset;

        public Player(Camera camera) {
            this.camera = camera;
        }

        public void render()
        {
            position = camera.position;

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
