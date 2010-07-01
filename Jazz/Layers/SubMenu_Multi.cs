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
    public class SubMenu_Multi : SubMenu
    {

        public SubMenu_Multi(Game game)
            : base(game)
        {
        }
        public SubMenu_Multi(Game game, Menu previous)
            : base(game, previous)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // Set-up Menu
            List<MenuItem_Choice> lItems = new List<MenuItem_Choice>();
            lItems.Add(new MenuItem_Choice(Game, "Online", new EmptyLayer(Game), true));
            lItems.Add(new MenuItem_Choice(Game, "LAN", new EmptyLayer(Game), true));

            // Iniatilaize Base Class
            base.Initialize(lItems);
        }

        protected override void SetUpTransitions()
        {
            // Transition In
            m_fTransitionIn = .2f;

            // Transition Out
            m_fTransitionOut = 0.0f;

            // Transparency of Menu
            m_fTransparency = .5f;
        }
        protected override void SetUpOffsets()
        {
            m_fChoiceToChoiceOffset = 50.0f;
            m_fLeftPadding = 40.0f;
            m_fPushOffest = 30.0f;
        }
    }
}