using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace bottleReturn
{
    class drop
    {
        Vector2 location;
        Texture2D dropTexture;
        SoundEffectInstance currentSound;
        float soundPan;
        public bool breakable;
        public bool fade;

        public drop(Texture2D objectTexure, Vector2 location, SoundEffect sound,
                    bool breakable, bool fade)
        {
            this.location = location;
            this.dropTexture = objectTexure;
            currentSound = sound.CreateInstance();
            this.breakable = breakable;
            this.fade = fade;
        }

        public Vector2 getLocation()
        {
            return location;
        }

        public void Update(GameTime gameTime, Vector2 speed)
        {
            location.Y += speed.Y;
            soundPan = ((float)(location.X - 520) / 520);
            soundPan = MathHelper.Clamp(soundPan, -1.0f, 1.0f);
            currentSound.Pan = soundPan;
        }

        public void UpdateFade(GameTime gameTime, Vector2 speed)
        {
            location.Y += 5 + (int)(0.40 * speed.Y);
            location.X += (int)(0.60 * speed.X);
            soundPan = ((float)(location.X - 520) / 520);
            soundPan = MathHelper.Clamp(soundPan, -1.0f, 1.0f);
            currentSound.Pan = soundPan;
        }

        public void Draw(SpriteBatch sprBatch)
        {
            sprBatch.Draw(dropTexture, location, Color.White);
        }

        public void DrawFade(SpriteBatch sprBatch)
        {
            sprBatch.Draw(dropTexture, location, Color.White * 0.5f);
        }

        public void PlaySound()
        {
            if (currentSound.State != SoundState.Playing && !fade)
            {
                currentSound.Play();
            }
        }
    }
}
