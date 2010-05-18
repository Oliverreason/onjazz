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


namespace Jazz.Camera
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FirstPersonCamera : Microsoft.Xna.Framework.GameComponent
    {
        #region Member Variables
        protected Vector3 m_vAvatarHeadOffset;
        protected Vector3 m_vPosition;
        protected Vector3 m_vForward;
        protected Matrix m_mView;
        protected Matrix m_mProjection;
        protected Viewport m_vtViewPort;
        protected float m_fAspectRatio;
        protected float m_fFieldOfView;         // In degrees

        #endregion

        public FirstPersonCamera(Game game)
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
            m_vAvatarHeadOffset = new Vector3(0, 0, -15);
            m_vPosition = new Vector3();
            m_vForward = new Vector3(0, 0, -1);
            m_mView = Matrix.Identity;
            m_mProjection = Matrix.Identity;
            m_vtViewPort = Game.GraphicsDevice.Viewport;
            m_fAspectRatio = (float)m_vtViewPort.Width / (float)m_vtViewPort.Height;
            m_fFieldOfView = Constants.FOV_DEFAULT;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public void Update(GameTime gameTime, float avatarYaw, Vector3 position, int playerIndex)
        {
            // Keep Viewport updated
            UpdateViewPort(playerIndex);
         
            Matrix rotationMatrix = Matrix.CreateRotationY(avatarYaw);

            Vector3 transformedheadOffset = Vector3.Transform(m_vAvatarHeadOffset, rotationMatrix);

            Vector3 cameraPosition = position + transformedheadOffset;

            //Calculate the camera's view and projection 
            //matrices based on current values.
            m_mView = Matrix.CreateLookAt(cameraPosition,
                                          m_vForward,
                                          Vector3.Up);

            m_mProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(m_fFieldOfView),
                                                                m_fAspectRatio,
                                                                Constants.NEAR_CLIPPING_PLANE,
                                                                Constants.FAR_CLIPPING_PLANE);
            Update(gameTime); 
        }

        private void UpdateViewPort(int playerIndex)
        {
            int numPlayersActive = GameManager.m_iNumPlayersActive;
            switch(playerIndex){
                case 0:
                    m_vtViewPort.X = 0;
                    m_vtViewPort.Y = 0;
                    m_vtViewPort.Width = Constants.PREFERRED_WIDTH / ((int)(numPlayersActive+1) / 2);
                    m_vtViewPort.Height = Constants.PREFERRED_HEIGHT / ((int)(numPlayersActive + 4) / 3);
                    break;
                case 1:
                    if (numPlayersActive>2){// 3-4 players
                        m_vtViewPort.X = Constants.PREFERRED_WIDTH / 2;
                        m_vtViewPort.Y = 0;
                        m_vtViewPort.Width = Constants.PREFERRED_WIDTH / 2;
                        m_vtViewPort.Height = Constants.PREFERRED_HEIGHT / 2;
                    }
                    else{ // 2 players
                        m_vtViewPort.X = 0;
                        m_vtViewPort.Y = Constants.PREFERRED_HEIGHT / 2;
                        m_vtViewPort.Width = Constants.PREFERRED_WIDTH;
                        m_vtViewPort.Height = Constants.PREFERRED_HEIGHT / 2;
                    }
                    break;
                case 2:
                    m_vtViewPort.X = 0;
                    m_vtViewPort.Y = Constants.PREFERRED_HEIGHT / 2;
                    m_vtViewPort.Width = Constants.PREFERRED_WIDTH / 2;
                    m_vtViewPort.Height = Constants.PREFERRED_HEIGHT / 2;
                    break;
                case 3:
                    m_vtViewPort.X = Constants.PREFERRED_WIDTH / 2;
                    m_vtViewPort.Y = Constants.PREFERRED_HEIGHT / 2;
                    m_vtViewPort.Width = Constants.PREFERRED_WIDTH / 2;
                    m_vtViewPort.Height = Constants.PREFERRED_HEIGHT / 2;
                    break;
                default:
                    m_vtViewPort.X = 0;
                    m_vtViewPort.Y = 0;
                    m_vtViewPort.Width = Constants.PREFERRED_WIDTH;
                    m_vtViewPort.Height = Constants.PREFERRED_HEIGHT;
                    break;
            }

            m_fAspectRatio = (float)m_vtViewPort.Width / (float)m_vtViewPort.Height;
        }

        #region Getters and Setters for member variables.
        public Vector3 HeadOffset
        {
            get { return m_vAvatarHeadOffset; }
            set { m_vAvatarHeadOffset = value; }
        }
        public Vector3 Forward
        {
            get { return m_vForward; }
            set { m_vForward = value; }
        }
        public Matrix ViewMatrix
        {
            get { return m_mView; }
            set { m_mView = value; }
        }
        public Matrix ProjectionMatrix
        {
            get { return m_mProjection; }
            set { m_mProjection = value; }
        }
        public Viewport TheViewPort
        {
            get { return m_vtViewPort; }
            set { m_vtViewPort = value; }
        }
        public float FieldOfView
        {
            get { return m_fFieldOfView; }
            set { m_fFieldOfView = MathHelper.Clamp(value, Constants.FOV_MIN, Constants.FOV_MAX); }
        }
        #endregion
    }
}