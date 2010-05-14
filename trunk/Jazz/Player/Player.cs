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

    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        private enum movement_state:byte { NO_ACTION, WALK, CROUTCH, SPRINT, JOG };
        private enum weapon_state:byte { NO_ACTION, GUN_READY, GUN_SHOOT, RELOAD, WEAPON_SWITCH };
        
        // Member variables
        // Stats
        protected float m_fHitPoints;
        protected float m_fArmor;

        // Physics
        protected Vector3 m_vPosition;
        protected Vector3 m_vVelocity;
        protected float m_fMass;

        // Camera
        protected Jazz.Camera.FirstPersonCamera m_fpCamera;

        // Player States
        //protected movement_state m_aMovementState;
        //protected weapon_state m_wWeaponState;

        public Player(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);

        }

        public void updateMovement() { }
        public void updateCollisions() { }


        // Actions & Event Handlers
        // Movement
        public void doCroutch() { 
            // update camera
            // update skeleton and hitboxes of mesh
        }
        public void doWalk() { }
        public void doSprint() { }
        public void doJog() { }

        // Getters & Setters
    }
}