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
    public class MenuItem_Choice : MenuItem
    {

        public MenuItem_Choice(Game game)
            : base(game)
        {
        }
        public MenuItem_Choice(Game game, string value, Layer nextLayer, bool IsCurrentLayerRemaining)
            : this(game)
        {
            Initialize(value, nextLayer, IsCurrentLayerRemaining);
        }
        public MenuItem_Choice(Game game, string value, Layer nextLayer, Constants.MenuItem_Prefix prefix, bool IsCurrentLayerRemaining)
            : this(game, value, nextLayer, IsCurrentLayerRemaining)
        {
            m_prefix = prefix;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(string value, Layer nextLayer, bool IsCurrentLayerRemaining)
        {
            base.Initialize();

            m_sValue = value;
            m_nextLayer = nextLayer;
            m_IsCurrentLayerRemaining = IsCurrentLayerRemaining;
            m_prefix = Constants.MenuItem_Prefix.FORWARD_SLASH;
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            m_font = m_ContentManager.Load<SpriteFont>(Constants.FONT_CHOICE);
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            
        }

        public override void Draw(GameTime gameTime, Vector2 position, float transparency)
        {
            m_spriteBatch.Begin();
            // Draw the string
            m_spriteBatch.DrawString(m_font, CalculateNewString(), position, new Color(m_color, transparency));
            m_spriteBatch.End();
            base.Draw(gameTime);
        }

        private string CalculateNewString()
        {
            string temp = m_sValue;
            if (m_IsValueChanged)
            {
                switch(m_prefix){
                    case Constants.MenuItem_Prefix.FORWARD_SLASH:
                        temp = "//" + temp;
                        break;
                    case Constants.MenuItem_Prefix.GREATER_THAN:
                        temp = ">>" + temp;
                        break;
                    case Constants.MenuItem_Prefix.LESS_THAN:
                        temp = "<<" + temp;
                        break;
                    case Constants.MenuItem_Prefix.MINUS:
                        temp = "-" + temp;
                        break;
                    default:
                        break;
                }
            }
            return temp;
        }
    }
}