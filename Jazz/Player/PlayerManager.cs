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


namespace Jazz.Player
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PlayerManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Player[] m_thePlayers;
        
        public PlayerManager(Game game)
            : base(game)
        {
            m_thePlayers = new Player[Constants.MAX_PLAYERS];
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                m_thePlayers[i] = new Player(Game,i);
            }
        }

        /// <summary>
        ///      
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: Add your initialization code here

            base.LoadContent();
        }

        /// <summary>
        ///      
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Add your initialization code here

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (GamePad.GetState((PlayerIndex)i).IsConnected)
                    m_thePlayers[i].Update(gameTime);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        public Player GetPlayerIndex(int iIndex)
        {
            return m_thePlayers[iIndex];
        }
    }
}