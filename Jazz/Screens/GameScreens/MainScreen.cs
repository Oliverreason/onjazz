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


namespace Jazz.Screens.GameScreens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MainScreen : GameScreen
    {
        #region Member Variables
        

        #endregion

        public MainScreen(Game game)
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
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Layer layer in m_lLayers)
            {
                if (layer.IsDrawable)
                {
                    layer.Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }

        #region Abstract Functions
        protected override void HandleButtons(int playerIndex, Buttons button, Constants.GamePad_ButtonState buttonState)
        {
            Constants.GameLayers result = Constants.GameLayers.NO_ACTION;
            foreach (Layer layer in m_lLayers)
            {
                if (layer.IsSelected)
                {
                    result = layer.HandleButton(button, buttonState);
                    if (result != Constants.GameLayers.NO_ACTION)
                    {
                        foreach (Layer layer2 in m_lLayers)
                        {
                            if (layer2.LayerType.Equals(result))
                            {
                                layer2.IsSelected = true;
                                layer2.IsDrawable = true;
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            
        }
        public override GameScreen GetNewGameScreen()
        {
            GameScreen result = null;
            foreach (Layer layer in m_lLayers)
            {
                if (layer.IsSelected)
                {
                    result = layer.GetNewGameScreen();
                    break;
                }
            }
            return result;
        }
        protected override void BuildLayers()
        {
            m_lLayers.Add(new Layers.Main_StartLayer(Game));
            m_lLayers.Add(new Layers.Main_ChoiceLayer(Game));
            //m_lLayers.Add(new Layers.Main_LANLayer());
            //m_lLayers.Add(new Layers.Main_LAN_OptionsLayer());
            foreach (Layer layer in m_lLayers)
            {
                layer.Initialize();
            }
            m_lLayers[0].IsDrawable = true;
            m_lLayers[0].IsSelected = true;
        }
        #endregion 

        #region Extra Functions
        
        #endregion
    }
}