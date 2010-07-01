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
    public abstract class Menu : Layer
    {
        protected Vector2 m_vPosition;
        protected float m_fTitleToChoiceOffset;
        protected float m_fChoiceToChoiceOffset;
        protected float m_fLeftPadding;
        protected float m_fPushOffest;
        protected MenuItem_Title m_Title;
        protected List<MenuItem_Choice> m_lItems;
        protected Layer m_lPrevious;
        protected VertexPositionColor[] m_pointList;
        protected VertexBuffer m_vertexBuffer;
        
        protected Vector2 m_vMaxPoint1;
        protected Vector2 m_vMaxPoint2;

        protected float m_fTransitionIn;
        protected float m_fTransitionOut;
        protected float m_fTransitionCounter;
        protected float m_fTransparency;
        protected bool m_IsTransitionButton;
        protected float m_fTransitionButton;
        protected float m_fTransitionButtonCounter;

        public Menu(Game game)
            : base(game)
        {
            m_lsState = Constants.Layer_state.NO_ACTION;
            m_Title = new MenuItem_Title(game);
            m_lItems = new List<MenuItem_Choice>();
            m_fTitleToChoiceOffset = 90.0f;
            m_fChoiceToChoiceOffset = 50.0f;
            m_fLeftPadding = 40.0f;
            m_fPushOffest = 30.0f;
            m_vMaxPoint1 = new Vector2();
            m_vMaxPoint2 = new Vector2();
            m_vPosition = new Vector2();

            m_fTransitionCounter = 0.0f;
            // Transition In
            m_fTransitionIn = 1.0f;

            // Transition Out
            m_fTransitionOut = 1.0f;

            // Transparency of Menu
            m_fTransparency = 1.0f;

            // Button/Selected Items Options
            m_fTransitionButtonCounter = 0.0f;
            m_fTransitionButton = 0.0f;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(string sTitle, List<MenuItem_Choice> lItems)
        {
            base.Initialize();
            Input.InputManager.HandleButton += new Input.InputManager.InputButtonHandler(HandleButton);

            m_lsState = Constants.Layer_state.TRANSITION_IN;
            m_IsTransitionButton = true;
            m_Title = new MenuItem_Title(Game, sTitle);
            m_lItems = new List<MenuItem_Choice>();
            if (lItems.Count > 0)
                lItems[0].IsSelected = true;
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

            if (m_IsTransitionButton)
            {
                // Get Selected MenuItem
                MenuItem_Choice selected = (MenuItem_Choice)GetSelectedMenuItem();
                // Draw line(s)
                m_fTransitionButtonCounter += fDeltaTime;
                if (m_fTransitionButtonCounter > m_fTransitionButton)
                {
                    m_fTransitionButtonCounter = 0.0f;
                    m_IsTransitionButton = false;
                    selected.IsValueChanged = true;
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
            DrawButtonTransition();
            // Calculate Transparency
            float transparency = 0.0f;
            if (m_lsState == Constants.Layer_state.TRANSITION_IN)
                transparency = (1 - (m_fTransitionIn - m_fTransitionCounter) / m_fTransitionIn) * m_fTransparency;
            else if(m_lsState == Constants.Layer_state.TRANSITION_OUT ||
                    m_lsState == Constants.Layer_state.INACTIVE)
                transparency = ((m_fTransitionOut - m_fTransitionCounter) / m_fTransitionOut) * m_fTransparency;
            else
                transparency = m_fTransparency;

            m_Title.Draw(gameTime, m_vPosition, transparency);
            for (int i = 0; i < m_lItems.Count; i++)
            {
                Vector2 offset = new Vector2(m_fLeftPadding, m_fTitleToChoiceOffset + m_fChoiceToChoiceOffset*i);
                if (m_lItems[i].IsSelected)
                    offset.X += m_fPushOffest;
                m_lItems[i].Draw(gameTime, offset + m_vPosition, transparency); 
            }
        }

        public new void UnloadContent()
        {
            m_Title.UnloadContent();
            Input.InputManager.HandleButton -= new Input.InputManager.InputButtonHandler(HandleButton);
            foreach (MenuItem_Choice choice in m_lItems)
            {
                choice.UnloadContent();
            }
        }

        #region Event Handling
        public override void HandleButton(int playerIndex, Buttons button, Constants.GamePad_ButtonState buttonState)
        {
            if (playerIndex == 0 && m_IsActive && buttonState.Equals(Constants.GamePad_ButtonState.JUST_PRESSED))
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
                    m_IsTransitionButton = true;
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
                    m_IsTransitionButton = true;
                }
                // Button Handling
                if (button.Equals(Buttons.A))
                {
                    for (int i = 0; i < m_lItems.Count; i++)
                    {
                        if (m_lItems[i].IsSelected)
                        {
                            if (!m_lItems[i].IsCurrentLayerRemaining)
                                m_lsState = Constants.Layer_state.TRANSITION_OUT;
                            if (m_lItems[i].NextLayer == null)
                                m_lsState = Constants.Layer_state.INACTIVE;
                            else if (m_lItems[i].NextLayer is EmptyLayer)
                            { 
                                //Do Nothing
                            }
                            else
                            {
                                LayerManager.Singleton.AddLayer(m_lItems[i].NextLayer);
                                m_IsActive = false;
                                
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Abstract Functions
        protected abstract void SetUpTransitions();
        protected abstract void SetUpOffsets();
        #endregion

        #region Misc. Functions
        private void DrawButtonTransition()
        {
            //DrawLines();
        }
        private void DrawLines()
        {
            Objects.Line.DrawLine(new Vector2(), new Vector2(100, 300), Color.White, Game);
        }
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
        protected float CalculateNextColumn()
        {
            float distanceToNextColumn = 0.0f;
            for (int i = 0; i < m_lItems.Count; i++)
            {
                float comparedValue = m_vPosition.X + m_fLeftPadding + m_fPushOffest + m_lItems[i].Length;
                if (comparedValue > distanceToNextColumn)
                    distanceToNextColumn = comparedValue;
            }
            return distanceToNextColumn;
        }

        public Vector2 GetSelectedMenuItemPosition()
        {
            Vector2 selected = new Vector2();
            for (int i = 0; i < m_lItems.Count; i++)
            {
                if (m_lItems[i].IsSelected)
                {
                    selected.X = CalculateNextColumn();
                    selected.Y = m_fTitleToChoiceOffset + m_fChoiceToChoiceOffset * i + m_vPosition.Y;
                    break;
                }
            }
            return selected;
        }
        #endregion

        #region Getters and Setters for member variables.
        public Vector2 Position
        {
            get { return m_vPosition; }
            set { m_vPosition = value; }
        }
        #endregion
    }
}