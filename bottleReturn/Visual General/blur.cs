using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace bottleReturn
{
    public class blur
    {
            // Quadrant 1, 2, 3 and 4
        Rectangle blurRectq1;
        Rectangle blurRectq2;
        Rectangle blurRectq3;
        Rectangle blurRectq4;

        Texture2D blurTexture;
        
            //Quadrant 1, 2, 3, and 4 for 2nd Blur Image
        Rectangle blurRect2q1;
        Rectangle blurRect2q2;
        Rectangle blurRect2q3;
        Rectangle blurRect2q4;

        public Vector2 BlurRect2q4
        {
            get { return new Vector2(blurRect2q4.X, blurRect2q4.Y); }
            set
            {
                blurRect2q4.X = (int)value.X;
                blurRect2q4.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// Creates a new Blur Object
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="initialPos"></param>
        /// <param name="device"></param>
        /// <param name="radial"></param>
        /// <param name="fuzz"></param>
        public blur(int width, int height, Vector2 initialPos, GraphicsDevice device, float radial, float fuzz)
        {
            // Standard math quadrant stuff.
            #region Quadrant mirroring
            blurRectq1.X = width;
            blurRectq1.Y = (int)initialPos.Y;
            blurRectq1.Width = width;
            blurRectq1.Height = height;

            blurRectq2.X = (int)initialPos.X;
            blurRectq2.Y = (int)initialPos.Y;
            blurRectq2.Width = width;
            blurRectq2.Height = height;

            blurRectq3.X = (int)initialPos.X;
            blurRectq3.Y = height;
            blurRectq3.Width = width;
            blurRectq3.Height = height;

            blurRectq4.X = width;
            blurRectq4.Y = height;
            blurRectq4.Width = width;
            blurRectq4.Height = height;

            blurRect2q1 = blurRectq1;
            blurRect2q2 = blurRectq2;
            blurRect2q3 = blurRectq3;
            blurRect2q4 = blurRectq4;

            #endregion

            blurTexture = new Texture2D(device, width, height, false, SurfaceFormat.Color);
            UInt32[] toSet = new UInt32[width * height];
            float squaredRadius = (float)Math.Pow(200 + (200 * (1 - radial)), 2);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < (height); y++)
                {
                    float distance = (float)(Math.Pow((x - width), 2) + Math.Pow((y - height), 2));
                    toSet[y * width + x] = 0xBF000000;

                    if (distance <= squaredRadius)
                    {
                        float lerpVal = (squaredRadius - distance) / squaredRadius;
                        // Lower transparency for fuzz factor
                        lerpVal = MathHelper.Clamp(lerpVal, 0.25f, 0.90f);

                        toSet[y * width + x] = (uint)((1 - lerpVal) * 255);

                        //toSet[y * width + x] <<= 24;
                        for (int fz = 0; fz < 3; fz++)
                        {
                            // Here is the magic bitshift number:
                            toSet[y * width + x] <<= 8;
                            toSet[y * width + x] += (uint)(fuzz * 0.5 * 255);
                        }
                    }
                    //if (distance <= squaredRadius / 2)
                    //{
                    //    float lerpVal = (squaredRadius - distance) / squaredRadius;
                    //    lerpVal = MathHelper.Clamp(lerpVal, 0.25f, 1.00f);

                    //    toSet[y * width + x] = (uint)((1 - lerpVal) * 255);
                    //    for (int fz = 0; fz < 3; fz++)
                    //    {
                    //        // Here is the magic bitshift number:
                    //        toSet[y * width + x] <<= 8;
                    //        toSet[y * width + x] += (uint)(fuzz * 0.5 * 255);
                    //    }
                    //}
                }
            }
            blurTexture.SetData<UInt32>(toSet);
        }

        /// <summary>
        /// Sets position of Radial and Fuzz Blur overlays
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="X_speed"></param>
        /// <param name="Y_speed"></param>
        public void setPos(int x, int y, double X_speed, double Y_speed)
        {
            //Control Outer-Bounds of Blur Radius Movement
            if (x > 1040)
                x = 1040;
            if (x < 0)
                x = 0;
            if (y > 651)
                y = 651;
            if (y < 69)
                y = 69;

            blurRectq1.X = x;
            blurRectq1.Y = y - blurTexture.Height;
            blurRectq2.X = x - blurTexture.Width;
            blurRectq2.Y = y - blurTexture.Height;
            blurRectq3.X = x - blurTexture.Width;
            blurRectq3.Y = y;
            blurRectq4.X = x;
            blurRectq4.Y = y;
        }

        /// <summary>
        /// Updates the Position of the Blur Images
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="X_speed"></param>
        /// <param name="Y_speed"></param>
        public void Update(GameTime gameTime, double X_speed, double Y_speed, double intoxicationLevel,
                            float tolerance)
        {
            blurRect2q1.X += (int)(0.80 * X_speed);
            blurRect2q1.Y += (int)(0.45 * Y_speed);
            blurRect2q2.X += (int)(0.80 * X_speed);
            blurRect2q2.Y += (int)(0.45 * Y_speed);
            blurRect2q3.X += (int)(0.80 * X_speed);
            blurRect2q3.Y += (int)(0.45 * Y_speed);
            blurRect2q4.X += (int)(0.80 * X_speed);
            blurRect2q4.Y += (int)(0.45 * Y_speed);

            if (blurRect2q1.X > blurRectq1.X + (1 - tolerance) * (2.5 * intoxicationLevel))
            {
                blurRect2q1.X = blurRectq1.X + (int)((1 - tolerance) * 2.5 * intoxicationLevel);
                blurRect2q2.X = blurRect2q1.X - blurTexture.Width;
                blurRect2q3.X = blurRect2q1.X - blurTexture.Width;
                blurRect2q4.X = blurRect2q1.X;
            }
            if (blurRect2q1.X < blurRectq1.X - (1 - tolerance) * (2.5 * intoxicationLevel))
            {
                blurRect2q1.X = blurRectq1.X - (int)((1 - tolerance) * 2.5 * intoxicationLevel);
                blurRect2q2.X = blurRect2q1.X - blurTexture.Width;
                blurRect2q3.X = blurRect2q1.X - blurTexture.Width;
                blurRect2q4.X = blurRect2q1.X;
            }
            if (blurRect2q1.Y > blurRectq1.Y + (1 - tolerance) * (0.50 * intoxicationLevel))
            {
                blurRect2q1.Y = blurRectq1.Y + (int)((1 - tolerance) * 0.50 * intoxicationLevel);
                blurRect2q2.Y = blurRect2q1.Y;
                blurRect2q3.Y = blurRect2q1.Y + blurTexture.Height;
                blurRect2q4.Y = blurRect2q1.Y + blurTexture.Height;
            }
            if (blurRect2q1.Y < blurRectq1.Y - (1 - tolerance) * (0.50 * intoxicationLevel))
            {
                blurRect2q1.Y = blurRectq1.Y - (int)((1 - tolerance) * 0.50 * intoxicationLevel);
                blurRect2q2.Y = blurRect2q1.Y;
                blurRect2q3.Y = blurRect2q1.Y + blurTexture.Height;
                blurRect2q4.Y = blurRect2q1.Y + blurTexture.Height;
            }
        }

        public void Draw(SpriteBatch sprBatch)
        {
            #region BLUR IMAGE 1 DRAW
            // Quadrant 2 draw
            sprBatch.Draw(blurTexture, blurRectq2, Color.White);
                // Quadrant 1 draw
            sprBatch.Draw(blurTexture, blurRectq1,
                null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
                // Quadrant 3 draw
            sprBatch.Draw(blurTexture, blurRectq3,
                null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
                // Quadrant 4 draw
            sprBatch.Draw(blurTexture, blurRectq4,
                null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally, 0.0f);
            #endregion

            #region BLUR IMAGE 2 DRAW
            // Quadrant 2 draw
            sprBatch.Draw(blurTexture, blurRect2q2, Color.White);
            // Quadrant 1 draw
            sprBatch.Draw(blurTexture, blurRect2q1,
                null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            // Quadrant 3 draw
            sprBatch.Draw(blurTexture, blurRect2q3,
                null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            // Quadrant 4 draw
            sprBatch.Draw(blurTexture, blurRect2q4,
                null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally, 0.0f);
            #endregion
        }
    }
}
