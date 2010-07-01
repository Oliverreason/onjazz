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


namespace Jazz.Layers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class SubMenu : Layer
    {
        protected Vector2 m_vPosition;
        protected float m_fChoiceToChoiceOffset;
        protected float m_fLeftPadding;
        protected float m_fPushOffest;
        protected List<MenuItem_Choice> m_lItems;
        protected Menu m_lPrevious;
        protected bool m_isPositionEstablished;

        protected float m_fTransitionIn;
        protected float m_fTransitionOut;
        protected float m_fTransitionCounter;
        protected float m_fTransparency;

        public SubMenu(Game game)
            : base(game)
        {
            m_lsState = Constants.Layer_state.NO_ACTION;
            m_lItems = new List<MenuItem_Choice>();
            m_lPrevious = null;
            m_fChoiceToChoiceOffset = 50.0f;
            m_fLeftPadding = 40.0f;
            m_fPushOffest = 30.0f;
            m_fTransitionCounter = 0.0f;
            m_vPosition = new Vector2();
            m_isPositionEstablished = false;
        }
        public SubMenu(Game game, Menu previous)
            : this(game)
        {
            m_lPrevious = previous;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(List<MenuItem_Choice> lItems)
        {
            base.Initialize();
            Input.InputManager.HandleButton += new Input.InputManager.InputButtonHandler(HandleButton);

            m_lsState = Constants.Layer_state.TRANSITION_IN;
            m_vPosition = new Vector2();
            m_lItems = new List<MenuItem_Choice>();
            if (lItems.Count > 0)
            {
                lItems[0].IsSelected = true;
                lItems[0].IsValueChanged = true;
            }
            foreach (MenuItem_Choice choice in lItems)
            {
                m_lItems.Add(choice);
            }
            SetUpTransitions();
            SetUpOffsets();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Get position of this menu only once
            if (!m_isPositionEstablished) 
            {
                m_vPosition = m_lPrevious.GetSelectedMenuItemPosition();
                m_isPositionEstablished = true;
            }
            // Delta time
            float fDeltaTime = (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerSecond;

            if (m_lsState == Constants.Layer_state.TRANSITION_IN)
            {
                m_fTransitionCounter += fDeltaTime;
                if (m_fTransitionIn < m_fTransitionCounter)
                {
                    m_fTransitionCounter = 0.0f;
                    m_lsState = Constants.Layer_state.NO_ACTION;
                }
            }
            else if (m_lsState == Constants.Layer_state.TRANSITION_OUT || m_lsState == Constants.Layer_state.INACTIVE)
            {
                m_fTransitionCounter += fDeltaTime;
                if (m_fTransitionOut < m_fTransitionCounter)
                {
                    UnloadContent();
                    LayerManager.Singleton.RemoveLayer(this);
                    if (m_lsState == Constants.Layer_state.INACTIVE)
                        Game.Exit();
                }
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            // Calculate Transparency
            float transparency = 0.0f;
            if (m_lsState == Constants.Layer_state.TRANSITION_IN)
                transparency = (1 - (m_fTransitionIn - m_fTransitionCounter) / m_fTransitionIn) * m_fTransparency;
            else if (m_lsState == Constants.Layer_state.TRANSITION_OUT ||
                    m_lsState == Constants.Layer_state.INACTIVE)
                transparency = ((m_fTransitionOut - m_fTransitionCounter) / m_fTransitionOut) * m_fTransparency;
            else
                transparency = m_fTransparency;

            for (int i = 0; i < m_lItems.Count; i++)
            {
                Vector2 offset = new Vector2(m_fLeftPadding, m_fChoiceToChoiceOffset * i);
                if (m_lItems[i].IsSelected)
                    offset.X += m_fPushOffest;
                m_lItems[i].Draw(gameTime, offset + m_vPosition, transparency);
            }
        }

        public override void UnloadContent()
        {
            Input.InputManager.HandleButton -= new Input.InputManager.InputButtonHandler(HandleButton);
            foreach (MenuItem_Choice choice in m_lItems)
            {
                choice.UnloadContent();
            }
        }

        public override void HandleButton(int playerIndex, Buttons button, Constants.GamePad_ButtonState buttonState)
        {
            if (playerIndex == 0 && buttonState.Equals(Constants.GamePad_ButtonState.JUST_PRESSED))
            {
                // Directional Handling
                // Direction: Up
                if (button.Equals(Buttons.DPadUp) ||
                    button.Equals(Buttons.LeftThumbstickUp))
                {
                    int indexSelected = -1;
                    for (int i = 0; i < m_lItems.Count; i++)
                    {
                        if (m_lItems[i].IsSelected)
                        {
                            indexSelected = i;
                            m_lItems[i].IsSelected = false;
                            m_lItems[i].IsValueChanged = false;
                        }
                    }
                    if (indexSelected == 0)
                        indexSelected = m_lItems.Count - 1;
                    else
                        indexSelected--;
                    m_lItems[indexSelected].IsSelected = true;
                    m_lItems[indexSelected].IsValueChanged = true;
                }
                // Direction: Down
                if (button.Equals(Buttons.DPadDown) ||
                    button.Equals(Buttons.LeftThumbstickDown))
                {
                    int indexSelected = -1;
                    for (int i = 0; i < m_lItems.Count; i++)
                    {
                        if (m_lItems[i].IsSelected)
                        {
                            indexSelected = i;
                            m_lItems[i].IsSelected = false;
                            m_lItems[i].IsValueChanged = false;
                        }
                    }
                    if (indexSelected == m_lItems.Count - 1)
                        indexSelected = 0;
                    else
                        indexSelected++;
                    m_lItems[indexSelected].IsSelected = true;
                    m_lItems[indexSelected].IsValueChanged = true;
                }
                // Button Handling
                if (button.Equals(Buttons.A))
                {
                    for (int i = 0; i < m_lItems.Count; i++)
                    {
                        if (m_lItems[i].IsSelected)
                        {
                            if (m_lItems[i].NextLayer == null)
                                Game.Exit();
                            else if (m_lItems[i].NextLayer is EmptyLayer)
                            {
                                //Do Nothing
                            }
                            else
                            {
                                LayerManager.Singleton.AddLayer(m_lItems[i].NextLayer);
                                m_IsActive = false;
                                if (!m_lItems[i].IsCurrentLayerRemaining)
                                {
                                    m_lsState = Constants.Layer_state.TRANSITION_OUT;
                                }
                            }
                        }
                    }
                }
                if (m_lPrevious != null && button.Equals(Buttons.B))
                {
                    m_lPrevious.IsActive = true;
                    LayerManager.Singleton.RemoveLayer(this);
                }
            }
        }

        #region Abstract Functions
        protected abstract void SetUpTransitions();
        protected abstract void SetUpOffsets();
        #endregion

        protected MenuItem GetSelectedMenuItem()
        {
            MenuItem selected = null;
            foreach (MenuItem value in m_lItems)
            {
                if (value.IsSelected)
                {
                    selected = value;
                    break;
                }
            }
            return selected;
        }

        public Vector2 Position
        {
            get { return m_vPosition; }
            set { m_vPosition = value; }
        }


    }
}