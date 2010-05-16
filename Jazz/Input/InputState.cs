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


namespace Jazz.Input
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InputState : Microsoft.Xna.Framework.GameComponent
    {
        
        // Member Variables
        public GamePadState[] m_previousGamePadState;
        public GamePadState[] m_currentGamePadState;

        public InputState(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            // Initialize Gamepad States
            m_previousGamePadState = new GamePadState[Constants.MAX_PLAYERS];
            m_currentGamePadState = new GamePadState[Constants.MAX_PLAYERS];
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                m_previousGamePadState[i] = GamePad.GetState((PlayerIndex)i);
                m_currentGamePadState[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Update previous and current GamePadState
            for (int iCounter = 0; iCounter < Constants.MAX_PLAYERS; iCounter++)
            {
                m_previousGamePadState[iCounter] = m_currentGamePadState[iCounter];
                m_currentGamePadState[iCounter] = GamePad.GetState((PlayerIndex)iCounter);
            }
        }
    }
}