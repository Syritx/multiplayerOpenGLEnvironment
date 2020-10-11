using System;
using System.Net.Sockets;

namespace OpenGL.Environment.Server
{
    public class ClientCommands
    {
        public enum Commands {
            PLAYER_MOVE_FORWARD  = 1,
            PLAYER_MOVE_BACKWARD = 2,
            PLAYER_ROTATE_LEFT   = 3,
            PLAYER_ROTATE_RIGHT  = 4,
            PLAYER_ROTATE_UP     = 5,
            PLAYER_ROTATE_DOWN   = 6,
        };

        public string[] startMessages = {
            "[CO-NNE_CT3D-TO_$ER_VER}"
        };
    }
}
