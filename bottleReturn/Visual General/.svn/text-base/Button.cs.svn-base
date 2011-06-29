using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Sprites;

namespace ButtonSprites
{
    class Button : Sprite
    {
        #region fields

        protected bool visible = true;   //...not yet implemented

        protected bool downclicked = false;
        protected bool activated = false;

        protected Texture2D imgNormal;
        protected Texture2D imgMouseover;
        protected Texture2D imgClicked;
        protected SoundEffect seButtonPress = null;

        #endregion

        #region properties

        public bool IsVisible { get { return visible; } }
        public bool WasClicked { get { return downclicked; } }
        public bool WasActivated { get { bool result = activated;
                                         activated = false;
                                         return result; }
                                   set { activated = value; } }
        
        #endregion

        #region Mutators

        public void LoadMouseoverImage() { base.ReloadImage = imgMouseover; }
        public void LoadNormalImage() { base.ReloadImage = imgNormal; }
        public void Hide() { visible = false; }
        public void Show() { visible = true; }
        public void Activate() { activated = true; }

        #endregion


        public Button(Vector2 location, Texture2D normal, Texture2D mouseover, Texture2D clicked)
            : base(normal, location, new Vector2(0,0))
        {
            imgNormal = normal;
            imgMouseover = mouseover;
            imgClicked = clicked;
        }
        public Button(Vector2 location, Texture2D normal, Texture2D mouseover, Texture2D clicked, SoundEffect sound)
            : base(normal, location, new Vector2(0, 0))
        {
            imgNormal = normal;
            imgMouseover = mouseover;
            imgClicked = clicked;
            seButtonPress = sound;
        }


        public void ClickDown(Vector2 mouseLoc)
        {
            if (visible && MouseHit(mouseLoc))
            {
                base.ReloadImage = imgClicked;
                downclicked = true;
            }
        }

        public void ClickUp(Vector2 mouseLoc)
        {
            if (MouseHit(mouseLoc))
            {
                if (visible && downclicked)
                    activated = true;
                LoadMouseoverImage();
            }
            else
            {
                LoadNormalImage();
            }
            downclicked = false;
        }

        public new void Draw(SpriteBatch sprBatch)
        {
            if (visible)
                base.Draw(sprBatch);
        }

        public virtual bool MouseHit(Vector2 mouseLoc)
        {
            if (mouseLoc.X >= Hitbox.Left && mouseLoc.X <= Hitbox.Right
                    && mouseLoc.Y >= Hitbox.Top && mouseLoc.Y <= Hitbox.Bottom)
            {
                if (!downclicked)
                    LoadMouseoverImage();
                return true;
            }
            else
            {
                if (!downclicked)
                    LoadNormalImage();
                return false;
            }
        }
    }
}
