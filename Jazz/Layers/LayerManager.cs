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
    public sealed class LayerManager
    {
        private static Game m_game;
        private static List<Layer> m_lActiveLayers;
        private static volatile LayerManager instance;
        private static object syncRoot = new Object();
        private List<Layer> m_lNewLayers;
        private List<Layer> m_lOldLayers;

        private LayerManager() { }

        public static LayerManager Singleton
        {
          get 
          {
             if (instance == null) 
             {
                lock (syncRoot) 
                {
                   if (instance == null)
                       instance = new LayerManager();
                }
             }

             return instance;
          }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Game game)
        {
            m_game = game;
            m_lActiveLayers = new List<Layer>();
            m_lNewLayers = new List<Layer>();
            m_lOldLayers = new List<Layer>();
            Menu_Main mainMenu = new Menu_Main(m_game);
            mainMenu.Initialize();
            m_lActiveLayers.Add(mainMenu);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            AddLayers();
            RemoveLayers();
            foreach (Layer layer in m_lActiveLayers)
            {
                layer.Update(gameTime);
            }
            RemoveLayers();
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Layer layer in m_lActiveLayers)
            {
                layer.Draw(gameTime);
            }
        }

        public void AddLayer(Layer layer_add)
        {
            m_lNewLayers.Add(layer_add);
        }

        public void RemoveLayer(Layer layer_remove)
        {
            m_lOldLayers.Add(layer_remove);
        }

        private void AddLayers()
        {
            if (m_lActiveLayers.Count > 0)
            {
                foreach (Layer layer in m_lNewLayers)
                {
                    layer.Initialize();
                    m_lActiveLayers.Add(layer);
                }
                m_lNewLayers.Clear();
            }
        }

        private void RemoveLayers()
        {
            if (m_lOldLayers.Count > 0)
            {
                foreach (Layer layer in m_lOldLayers)
                {
                    layer.UnloadContent();
                    m_lActiveLayers.Remove(layer);
                }
                m_lOldLayers.Clear();
            }
        }

        public Game Game
        {
            get { return m_game; }
            set { m_game = value; }
        }
    }
}