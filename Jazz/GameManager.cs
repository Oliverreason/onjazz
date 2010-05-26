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


namespace Jazz
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameManager : Microsoft.Xna.Framework.GameComponent
    {
        #region Member Variables
        protected Input.InputManager m_inputManager;
        protected Screens.GameScreen m_previousGameScreen,
                                     m_currentGameScreen;
        protected Constants.GameScreenState m_gameScreenState;
        #endregion

        public GameManager(Game game)
            : base(game)
        {
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            m_inputManager = new Input.InputManager(Game);
            m_currentGameScreen = new Screens.GameScreens.MainScreen(Game);
            m_gameScreenState = Constants.GameScreenState.TRANSITION;

            m_inputManager.Initialize();
            m_currentGameScreen.Initialize();
        

            base.Initialize();
        }

        public void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            m_inputManager.Update(gameTime);
            if (m_gameScreenState == Constants.GameScreenState.TRANSITION)
                m_gameScreenState = Constants.GameScreenState.RUNNING;
            else if (m_gameScreenState == Constants.GameScreenState.RUNNING)
                m_currentGameScreen.Update(gameTime);

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            m_currentGameScreen.Draw(gameTime);            
        }

        public void ScreenStateChanged()
        {
            m_previousGameScreen = m_currentGameScreen;
            m_currentGameScreen = m_previousGameScreen.GetNewGameScreen();
        }
    }
}