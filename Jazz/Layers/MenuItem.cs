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
    public abstract class MenuItem : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Member Variables
        protected bool m_IsSelected = false;
        protected bool m_IsValueChanged = false;
        protected string m_sValue;
        protected Layer m_nextLayer;
        protected bool m_IsCurrentLayerRemaining;
        protected ContentManager m_ContentManager;
        protected SpriteFont m_font;
        protected SpriteBatch m_spriteBatch;
        protected Color m_color;
        protected Constants.MenuItem_Prefix m_prefix;
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
            base.Initialize();
            m_IsSelected = false;
            m_IsCurrentLayerRemaining = false;
            m_sValue = "";
            m_color = Color.Red;
            m_prefix = Constants.MenuItem_Prefix.NONE;
            LoadContent();
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
        public new void UnloadContent()
        {
            m_ContentManager.Unload();

            base.UnloadContent();
        }
        #endregion

        #region Abstract Functions
        public abstract void Draw(GameTime gameTime, Vector2 position, float transparency);
        #endregion

        public bool IsSelected
        {
            get { return m_IsSelected; }
            set { m_IsSelected = value; }
        }
        public bool IsValueChanged
        {
            get { return m_IsValueChanged; }
            set { m_IsValueChanged = value; }
        }
        public string Value
        {
            get { return m_sValue; }
            set { m_sValue = value; }
        }
        public bool IsCurrentLayerRemaining
        {
            get { return m_IsCurrentLayerRemaining; }
            set { m_IsCurrentLayerRemaining = value; }
        }
        public Constants.MenuItem_Prefix Prefix
        {
            get { return m_prefix; }
            set { m_prefix = value; }
        }
        public Layer NextLayer
        {
            get { return m_nextLayer; }
        }
        public float Length
        {
            get { return m_font.MeasureString(m_sValue).X; }
        }
    }
}