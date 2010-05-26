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


namespace Jazz.Screens.Layers
{
    /// <summary>
    /// 
    /// </summary>
    public class Main_StartLayer : Layer
    {
        public Main_StartLayer(Game game)
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
            m_gameLayerType = Constants.GameLayers.MAIN_START;
            m_vStart = new Vector2(GraphicsDevice.Viewport.Width / 2,
                                   GraphicsDevice.Viewport.Height * 2 / 3);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i< m_lMenuItems.Count; i++)
            {
                m_lMenuItems[i].Draw(gameTime, m_vStart + i * new Vector2(0, 5.0f));
            }
            base.Draw(gameTime);
        }

        #region Abstract Functions
        protected override void BuildMenu()
        {
            m_lMenuItems.Add(new MenuItems.Main_StartItem(Game));
            foreach (MenuItem menuItem in m_lMenuItems)
            {
                menuItem.Initialize();
            }
            m_lMenuItems[0].IsSelected = true;
        }
        public override Constants.GameLayers HandleButton(Buttons button, Constants.GamePad_ButtonState buttonState)
        {
            Constants.GameLayers result = Constants.GameLayers.NO_ACTION;
            foreach (MenuItem menuItem in m_lMenuItems)
            {
                if (menuItem.IsSelected)
                    result = menuItem.HandleButton(button, buttonState, ref m_IsDrawable);
            }
            if (!m_IsDrawable)
                m_IsSelected = false;
            return result;
        }
        public override GameScreen GetNewGameScreen()
        {
            GameScreen result = null;
            foreach (MenuItem menuItem in m_lMenuItems)
            {
                if (menuItem.IsSelected)
                    result = menuItem.GetNewGameScreen();
            }
            return result;
        }
        #endregion
    }
}