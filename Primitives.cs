using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Nano
{
    /// <summary>
    /// Line Batch
    /// For drawing lines in a spritebatch
    /// </summary>
    static public class Primitives
    {
        static private Texture2D _empty_texture;
        static private bool      _set_data = false;

        static public void Init(ContentManager content)
        {
            _empty_texture = content.Load<Texture2D>("pixel");
        }

        static public void DrawLine(SpriteBatch batch, Color color,
                                    Vector2 point1, Vector2 point2)
        {

            DrawLine(batch, color, point1, point2, 0);
        }

        static public void DrawRect(SpriteBatch batch, Color color,
                                    Rectangle rect)
        {
            Vector2 point1 = new Vector2(rect.Left, rect.Top);
            Vector2 point2 = new Vector2(rect.Right, rect.Top);
            Vector2 point3 = new Vector2(rect.Right, rect.Bottom);
            Vector2 point4 = new Vector2(rect.Left, rect.Bottom);

            DrawLine(batch, color, point1, point2, 0);
            DrawLine(batch, color, point2, point3, 0);
            DrawLine(batch, color, point3, point4, 0);
            DrawLine(batch, color, point4, point1, 0);
        }


        /// <summary>
        /// Draw a line into a SpriteBatch
        /// </summary>
        /// <param name="batch">SpriteBatch to draw line</param>
        /// <param name="color">The line color</param>
        /// <param name="point1">Start Point</param>
        /// <param name="point2">End Point</param>
        /// <param name="Layer">Layer or Z position</param>
        static public void DrawLine(SpriteBatch batch, Color color, Vector2 point1,
                                    Vector2 point2, float Layer)
        {
            //Check if data has been set for texture
            //Do this only once otherwise
            if (!_set_data)
            {
                _empty_texture.SetData(new[] { Color.White });
                _set_data = true;
            }


            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = (point2 - point1).Length();

            batch.Draw(_empty_texture, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, 1),
                       SpriteEffects.None, Layer);
        }

        static public void DrawFillRect(SpriteBatch batch, Color color, Vector2 point1,
                                    Vector2 point2)
        {
            //Check if data has been set for texture
            //Do this only once otherwise
            if (!_set_data)
            {
                _empty_texture.SetData(new[] { Color.White });
                _set_data = true;
            }

            Vector2 length = (point2 - point1);

            batch.Draw(_empty_texture, point1, null, color,
                       0, Vector2.Zero, new Vector2(length.X, length.Y),
                       SpriteEffects.None, 0);
        }

        static public void DrawFillRect(SpriteBatch batch, Color color, Rectangle rect)
        {
            //Check if data has been set for texture
            //Do this only once otherwise
            if (!_set_data)
            {
                _empty_texture.SetData(new[] { Color.White });
                _set_data = true;
            }

            batch.Draw(_empty_texture, new Vector2(rect.Left, rect.Top), null, color,
                       0, Vector2.Zero, new Vector2(rect.Width, rect.Height),
                       SpriteEffects.None, 0);
        }
    }
}
