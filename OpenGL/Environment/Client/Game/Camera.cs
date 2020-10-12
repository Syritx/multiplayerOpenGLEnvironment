using System;
using OpenTK;
using OpenTK.Input;

namespace OpenGL.Environment.Client.Game
{
    public class Camera
    {
        public Vector3 position = new Vector3(-50, 20, 0);
        public Vector3 front = new Vector3(0.0f, 0.0f, -0.001f);
        public Vector3 up = new Vector3(0.0f, .01f, 0.0f);

        private GameUI game;
        bool canRotate = false;

        float xRotation,
              yRotation;

        float speed = 5,
              rotateSensitivity = .5f;

        Vector2 lastPosition;

        public Camera(GameUI game) {
            this.game = game;

            game.KeyDown += keyDown;
            game.UpdateFrame += update;

            game.MouseDown += mouseDown;
            game.MouseUp += mouseUp;
            game.MouseMove += mouseMove;
        }

        private void update(object sender, FrameEventArgs e) {
            xRotation = Clamp(xRotation, -89.9f, 89.9f);

            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(xRotation)) * (float)Math.Cos(MathHelper.DegreesToRadians(yRotation));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(xRotation));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(xRotation)) * (float)Math.Sin(MathHelper.DegreesToRadians(yRotation));

            front = Vector3.Normalize(front);
        }

        private void mouseMove(object sender, MouseMoveEventArgs e) {
            if (canRotate)
            {
                xRotation += (lastPosition.Y - e.Y) * rotateSensitivity;
                yRotation -= (lastPosition.X - e.X) * rotateSensitivity;
            }
            lastPosition = new Vector2(e.X, e.Y);
        }

        private void mouseDown(object sender, MouseButtonEventArgs e) {
            if (e.Mouse.IsButtonDown(MouseButton.Right)) canRotate = true;
        }
        private void mouseUp(object sender, MouseButtonEventArgs e) {
            if (e.Mouse.IsButtonUp(MouseButton.Right)) canRotate = false;
        }

        private void keyDown(object sender, KeyboardKeyEventArgs e) {
            if (e.Key == Key.W) position += front * speed;
            if (e.Key == Key.S) position -= front * speed;

            game.CommunicateWithClient("[POSITION]: "+position.X+", "+position.Y+", "+position.Z);
        }

        float Clamp(float value, float min, float max) {

            if (value > max) return max;
            if (value < min) return min;
            return value;
        }
    }
}
