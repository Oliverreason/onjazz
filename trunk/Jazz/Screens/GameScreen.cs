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
    public abstract class GameScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Member Variables
        protected List<Layer> m_lLayers;
        #endregion

        public GameScreen(Game game)
            : base(game)
        {
            m_lLayers = new List<Layer>();
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Input.InputManager.HandleButton += new Input.InputManager.InputButtonHandler(HandleButtons);
            BuildLayers();

            base.Initialize();
        }

        
        /// <summary>
        ///      
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        ///      
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
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
            base.Draw(gameTime);
        }

        #region Abstract Functions
        protected abstract void HandleButtons(int playerIndex, Buttons button, Constants.GamePad_ButtonState buttonState);
        public abstract GameScreen GetNewGameScreen();
        protected abstract void BuildLayers();
        #endregion 

    }
}