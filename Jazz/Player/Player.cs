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

    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Member variables
        // Stats
        protected int m_playerIndex;
        protected int m_playerId;
        protected float m_fHitPoints;
        protected float m_fArmor;

        // Model
        protected Model m_actorModel;         // A Model class which is all the mesh data that will be loaded.
        public Matrix m_mWorldTransform;       // A Matrix which will keep track of our world transform. This should be a public property.
        protected ContentManager m_content;   // A ContentManager that we use to load/unload our actor model
        protected string m_sMeshName;         // A string which stores the name of the mesh to be loaded.
        protected Matrix[] m_actorBones;      // A Matrix array which stores bone transforms for the mesh (don’t worry if you don’t understand its purpose right now).
        protected float m_fScale;              // Model Scale
        public Quaternion m_qRotation;         // Model Rotations
        //public BoundingSphere WorldBounds;
        //public BoundingSphere ModelBounds;

        // Physics
        protected Vector3 m_vPosition;
        protected Vector3 m_vVelocity;
        protected float m_fMass;

        // Camera
        protected Camera.FirstPersonCamera m_fpCamera;

        // Player States
        protected Constants.Movement_state m_aMovementState;
        protected Constants.Weapon_state m_wWeaponState;

        // GamePad Controls
        protected Dictionary<Buttons, Associated_Actions> m_controls;
        protected bool m_isDefaultSticks;

        // Helper class for GamePad Controls
        public class Associated_Actions
        {
            private Constants.Action_state m_ActionState;
            private Constants.GamePad_ButtonState m_GamePadButtonState;
            public Associated_Actions(Constants.Action_state actionState, Constants.GamePad_ButtonState buttonState)
            {
                m_ActionState = actionState;
                m_GamePadButtonState = buttonState;
            }
            // Setters and Getters
            public Constants.GamePad_ButtonState Button_State
            {
                get
                {
                    return m_GamePadButtonState;
                }
                set
                {
                    m_GamePadButtonState = value;
                }
            }
            public Constants.Action_state Action_State
            {
                get
                {
                    return m_ActionState;
                }
                set
                {
                    m_ActionState = value;
                }
            }
        }
        #endregion

        #region Constructors
        public Player(Game game)
            : base(game)
        {
            // This constructor should never be called directly.
        }
        public Player(Game game, int iPlayerIndex)
            : base(game)
        {
            m_playerIndex = iPlayerIndex;
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            // Stats
            m_fHitPoints = 100.0f;
            m_fArmor = 0.0f;
            // Model
            m_mWorldTransform = Matrix.Identity;
            m_fScale = 1.0f;
            m_qRotation = Quaternion.Identity;
            // Physics
            m_vPosition = new Vector3();
            m_vVelocity = new Vector3();
            m_fMass = 0.0f;
            // Player States
            m_aMovementState = Constants.Movement_state.NO_ACTION;
            m_wWeaponState = Constants.Weapon_state.NO_ACTION;
            // GamePad Controls
            m_controls = new Dictionary<Buttons, Associated_Actions>();
            m_isDefaultSticks = true;
        }
        #endregion

        #region Content
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
        #endregion

        #region Update
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
            Console.WriteLine(m_playerIndex.ToString() + " has run its update.");

        }

        public void UpdateMovement() { }
        public void UpdateCollisions() { }

        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        #endregion

        #region Actions & Event Handlers

        #region GamePad Event Handlers

        public void HandleButton(Buttons button, Constants.GamePad_ButtonState buttonState)
        {
            Console.WriteLine(button.ToString() + " is " + buttonState.ToString() + ".");
            if (m_controls.ContainsKey(button) && m_controls[button].Button_State.Equals(buttonState))
            {
                CaculateGameControls(m_controls[button].Action_State);
            }
        }
                
        public void HandleStick(Constants.Thumbstick_selection thumbstickSelection)
        {
            if (thumbstickSelection == Constants.Thumbstick_selection.LEFT)
            {
                Console.WriteLine("Left thumbstick moved.");
                if (m_isDefaultSticks)
                    DoMove();
                else
                    DoAim();
            }
            else  // right thumbstick
            {
                Console.WriteLine("Right thumbstick moved.");
                if (m_isDefaultSticks)
                    DoAim();
                else
                    DoMove();
            }
        }

        private void CaculateGameControls(Constants.Action_state actionState)
        {
            switch (actionState)
            {
                case Constants.Action_state.ALT_SHOOT:
                    break;
                case Constants.Action_state.ASSASSINATE:
                    break;
                case Constants.Action_state.CROUTCH:
                    break;
                case Constants.Action_state.JUMP:
                    break;
                case Constants.Action_state.SHOOT:
                    break;
                case Constants.Action_state.SPRINT:
                    break;
                case Constants.Action_state.SWITCH_NADE:
                    break;
                case Constants.Action_state.SWITCH_WEAPON:
                    break;
                case Constants.Action_state.THROW_NADE:
                    break;
                case Constants.Action_state.USE:
                    break;
                case Constants.Action_state.WALK:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Movement
        public void DoMove()
        { }
        public void DoAim()
        { }
        public void DoCroutch()
        {
            // update camera
            // update skeleton and hitboxes of mesh
        }
        public void DoWalk() { }
        public void DoSprint() { }
        public void DoJog() { }
        #endregion
        #endregion

        #region Getters and Setters for member variables.
        public float HitPoints
        {
            get
            {
                return m_fHitPoints;
            }
            set
            {
                m_fHitPoints = value;
            }
        }
        public float Armor
        {
            get
            {
                return m_fArmor;
            }
            set
            {
                m_fArmor = value;
            }
        }
        public Vector3 Position
        {
            get
            {
                return m_vPosition;
            }
            set
            {
                m_vPosition = value;
            }
        }
        public Vector3 Velocity
        {
            get
            {
                return m_vVelocity;
            }
            set
            {
                m_vVelocity = value;
            }
        }
        public float Mass
        {
            get
            {
                return m_fMass;
            }
            set
            {
                m_fMass = value;
            }
        }
         #endregion
    }
}