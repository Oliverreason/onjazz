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


namespace Jazz.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class Layer : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Constants.GameLayers m_gameLayerType;
        protected List<MenuItem> m_lMenuItems;
        protected bool m_IsDrawable;
        protected bool m_IsSelected;
        protected Vector2 m_vStart;

        public Layer(Game game)
            : base(game)
        {
            m_lMenuItems = new List<MenuItem>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            m_IsDrawable = false;
            m_IsSelected = false;
            m_vStart = new Vector2();
            BuildMenu();
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #region Abstract Functions
        protected abstract void BuildMenu();
        public abstract Constants.GameLayers HandleButton(Buttons button, Constants.GamePad_ButtonState buttonState);
        public abstract GameScreen GetNewGameScreen();
        #endregion

        public bool IsDrawable
        {
            get { return m_IsDrawable; }
            set { m_IsDrawable = value; }
        }
        public bool IsSelected
        {
            get { return m_IsSelected; }
            set { m_IsSelected = value; }
        }
        public Constants.GameLayers LayerType
        {
            get { return m_gameLayerType; }
        }
    }
}