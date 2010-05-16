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
        // Constant numbers
        public static int MAX_PLAYERS = 4;
        public static int MAX_MSPEED = 255;

        // Enums
        public enum Movement_state { NO_ACTION, WALK, CROUTCH, SPRINT, JOG };
        public enum Weapon_state { NO_ACTION, GUN_READY, GUN_SHOOT, RELOAD, WEAPON_SWITCH };
        public enum Action_state { SHOOT, JUMP, SPRINT, WALK, THROW_NADE, SWITCH_NADE, CROUTCH, ALT_SHOOT, USE, SWITCH_WEAPON, ASSASSINATE };
        public enum Thumbstick_selection { RIGHT, LEFT };
        public enum GamePad_ButtonState { JUST_PRESSED, PRESSED, JUST_RELEASED, RELEASED };
    }
}