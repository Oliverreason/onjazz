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
    public class PlayerManager : Microsoft.Xna.Framework.GameComponent
    {
        #region Member Variables

        protected Player[] m_thePlayers;


        #endregion

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
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                m_thePlayers[i] = new Player(Game, i);
                m_thePlayers[i].Initialize();
            }
            Input.InputManager.HandleButton += new Input.InputManager.InputButtonHandler(HandleButtons);
            Input.InputManager.HandleThumbstick += new Input.InputManager.InputThumbstickHandler(HandleThumbsticks);
            SetUpPlayers();     // Testing Purposes Only
            base.Initialize();            
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (m_thePlayers[i].IsActive)
                    m_thePlayers[i].Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime, Matrix view, Matrix projection, int playerIndex)
        {
            //for (int m = 0; m < Constants.MAX_PLAYERS; m++)
            //{
                for (int i = 0; i < Constants.MAX_PLAYERS; i++)
                {

                    if (m_thePlayers[i].IsActive && playerIndex != i)
                        m_thePlayers[i].Draw(gameTime, view, projection);
                }
            //}
        }

        // Extras
        public Player GetPlayerIndex(int iIndex)
        {
            return m_thePlayers[iIndex];
        }
        public int GetNumPlayersActive()
        {
            int numPlayersActive = 0;
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (m_thePlayers[i].IsActive)
                    numPlayersActive++;
            }
            return numPlayersActive;
        }

        private void HandleButtons(int playerIndex, Buttons button, Constants.GamePad_ButtonState buttonState)
        {
            m_thePlayers[playerIndex].HandleButton(button, buttonState);
        }
        private void HandleThumbsticks(int playerIndex, Constants.GamePad_ThumbSticks thumbstickType, Vector2 thumbstick)
        {
            m_thePlayers[playerIndex].HandleThumbstick(thumbstickType, thumbstick);
        }

        // Testing Purposes Only
        private void SetUpPlayers()
        {
            // Hard-Code Must Remove
            m_thePlayers[0].IsActive = true;
            m_thePlayers[1].IsActive = true;
            m_thePlayers[2].IsActive = true;
            m_thePlayers[3].IsActive = true;

            m_thePlayers[0].Position = new Vector3(0.0f, 0.0f, 0.0f);
            m_thePlayers[1].Position = new Vector3(0.0f, 0.0f, -10.0f);
            m_thePlayers[2].Position = new Vector3(0.0f, 0.0f, 10.0f);
            m_thePlayers[3].Position = new Vector3(5.0f, 0.0f, 0.0f);

            //m_thePlayers[0].FirstPersonCamera.Forward = new Vector3(0.0f, 0.0f, 1.0f);
            //m_thePlayers[1].Facing = new Vector3(0.0f, 0.0f, 1.0f);
            //m_thePlayers[1].Rotation = new Quaternion(0.0f, 90.0f, 0.0f, 1.0f);
        }
    }
}