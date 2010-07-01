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


namespace Jazz.Objects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public static class Line
    {
        public static void DrawLine(Vector2 from, Vector2 to, Color color, Game game)
        {
            Matrix viewMatrix;
            Matrix projectionMatrix;
            BasicEffect basicEffect;
            VertexDeclaration vertexDeclaration;
            VertexPositionColor[] pointList;
            VertexBuffer vertexBuffer;
            int points = 2;
            short[] lineListIndices;

            viewMatrix = Matrix.CreateLookAt(
                new Vector3(0.0f, 0.0f, 1.0f),
                Vector3.Zero,
                Vector3.Up
                );
            projectionMatrix = Matrix.CreateOrthographicOffCenter(
                0,
                (float)game.GraphicsDevice.Viewport.Width,
                (float)game.GraphicsDevice.Viewport.Height,
                0,
                1.0f, 1000.0f);
            vertexDeclaration = new VertexDeclaration(
                game.GraphicsDevice,
                VertexPositionColor.VertexElements
                );
            basicEffect = new BasicEffect(game.GraphicsDevice, null);
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;
            pointList = new VertexPositionColor[2];
            pointList[0] = new VertexPositionColor(new Vector3(from, 0), color);
            pointList[1] = new VertexPositionColor(new Vector3(to, 0), color);
            // Initialize the vertex buffer, allocating memory for each vertex.
            vertexBuffer = new VertexBuffer(game.GraphicsDevice,
                VertexPositionColor.SizeInBytes * (pointList.Length),
                BufferUsage.None);

            // Set the vertex buffer data to the array of vertices.
            vertexBuffer.SetData<VertexPositionColor>(pointList);

            // Initialize an array of indices of type short.
            lineListIndices = new short[2];

            // Populate the array with references to indices in the vertex buffer
            lineListIndices[0] = 0;
            lineListIndices[1] = 1;

            // Draw
            //game.GraphicsDevice.Clear(Color);
            game.GraphicsDevice.VertexDeclaration = vertexDeclaration;

            // The effect is a compiled effect created and compiled elsewhere
            // in the application.
            basicEffect.Begin();

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList,
                    pointList,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    2,  // number of vertices in pointList
                    lineListIndices,  // the index buffer
                    0,  // first index element to read
                    1   // number of primitives to draw
                );
                pass.End();
            }
            basicEffect.End();
        }
    }
}