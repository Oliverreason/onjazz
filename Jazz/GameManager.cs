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
        // Member Variables
        protected Input.InputManager m_inputManager;
        public static int m_iNumPlayersActive = 0;

        public GameManager(Game game)
            : base(game)
        {
            m_inputManager = new Input.InputManager(game);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            m_inputManager.Initialize();
            Layers.LayerManager.Singleton.Initialize(Game);
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            m_inputManager.Update(gameTime);
            Layers.LayerManager.Singleton.Update(gameTime);

            base.Update(gameTime);
        }

        public  void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }

        public void Draw(GameTime gameTime)
        {
            Layers.LayerManager.Singleton.Draw(gameTime);
        }
    }
}