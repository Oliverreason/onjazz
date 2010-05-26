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
    public abstract class MenuItem : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Member Variables
        protected bool m_IsSelected = false;
        protected string m_sValue;
        protected ContentManager m_ContentManager;
        protected SpriteFont m_font;
        protected SpriteBatch m_spriteBatch;
        #endregion

        public MenuItem(Game game)
            : base(game)
        {
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            m_IsSelected = false;
            m_sValue = "";
            base.Initialize();
        }

        #region Content
        /// <summary>
        ///      
        /// </summary>
        protected override void LoadContent()
        {
            m_ContentManager = new ContentManager(Game.Services, "Content");

            // Create a new SpriteBatch, which can be used to draw textures.
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        /// <summary>
        ///      
        /// </summary>
        protected override void UnloadContent()
        {
            m_ContentManager.Unload();

            base.UnloadContent();
        }
        #endregion


        #region Abstract Functions
        public abstract Constants.GameLayers HandleButton(Buttons button, Constants.GamePad_ButtonState buttonState, ref bool IsLayerDrawble);
        public abstract GameScreen GetNewGameScreen();
        public abstract void Draw(GameTime gameTime, Vector2 vPosition);
        #endregion

        public void ToggleSelected() {
            m_IsSelected = !m_IsSelected;
        }
        public bool IsSelected
        {
            get { return m_IsSelected; }
            set { m_IsSelected = value; }
        }
    }
}