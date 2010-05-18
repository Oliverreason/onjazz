#region Using Statements
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
#endregion

namespace Jazz.Input
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InputManager : Microsoft.Xna.Framework.GameComponent
    {
        InputState m_inputState;
        Game m_game;
        public InputManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            m_game = game;
            m_inputState = new InputState(game);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();  
            // TODO: Add your initialization code here
            m_inputState.Initialize();

        }

        #region Update
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Input Update
            m_inputState.Update(gameTime);
            
        }

        public void Update(GameTime gameTime, Player.PlayerManager playerManager)
        {
            Update(gameTime);
            if (playerManager != null)
                CalculateEvents(gameTime, playerManager);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void CalculateEvents(GameTime gameTime, Player.PlayerManager playerManager)
        {
            for (int i = 0; i < Constants.MAX_PLAYERS; i++){
                if (GamePad.GetState((PlayerIndex)i).IsConnected){
                    GamePadState previousGamePadState = m_inputState.m_previousGamePadState[i];
                    GamePadState currentGamePadState = m_inputState.m_currentGamePadState[i];
                    // Check GamePad Buttons and Sticks
                    foreach (Buttons value in Enum.GetValues(typeof(Buttons)))
                    {
                        if (previousGamePadState.IsButtonUp(value) && currentGamePadState.IsButtonDown(value))
                        {
                            playerManager.GetPlayerIndex(i).HandleButton(value, Constants.GamePad_ButtonState.JUST_PRESSED);
                        }
                        else if (previousGamePadState.IsButtonDown(value) && currentGamePadState.IsButtonDown(value))
                        {
                            playerManager.GetPlayerIndex(i).HandleButton(value, Constants.GamePad_ButtonState.PRESSED);
                        }
                        else if (previousGamePadState.IsButtonDown(value) && currentGamePadState.IsButtonUp(value))
                        {
                            playerManager.GetPlayerIndex(i).HandleButton(value, Constants.GamePad_ButtonState.JUST_RELEASED);
                        }
                    }
                    //playerManager.GetPlayerIndex(i).HandleStick(gameTime, Constants.Thumbstick_selection.LEFT);
                    playerManager.GetPlayerIndex(i).HandleStick(gameTime, Constants.Thumbstick_selection.RIGHT);
                }
            }
        }
        #endregion

    }
}