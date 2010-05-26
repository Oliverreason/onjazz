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


namespace Jazz.Screens.MenuItems
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Main_StartItem : MenuItem
    {
        public Main_StartItem(Game game)
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
            m_sValue = "Press 'START' to Continue.";
            LoadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_font = m_ContentManager.Load<SpriteFont>(Constants.FONT_START);
            
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
        public override Constants.GameLayers HandleButton(Buttons button, Constants.GamePad_ButtonState buttonState, ref bool IsLayerDrawble)
        {
            Constants.GameLayers result = Constants.GameLayers.NO_ACTION;
            if (button.Equals(Buttons.Start) &&
                buttonState.Equals(Constants.GamePad_ButtonState.JUST_PRESSED) &&
                m_IsSelected)
            {
                result = Constants.GameLayers.MAIN_CHOICE;
                IsLayerDrawble = false;
            }
            return result;
        }
        public override GameScreen GetNewGameScreen()
        {
            return null;
        }
        public override void Draw(GameTime gameTime, Vector2 vPosition)
        {
            m_spriteBatch.Begin();
            vPosition -= m_font.MeasureString(m_sValue) / 2;

            // Draw the string
            m_spriteBatch.DrawString(m_font, m_sValue, vPosition, Color.Red);
            m_spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion
    }
}