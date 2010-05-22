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
        #region Event Handling
        public delegate void InputButtonHandler(int playerIndex, Buttons button, Constants.GamePad_ButtonState buttonState);
        public delegate void InputThumbstickHandler(int playerIndex, Constants.GamePad_ThumbSticks thumbstickType, Vector2 thumbstick);
        public static event InputButtonHandler HandleButton;
        public static event InputThumbstickHandler HandleThumbstick;
        #endregion

        #region Member Variables
        protected Dictionary<int, Dictionary<Buttons, Constants.GamePad_ButtonState>> m_dStateOfGamePad;
        protected Dictionary<int, Dictionary<Constants.GamePad_ThumbSticks, Vector2>> m_dStateOfThumbsticks;
        protected List<Button_Package> m_lButtonsChanged;
        protected List<Thumbstick_Package> m_lThumbsticksChanged;
        protected GamePadState[] m_previousGamePadState;
        protected GamePadState[] m_currentGamePadState;

        public class Button_Package {
            private int m_PlayerIndex;
            private Buttons m_Button;
            private Constants.GamePad_ButtonState m_ButtonState;
            public Button_Package(int playerIndex, Buttons button, Constants.GamePad_ButtonState buttonState)
            {
                m_PlayerIndex = playerIndex;
                m_Button = button;
                m_ButtonState = buttonState;
            }
            public int PlayerIndex
            {
                get { return m_PlayerIndex; }
                set { m_PlayerIndex = value; }
            }
            public Buttons Button
            {
                get { return m_Button; }
                set { m_Button = value; }
            }
            public Constants.GamePad_ButtonState ButtonState
            {
                get { return m_ButtonState; }
                set { m_ButtonState = value; }
            }
        }
        public class Thumbstick_Package
        {
            private int m_PlayerIndex;
            private Vector2 m_Thumbstick;
            private Constants.GamePad_ThumbSticks m_ThumbstickType;
            public Thumbstick_Package(int playerIndex, Constants.GamePad_ThumbSticks thumbstickType, Vector2 thumbstick)
            {
                m_PlayerIndex = playerIndex;
                m_Thumbstick = thumbstick;
                m_ThumbstickType = thumbstickType;
            }
            public int PlayerIndex
            {
                get { return m_PlayerIndex; }
                set { m_PlayerIndex = value; }
            }
            public Vector2 Thumbstick
            {
                get { return m_Thumbstick; }
                set { m_Thumbstick = value; }
            }
            public Constants.GamePad_ThumbSticks ThumbstickType
            {
                get { return m_ThumbstickType; }
                set { m_ThumbstickType = value; }
            }
        }
        #endregion

        #region Constructors
        public InputManager(Game game)
            : base(game)
        {
            m_dStateOfGamePad = new Dictionary<int, Dictionary<Buttons, Constants.GamePad_ButtonState>>();
            m_dStateOfThumbsticks = new Dictionary<int, Dictionary<Constants.GamePad_ThumbSticks, Vector2>>();
            m_lButtonsChanged = new List<Button_Package>();
            m_lThumbsticksChanged = new List<Thumbstick_Package>();
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            
            m_previousGamePadState = new GamePadState[Constants.MAX_PLAYERS];
            m_currentGamePadState = new GamePadState[Constants.MAX_PLAYERS];
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                // Initialize Gamepad States
                m_previousGamePadState[i] = GamePad.GetState((PlayerIndex)i);
                m_currentGamePadState[i] = GamePad.GetState((PlayerIndex)i);

                // Initialize newStateofGamePads
                m_dStateOfGamePad.Add(i, new Dictionary<Buttons, Constants.GamePad_ButtonState>());
                foreach (Buttons value in Enum.GetValues(typeof(Buttons)))
                {
                    m_dStateOfGamePad[i].Add(value, Constants.GamePad_ButtonState.RELEASED);
                }

                // Initialize newStateofThumbsticks
                m_dStateOfThumbsticks.Add(i, new Dictionary<Constants.GamePad_ThumbSticks, Vector2>());
                foreach (Constants.GamePad_ThumbSticks value in Enum.GetValues(typeof(Constants.GamePad_ThumbSticks)))
                {
                    m_dStateOfThumbsticks[i].Add(value, new Vector2());
                }
            }
            base.Initialize();
        }
        #endregion

        #region Update
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            for (int iCounter = 0; iCounter < Constants.MAX_PLAYERS; iCounter++)
            {
                // Update previous and current GamePadState
                m_previousGamePadState[iCounter] = m_currentGamePadState[iCounter];
                m_currentGamePadState[iCounter] = GamePad.GetState((PlayerIndex)iCounter);

                // Compare previous to current
                ComparePreviousToCurrent(iCounter);
            }


            HandleButtonStateChange();          // Handle gamepad state changes
            HandleThumbstickStateChange();     // Handle thumbsticks state changes
            base.Update(gameTime);
        }

        #region Update - Helper Methods
        private void ComparePreviousToCurrent(int playerIndex)
        {
            // Button Comparison
            foreach (Buttons value in Enum.GetValues(typeof(Buttons)))
            {
                if (m_previousGamePadState[playerIndex].IsButtonUp(value) && m_currentGamePadState[playerIndex].IsButtonDown(value))
                {
                    if (!m_dStateOfGamePad[playerIndex][value].Equals(Constants.GamePad_ButtonState.JUST_PRESSED))
                    {
                        m_lButtonsChanged.Add(new Button_Package(playerIndex, value, Constants.GamePad_ButtonState.JUST_PRESSED));
                        m_dStateOfGamePad[playerIndex][value] = Constants.GamePad_ButtonState.JUST_PRESSED;
                    }
                }
                else if (m_previousGamePadState[playerIndex].IsButtonDown(value) && m_currentGamePadState[playerIndex].IsButtonDown(value))
                {
                    if (!m_dStateOfGamePad[playerIndex][value].Equals(Constants.GamePad_ButtonState.PRESSED))
                    {
                        m_lButtonsChanged.Add(new Button_Package(playerIndex, value, Constants.GamePad_ButtonState.PRESSED));
                        m_dStateOfGamePad[playerIndex][value] = Constants.GamePad_ButtonState.PRESSED;
                    }
                }
                else if (m_previousGamePadState[playerIndex].IsButtonDown(value) && m_currentGamePadState[playerIndex].IsButtonUp(value))
                {
                    if (!m_dStateOfGamePad[playerIndex][value].Equals(Constants.GamePad_ButtonState.JUST_RELEASED))
                    {
                        m_lButtonsChanged.Add(new Button_Package(playerIndex, value, Constants.GamePad_ButtonState.JUST_RELEASED));
                        m_dStateOfGamePad[playerIndex][value] = Constants.GamePad_ButtonState.JUST_RELEASED;
                    }
                }
                else if (m_previousGamePadState[playerIndex].IsButtonUp(value) && m_currentGamePadState[playerIndex].IsButtonUp(value))
                {
                    if (!m_dStateOfGamePad[playerIndex][value].Equals(Constants.GamePad_ButtonState.RELEASED))
                    {
                        m_lButtonsChanged.Add(new Button_Package(playerIndex, value, Constants.GamePad_ButtonState.RELEASED));
                        m_dStateOfGamePad[playerIndex][value] = Constants.GamePad_ButtonState.RELEASED;
                    }
                }
            }
            // Thumbsticks Comparison
            Vector2 current, previous;
            current = m_currentGamePadState[playerIndex].ThumbSticks.Left;
            previous = m_dStateOfThumbsticks[playerIndex][Constants.GamePad_ThumbSticks.LEFT];
            if (!current.Equals(previous))
            {
                m_lThumbsticksChanged.Add(new Thumbstick_Package(playerIndex,
                                                                 Constants.GamePad_ThumbSticks.LEFT,
                                                                 current));
                m_dStateOfThumbsticks[playerIndex][Constants.GamePad_ThumbSticks.LEFT] = current;
            }
            current = m_currentGamePadState[playerIndex].ThumbSticks.Right;
            previous = m_dStateOfThumbsticks[playerIndex][Constants.GamePad_ThumbSticks.RIGHT];
            if (!current.Equals(previous))
            {
                m_lThumbsticksChanged.Add(new Thumbstick_Package(playerIndex,
                                                                 Constants.GamePad_ThumbSticks.RIGHT,
                                                                 current));
                m_dStateOfThumbsticks[playerIndex][Constants.GamePad_ThumbSticks.RIGHT] = current;
            }

        }

        private void HandleButtonStateChange()
        {
            if (HandleButton != null)
            {
                for (int i = 0; i < m_lButtonsChanged.Count; i++)
                {
                    HandleButton(m_lButtonsChanged[i].PlayerIndex,
                                 m_lButtonsChanged[i].Button,
                                 m_lButtonsChanged[i].ButtonState);
                }
                m_lButtonsChanged.Clear();
            }
        }
        private void HandleThumbstickStateChange()
        {
            if (HandleThumbstick != null)
            {
                for (int i = 0; i < m_lThumbsticksChanged.Count; i++)
                {
                    HandleThumbstick(m_lThumbsticksChanged[i].PlayerIndex,
                                     m_lThumbsticksChanged[i].ThumbstickType,
                                     m_lThumbsticksChanged[i].Thumbstick);
                }
                m_lThumbsticksChanged.Clear();
            }
        }
        #endregion

        #endregion
    }
}