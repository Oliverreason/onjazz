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
    public abstract class Layer : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected bool m_IsActive = true;
        protected Constants.Layer_state m_lsState;
        public Layer(Game game)
            : base(game)
        {

        }
        public abstract void HandleButton(int playerIndex, Buttons button, Constants.GamePad_ButtonState buttonState);
        public virtual void UnloadContent(){}
        public bool IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }
    }
}