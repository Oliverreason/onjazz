using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Jazz
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public static class Constants
    {
        #region Numbers, Strings
        public const int MAX_PLAYERS = 4;
        public const int MAX_MSPEED = 8;
        public const float MOVEMENT_DAMP = 3.25f;
        // Camera
        public const float FAR_CLIPPING_PLANE = 45.0f;
        public const float NEAR_CLIPPING_PLANE = 1.0f;
        public const float FOV_MIN = 50.0f;
        public const float FOV_MAX = 100.0f;
        public const float FOV_DEFAULT = 90.0f;
        public const float PITCH_MIN = -85.0f;
        public const float PITCH_MAX = 85.0f;
        // Player
        public const string PLAYER_MODEL_1 = "Models//Ninja";
        // Screen
        public const int PREFERRED_HEIGHT = 720;
        public const int PREFERRED_WIDTH = 1280;
        // Fonts
        public const string FONT_DEFAULT = "Fonts//Default";
        public const string FONT_START = "Fonts//Main_Start";
        #endregion


        #region Enumerated Variables
        public enum Movement_state { NO_ACTION, WALK, CROUTCH, SPRINT, JOG };
        public enum Weapon_state { NO_ACTION, GUN_READY, GUN_SHOOT, RELOAD, WEAPON_SWITCH };
        public enum Action_state { SHOOT, JUMP, SPRINT, WALK, THROW_NADE, SWITCH_NADE, CROUTCH, ALT_SHOOT, USE, SWITCH_WEAPON, ASSASSINATE };
        public enum GamePad_ThumbSticks { RIGHT, LEFT };
        public enum GamePad_ButtonState { JUST_PRESSED, PRESSED, JUST_RELEASED, RELEASED };
        public enum GameScreenState { RUNNING, TRANSITION };
        public enum GameLayers { NO_ACTION, MAIN_START, MAIN_CHOICE, MAIN_LAN, MAIN_ONLINE };
        #endregion
    }
}