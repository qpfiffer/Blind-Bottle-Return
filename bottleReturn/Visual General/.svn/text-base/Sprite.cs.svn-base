using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    public class Sprite    //***Superclass***
    {
        //Private members
        protected Texture2D mImage;

        protected Vector2 mPosition;
        protected Vector2 mVelocity;
        protected Vector2 mAcceleration;
        protected float mSpeedMultiplier = 1.0f;

        # region Properties
        public int X
            { get { return (int)mPosition.X; } }
        public int Y
            { get { return (int)mPosition.Y; } }   
        public int Width
            { get { return mImage.Width; } }
        public int Height
            { get { return mImage.Height; } }
        public float Speed
            { get { return mSpeedMultiplier; } }
        public Vector2 Velocity
            { get { return mVelocity; } }
        public Rectangle Hitbox
            { get { return (new Rectangle((int)mPosition.X, (int)mPosition.Y,
                                          mImage.Width, mImage.Height)); } }

        public Texture2D ReloadImage
            { set { mImage = value; } }
        public float setSpeedMultiplier
            { set { mSpeedMultiplier = value; } }
        public Vector2 setPosition
            { set { mPosition.X = value.X;
                    mPosition.Y = value.Y; } }
        public Vector2 setVelocity
            { set { mVelocity.X = value.X;
                    mVelocity.Y = value.Y; } }
        #endregion

        //Constructors
        public Sprite(Texture2D texture, Vector2 pos, Vector2 vel)
        {
            mImage = texture;
            mPosition = pos;
            mVelocity = vel;
        }
        public Sprite(Texture2D texture, Vector2 pos, Vector2 vel, float spd)
        {
            mImage = texture;
            mPosition = pos;
            mVelocity = vel;
            mSpeedMultiplier = spd;
        }
        public Sprite(Texture2D texture, Vector2 pos, Vector2 vel, Vector2 acc)
        {
            mImage = texture;
            mPosition = pos;
            mVelocity = vel;
            mAcceleration = acc;
        }
        public Sprite(Texture2D texture, Vector2 pos, Vector2 vel, Vector2 acc, float spd)
        {
            mImage = texture;
            mPosition = pos;
            mVelocity = vel;
            mAcceleration = acc;
            mSpeedMultiplier = spd;
        }
        public Sprite(Texture2D texture, Vector2 pos)
        {
            mImage = texture;
            mPosition = pos;
        }

        //General methods
        public void CheckBounds(int lower, int upper)
        {
            mPosition.X = Math.Max(0, Math.Min((int)mPosition.X, upper - mImage.Width));
        }

        public virtual void Draw(SpriteBatch sprBatch)
        {
            sprBatch.Draw(mImage, mPosition, Color.White);
        }
        public void Draw(SpriteBatch sprBatch, float Wscale, float Hscale)
        {
            sprBatch.Draw(mImage, new Rectangle((int)mPosition.X, (int)mPosition.Y, (int)(mImage.Width * Wscale), (int)(mImage.Height * Hscale)), Color.White);
        }
        public void Draw(SpriteBatch sprBatch, Rectangle rect, float rotation, bool wheel, SpriteEffects flip, float layer)
        {
            if (wheel)
                sprBatch.Draw(mImage, rect, null, Color.White, rotation, new Vector2(mImage.Width / 2, mImage.Height / 2), flip, layer);
            else
                sprBatch.Draw(mImage, new Rectangle((int)mPosition.X, (int)mPosition.Y, mImage.Width, mImage.Height), null, Color.White, rotation, new Vector2(0, 0), flip, layer);
        }
        public void Draw(SpriteBatch sprBatch, Rectangle destRect)
        {
            sprBatch.Draw(mImage, destRect, Color.White);
        }
        public void Draw(SpriteBatch sprBatch, float radians, Color color)
        {
            sprBatch.Draw(mImage, new Rectangle((int)mPosition.X, (int)mPosition.Y, mImage.Width, mImage.Height), null, color, radians, new Vector2(0, 0), SpriteEffects.None, 0);
        }
        public void Draw(SpriteBatch sprBatch, SpriteEffects flip)
        {
            sprBatch.Draw(mImage, new Rectangle((int)mPosition.X, (int)mPosition.Y, mImage.Width, mImage.Height), null, Color.White, 0, new Vector2(0, 0), flip, 0);
        }
        public void DrawFlipped(SpriteBatch sprBatch, float Wscale, float Hscale)
        {
            sprBatch.Draw(mImage, new Rectangle((int)mPosition.X, (int)mPosition.Y, (int)(mImage.Width * Wscale), (int)(mImage.Height * Hscale)), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
        }


        public void HorizontalBounce()
        {
            mVelocity.X *= -1;
            mPosition.X += mVelocity.X * mSpeedMultiplier;
        }

        public void Update()
        {
            mPosition.X += mVelocity.X * mSpeedMultiplier;
            mPosition.Y += mVelocity.Y * mSpeedMultiplier;
            mVelocity.X += mAcceleration.X;
            mVelocity.Y += mAcceleration.Y;
        }
    }
}
