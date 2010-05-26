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
        protected ContentManager m_ContentManager; // A ContentManager that we use to load/unload our actor model
        protected string m_sMeshName;         // A string which stores the name of the mesh to be loaded.
        protected Matrix[] m_actorBones;      // A Matrix array which stores bone transforms for the mesh (don’t worry if you don’t understand its purpose right now).
        protected float m_fScale;              // Model Scale
        protected Vector3 m_vRotation_Timestep;
        protected Vector3 m_vRotation_Full;
        protected Quaternion m_qRotationOffset;
        //public BoundingSphere WorldBounds;
        //public BoundingSphere ModelBounds;

        // Physics
        protected Vector3 m_vPosition;
        protected Vector3 m_vVelocity;
        protected Vector3 m_vFacing; 
        protected float m_fMass;

        // Camera
        protected Camera.FirstPersonCamera m_firstPersonCamera;
        protected Camera.ThirdPersonCamera m_thirdPersonCamera;

        // Player States
        protected bool m_isActive = false;
        protected Constants.Movement_state m_aMovementState;
        protected Constants.Weapon_state m_wWeaponState;

        // GamePad Controls
        protected Dictionary<Buttons, Associated_Actions> m_controls;
        protected bool m_isDefaultSticks;
        protected float m_fSensitivity_Aim;         // Range from 0-1
        protected float m_fSensitivity_Move;        // Range from 0-2
        protected bool m_isAcceleration_Aim;        // Check is Acceleration on aim is on [Like mouse acceleration]

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
                get { return m_GamePadButtonState; }
                set { m_GamePadButtonState = value; }
            }
            public Constants.Action_state Action_State
            {
                get { return m_ActionState; }
                set { m_ActionState = value; }
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
            m_firstPersonCamera = new Camera.FirstPersonCamera(game, iPlayerIndex);
            m_thirdPersonCamera = new Camera.ThirdPersonCamera(game);
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // Stats
            m_fHitPoints = 100.0f;
            m_fArmor = 0.0f;
            // Model
            m_mWorldTransform = Matrix.Identity;
            m_fScale = 1.0f;
            m_vRotation_Timestep = new Vector3();
            m_vRotation_Full = new Vector3();
            m_sMeshName = Constants.PLAYER_MODEL_1;
            // Physics
            m_vFacing = new Vector3(0.0f, 0.0f, -1.0f);
            m_vPosition = new Vector3();
            m_vVelocity = new Vector3();
            m_fMass = 0.0f;
            // Camera
            m_firstPersonCamera.Initialize();
            m_thirdPersonCamera.Initialize();
            // Player States
            m_isActive = false;
            m_aMovementState = Constants.Movement_state.NO_ACTION;
            m_wWeaponState = Constants.Weapon_state.NO_ACTION;
            // GamePad Controls
            m_controls = new Dictionary<Buttons, Associated_Actions>();
            m_isDefaultSticks = true;
            m_fSensitivity_Aim = .5f; 
            m_fSensitivity_Move = 1.0f;
            m_isAcceleration_Aim = false;
            LoadContent();

            base.Initialize();
        }
        #endregion

        #region Content
        /// <summary>
        ///      
        /// </summary>
        protected override void LoadContent()
        {
            m_ContentManager = new ContentManager(Game.Services, "Content");
            m_actorModel = m_ContentManager.Load<Model>(m_sMeshName);
            m_actorBones = new Matrix[m_actorModel.Bones.Count];

            base.LoadContent();
        }

        /// <summary>
        ///      
        /// </summary>
        protected override void UnloadContent()
        {
            m_ContentManager.Unload();

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
            // Update 1st-Person Camera
            m_firstPersonCamera.Update(gameTime, m_vRotation_Full, m_vPosition);

            UpdateModel();              // Update Model Transformations
            UpdateMovement(gameTime);   // Update Movement
            UpdateAim(gameTime);        // Update Aim
            
            base.Update(gameTime);
        }

        public void UpdateModel()
        {
            // Model -> World Transforms
            m_mWorldTransform = Matrix.CreateScale(m_fScale);                   // scale
            m_mWorldTransform *= Matrix.CreateRotationY(MathHelper.ToRadians(m_vRotation_Full.Y));         // rotate
            m_mWorldTransform *= Matrix.CreateTranslation(m_vPosition);         // transform
        }
        public void UpdateMovement(GameTime gameTime)
        {
            // Move Player
            float fDeltaTime = (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerSecond;
            //m_vPosition += Vector3.Multiply(m_vVelocity, fDeltaTime)-1/2at^2;
            m_vPosition += Vector3.Multiply(m_vVelocity * Constants.MAX_MSPEED, fDeltaTime);
        }
        public void UpdateAim(GameTime gameTime)
        {
            float fDeltaTime = (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerSecond;
            m_vRotation_Full += m_vRotation_Timestep * m_fSensitivity_Aim * (540.0f * fDeltaTime);
            m_vRotation_Full.X = MathHelper.Clamp(m_vRotation_Full.X,
                                                 Constants.PITCH_MIN,
                                                 Constants.PITCH_MAX);
        }
        public void UpdateCollisions(GameTime gameTime) { }
        public void UpdateAnimations(GameTime gameTime) { }

        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            GraphicsDevice.RenderState.DepthBufferEnable = true;

            // Copy any parent transforms.
            m_actorModel.CopyAbsoluteBoneTransformsTo(m_actorBones);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in m_actorModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = m_actorBones[mesh.ParentBone.Index] * m_mWorldTransform;
                    effect.View = view;
                    effect.Projection = projection;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
            Draw(gameTime);
        }
        #endregion

        #region Actions & Event Handlers

        #region GamePad Event Handlers

        public void HandleButton(Buttons button, Constants.GamePad_ButtonState buttonState)
        {
            if (m_controls.ContainsKey(button) && m_controls[button].Button_State.Equals(buttonState))
                CaculateGameControls(m_controls[button].Action_State);
        }

        public void HandleThumbstick(Constants.GamePad_ThumbSticks thumbstickType, Vector2 thumbstick)
        {
            if (((thumbstickType.Equals(Constants.GamePad_ThumbSticks.LEFT)) && m_isDefaultSticks) ||
                 ((thumbstickType.Equals(Constants.GamePad_ThumbSticks.RIGHT)) && !m_isDefaultSticks))
                CalculateVelocityVector(thumbstick);
            else if (((thumbstickType.Equals(Constants.GamePad_ThumbSticks.RIGHT)) && m_isDefaultSticks) ||
                     ((thumbstickType.Equals(Constants.GamePad_ThumbSticks.LEFT)) && !m_isDefaultSticks))
                CalculateRotationVector(thumbstick);
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

        private void CalculateRotationVector(Vector2 thumbstick)
        {
            m_vRotation_Timestep.X = thumbstick.Y;
            m_vRotation_Timestep.Y = -1 * thumbstick.X;
        }

        /// <summary>
        /// </summary>
        private void CalculateVelocityVector(Vector2 thumbstick)
        {
            if (thumbstick != Vector2.Zero)
            {
                Vector2 tempVector = thumbstick;
                tempVector.Normalize();
                double angle = Math.Atan2(tempVector.Y, tempVector.X) + Math.Atan2(-1.0, 0.0);
                if (angle < 0)
                    angle += Math.PI * 2;
                Vector3 forward = m_firstPersonCamera.Forward;
                forward.Y = 0;
                forward.Normalize();
                m_vVelocity = Vector3.Transform(forward, Quaternion.CreateFromAxisAngle(Vector3.Up, (float)angle));
                m_vVelocity *= thumbstick.Length();
                m_aMovementState = Constants.Movement_state.JOG;
                //Console.WriteLine((angle * 180 / Math.PI).ToString());
            }
            else
            {
                m_vVelocity = new Vector3();
                m_aMovementState = Constants.Movement_state.NO_ACTION;
            }
            //m_vVelocity = Vector3.Zero;
            //m_vVelocity.X = thumbstick.X;
            //m_vVelocity.Z = -1 * thumbstick.Y;
            //m_vVelocity *= Constants.MAX_MSPEED;
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
        // Stats
        public float HitPoints
        {
            get { return m_fHitPoints; }
            set { m_fHitPoints = value; }
        }
        public float Armor
        {
            get { return m_fArmor; }
            set { m_fArmor = value; }
        }
        // Model
        public float Scale
        {
            get { return m_fScale; }
            set { m_fScale = value; }
        }
        // Physics
        public Vector3 Facing
        {
            get { return m_vFacing; }
            set { m_vFacing = value; }
        }
        public Vector3 Position
        {
            get { return m_vPosition; }
            set { m_vPosition = value; }
        }
        public Vector3 Velocity
        {
            get { return m_vVelocity; }
            set { m_vVelocity = value; }
        }
        public float Mass
        {
            get { return m_fMass; }
            set { m_fMass = value; }
        }
        // Camera
        public Camera.FirstPersonCamera FirstPersonCamera
        {
            get { return m_firstPersonCamera; }
            set { m_firstPersonCamera = value; }
        }
        // Player States
        public bool IsActive
        {
            get { return m_isActive; }
            set { m_isActive = value; }
        }
        // GamePad Controls
        public float Sensivity_Aim
        {
            get { return m_fSensitivity_Aim; }
            set { m_fSensitivity_Aim = value; }
        }
        public float Sensivity_Move
        {
            get { return m_fSensitivity_Move; }
            set { m_fSensitivity_Move = value; }
        }
        public bool Acceleration_Aim
        {
            get { return m_isAcceleration_Aim; }
            set { m_isAcceleration_Aim = value; }
        }
         #endregion
    }
}