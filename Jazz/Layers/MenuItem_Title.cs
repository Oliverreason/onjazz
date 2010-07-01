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
    public class MenuItem_Title : MenuItem
    {
        

        public MenuItem_Title(Game game)
            : base(game)
        {
        }
        public MenuItem_Title(Game game, string title)
            : base(game)
        {
            Initialize();
            m_sValue = title;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_font = m_ContentManager.Load<SpriteFont>(Constants.FONT_TITLE);
        }

        public override void Draw(GameTime gameTime, Vector2 position, float transparency)
        {
            m_spriteBatch.Begin();
            m_spriteBatch.DrawString(m_font, m_sValue, position, new Color(m_color, transparency));
            m_spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}