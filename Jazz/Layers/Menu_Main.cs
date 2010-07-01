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
    /// 
    /// </summary>
    public class Menu_Main : Menu
    {
        #region Member Variables
        protected float m_offsetTopPercent;
        protected float m_offsetLeftPercent;
        #endregion

        public Menu_Main(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// 
        /// DO NOT CHANGE ORDER OF "Iniatilaize Base Class" AND "Set-Up Position".
        /// </summary>
        public override void Initialize()
        {
            // Set-up Menu
            List<MenuItem_Choice> lItems = new List<MenuItem_Choice>();
            lItems.Add(new MenuItem_Choice(Game, "Single-player", new SubMenu_Multi(Game, this), true));
            lItems.Add(new MenuItem_Choice(Game, "Multi-player", new SubMenu_Multi(Game, this), true));
            lItems.Add(new MenuItem_Choice(Game, "Options", new SubMenu_Multi(Game, this), true));
            lItems.Add(new MenuItem_Choice(Game, "Credits", new EmptyLayer(Game), true));
            lItems.Add(new MenuItem_Choice(Game, "Exit", null, false));
            // Set-up Position
            m_offsetTopPercent = 0.15f;
            m_offsetLeftPercent = 0.05f;

            // Iniatilaize Base Class
            base.Initialize("Main", lItems);

            // Set-Up Position
            m_vPosition = new Vector2((float)GraphicsDevice.Viewport.Width * m_offsetLeftPercent,
                                      (float)GraphicsDevice.Viewport.Height * m_offsetTopPercent);            
        }

        protected override void SetUpTransitions()
        {
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
        protected override void SetUpOffsets()
        {
            m_fTitleToChoiceOffset = 90.0f;
            m_fChoiceToChoiceOffset = 50.0f;
            m_fLeftPadding = 40.0f;
            m_fPushOffest = 30.0f;
        }
    }
}