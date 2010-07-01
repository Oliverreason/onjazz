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

namespace Jazz.Layers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EmptyLayer : Layer
    {       
        public EmptyLayer(Game game)
            : base(game)
        {
        }
        public override void HandleButton(int playerIndex, Buttons button, Constants.GamePad_ButtonState buttonState)
        { }
    }

}