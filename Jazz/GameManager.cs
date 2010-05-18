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
        protected Player.PlayerManager m_playerManager;
        public static int m_iNumPlayersActive = 0;
        public static Game m_game;

        public GameManager(Game game)
            : base(game)
        {
            m_game = game;
            m_inputManager = new Input.InputManager(game);
            m_playerManager = new Player.PlayerManager(game);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            m_inputManager.Initialize();
            m_playerManager.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Check for players active
            m_iNumPlayersActive = m_playerManager.GetNumPlayersActive();
            m_inputManager.Update(gameTime, m_playerManager);
            m_playerManager.Update(gameTime);

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
            Viewport temp = Game.GraphicsDevice.Viewport;
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                Player.Player player = m_playerManager.GetPlayerIndex(i);
                if (player.IsActive)
                {
                    Game.GraphicsDevice.Viewport = player.FirstPersonCamera.TheViewPort;
                    m_playerManager.Draw(gameTime, player.FirstPersonCamera.ViewMatrix, player.FirstPersonCamera.ProjectionMatrix, i);
                }
            }
            Game.GraphicsDevice.Viewport = temp;
        }
    }
}