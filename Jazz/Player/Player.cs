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
        public Quaternion m_qRotation;         // Model Rotations
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
        protected float m_fSensitivity_Aim;         // Range from 0-2
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
            m_firstPersonCamera = new Camera.FirstPersonCamera(game);
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
            m_qRotation = Quaternion.Identity;
            m_sMeshName = Constants.PLAYER_MODEL_1;
            // Physics
            m_vFacing = new Vector3(0.0f, 0.0f, 1.0f);
            m_vPosition = new Vector3();
            m_vVelocity = new Vector3();
            m_fMass = 0.0f;
            // Camera
            m_firstPersonCamera.Initialize();
            m_thirdPersonCamera.Initialize();
            // Player States
            m_isActive = true;
            m_aMovementState = Constants.Movement_state.NO_ACTION;
            m_wWeaponState = Constants.Weapon_state.NO_ACTION;
            // GamePad Controls
            m_controls = new Dictionary<Buttons, Associated_Actions>();
            m_isDefaultSticks = true;
            m_fSensitivity_Aim = 1.0f; 
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
            //Console.WriteLine(m_playerIndex.ToString() + " has run its update.");
            // Update 1st-Person Camera
            m_firstPersonCamera.Update(gameTime, 0.0f, m_vPosition, m_playerIndex);
            // Model -> World Transforms
            m_mWorldTransform = Matrix.CreateScale(m_fScale);                   // scale
            m_mWorldTransform *= Matrix.CreateFromQuaternion(m_qRotation);      // rotate
            m_mWorldTransform *= Matrix.CreateTranslation(m_vPosition);         // transform

            // Update Movement
            UpdateMovement(gameTime);

            base.Update(gameTime);
        }

        public void UpdateMovement(GameTime gameTime)
        {
            // Move Player
            float fDeltaTime = (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerSecond;
            m_vPosition += Vector3.Multiply(m_vVelocity, fDeltaTime);
            
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
            //Console.WriteLine(button.ToString() + " is " + buttonState.ToString() + ".");
            if (m_controls.ContainsKey(button) && m_controls[button].Button_State.Equals(buttonState))
            {
                CaculateGameControls(m_controls[button].Action_State);
            }
        }

        public void HandleStick(GameTime gameTime, Constants.Thumbstick_selection thumbstickSelection)
        {
            //if (thumbstickSelection == Constants.Thumbstick_selection.LEFT)
            //{
            //    Console.WriteLine("Left thumbstick moved.");
            //    if (m_isDefaultSticks)
            //        CalculateVelocityVector(gameTime);
            //    else
            //        CalculateFrontVector(gameTime);
            //}
            //else  // right thumbstick
            //{
            //    Console.WriteLine("Right thumbstick moved.");
            //    if (m_isDefaultSticks)
            //        CalculateFrontVector(gameTime);
            //    else
            //        CalculateVelocityVector(gameTime);
            //}

            CalculateVelocityVector(gameTime);
            CalculateFrontVector(gameTime);
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

        private void CalculateFrontVector(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Convert thumbstick vector to a 3d vector.
        /// Find angle between front vector and thumbstick vector.
        /// Rotate forward facing vector around the y-axis using angle found above.
        /// Get percentage of stick movement.
        /// Find new velocity.
        /// By percentage of stick movement as the scale factor and applying a spring to the velocity.
        /// </summary>
        private void CalculateVelocityVector(GameTime gameTime)
        {
            //Vector3 vGoal = new Vector3();
            Vector2 vStickOffset = new Vector2();
            //float fPercent;

            //if (m_isDefaultSticks)
            //    vStickOffset = GamePad.GetState((PlayerIndex)m_playerIndex).ThumbSticks.Right;
            //else
            //    vStickOffset = GamePad.GetState((PlayerIndex)m_playerIndex).ThumbSticks.Left;
            //vGoal.X = vStickOffset.X;
            //vGoal.Z = -1*vStickOffset.Y;

            ////Console.WriteLine(vGoal.X.ToString() + " // " + vGoal.Z.ToString());
            //double radians = Math.Acos(Vector3.Dot(Vector3.Forward, vGoal));
            //vGoal = Objects.Math.Vector3MultiplyMatrix(m_vFacing, Matrix.CreateRotationY(MathHelper.ToRadians((float)radians)));
            //fPercent = vStickOffset.Length() / 1;
            //vGoal.Normalize();
            //vGoal *= Constants.MAX_MSPEED * fPercent;
            //float fDeltaTime = (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerSecond;
            //m_vVelocity = Objects.Spring.Interpolate(m_vVelocity, vGoal, fDeltaTime, 2.0f * m_fSensitivity_Move + Constants.MOVEMENT_DAMP);
            //m_vVelocity = vGoal;
            if (m_isDefaultSticks)
                vStickOffset = GamePad.GetState((PlayerIndex)m_playerIndex).ThumbSticks.Right;
            else
                vStickOffset = GamePad.GetState((PlayerIndex)m_playerIndex).ThumbSticks.Left;
            m_vVelocity = Vector3.Zero;
            m_vVelocity.X = vStickOffset.X;
            m_vVelocity.Z = -1 * vStickOffset.Y;
            m_vVelocity *= Constants.MAX_MSPEED ;

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
        public Quaternion Rotation
        {
            get { return m_qRotation; }
            set { m_qRotation = value; }
        }
        // Physics
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