using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Sprites;

namespace bottleReturn
{
    #region healthBar class
    class Bar : Sprite
    {
        private Rectangle cliptangle;
        private Rectangle posTangle;

        public Bar(Texture2D texture, Vector2 pos, Rectangle cliptangle): 
            base(texture, pos)
        {
            this.posTangle = cliptangle;
            posTangle.X = (int)pos.X;
            posTangle.Y = (int)pos.Y;
            this.cliptangle = cliptangle;
        }

        public override void Draw(SpriteBatch sprBatch)
        {
            sprBatch.Draw(mImage, posTangle, cliptangle, Color.White);
        }

        public void DrawFlipped(SpriteBatch sprBatch)
        {
            //sprBatch.Draw(mImage, posTangle, cliptangle, Color.White, 0, new Vector2(cliptangle.X, cliptangle.Y),
            //                SpriteEffects.FlipHorizontally, 1);
            sprBatch.Draw(mImage, posTangle, cliptangle, Color.White, 0, new Vector2(cliptangle.X, cliptangle.Y),
                            SpriteEffects.FlipHorizontally, 1);
        }

        public void setWidth(int newWidth)
        {
            cliptangle.Width = newWidth;
            posTangle.Width = newWidth;
        }
    }
    #endregion

    class gameplayHUD
    {
        // Background images
        Sprite[] background;
        Vector2[] statLoc;
        int statRectWidth;
        int statRectHeight;
        SpriteFont fntAttribs;
        character Player;
        Texture2D smallBeer;
        int beerWidth;
        int beerHeight;
        Vector2[] inventoryLoc;
        Texture2D[] containerImage;
        Color reminderColor;

        Bar intoxBar;
        Bar odorBar;
        Bar energy;
        Bar wantedLevel;

        Rectangle portraitRect;
        Texture2D portraitTexture;

        /// <summary>
        /// Creates a new HUD for GamePlay Screen
        /// </summary>
        /// <param name="horizontalBar"></param>
        /// <param name="verticalBar"></param>
        /// <param name="powerBar1"></param>
        /// <param name="powerBar2"></param>
        /// <param name="statsFont"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="Player"></param>
        public gameplayHUD(Texture2D horizontalBar, Texture2D verticalBar, Texture2D powerBar1, Texture2D powerBar2,
                            Texture2D powerBar3, Texture2D smallBeer, Texture2D portraitTexture,
                            Texture2D paperBag, Texture2D trashBag, Texture2D cart,
                            SpriteFont statsFont, int screenWidth, int screenHeight, character Player)
        {
            beerHeight = (int)(horizontalBar.Height * 0.75);
            beerWidth = 12;
            this.Player = Player;
            this.smallBeer = smallBeer;
            containerImage = new Texture2D[3] { paperBag,
                                                trashBag,
                                                cart };
            reminderColor = Color.Black;
            fntAttribs = statsFont;
            #region BACKGROUND RECTANGLE ARRAY INITIALIZATION
            background = new Sprite[3];
            background[0] = new Sprite(horizontalBar, Vector2.Zero);
            background[1] = new Sprite(horizontalBar, new Vector2(0, screenHeight - horizontalBar.Height));
            background[2] = new Sprite(verticalBar, new Vector2(screenWidth - verticalBar.Width, 0));
            #endregion
            #region STATS RECTANGLE ARRAY INITIALIZATION
            statRectWidth = ((horizontalBar.Width - verticalBar.Width) / 2) - 10;
            statRectHeight = (int)(horizontalBar.Height * 0.75);

            intoxBar = new Bar(powerBar1, new Vector2(0, 8),
                new Rectangle(0, 0, statRectWidth, statRectHeight));

            odorBar = new Bar(powerBar3, new Vector2((((horizontalBar.Width - verticalBar.Width) / 2) + 10), 8),
                new Rectangle(0, 0, statRectWidth, statRectHeight));

            energy = new Bar(powerBar2, new Vector2(0, (screenHeight - horizontalBar.Height + 8)),
                new Rectangle(0, 0, statRectWidth, statRectHeight));

            wantedLevel = new Bar(powerBar1, new Vector2((((horizontalBar.Width - verticalBar.Width) / 2) + 10),
                (screenHeight - horizontalBar.Height + 8)),
                new Rectangle(0, 0, statRectWidth, statRectHeight));
            #endregion
            #region STAT BARS VECTOR2 ARRAY INITIALIZATION
            statLoc = new Vector2[6];
            for (int index = 0; index < 2; index++)
            {
                statLoc[index].X = (index * ((horizontalBar.Width - verticalBar.Width) / 2) + 10) + 4;
                statLoc[index].Y = 10;
            }
            for (int index = 0; index < 2; index++)
            {
                statLoc[index + 2].X = (index * ((horizontalBar.Width - verticalBar.Width) / 2) + 10) + 4;
                statLoc[index + 2].Y = (screenHeight - horizontalBar.Height + 8) + 2;
            }
            #endregion

            #region CHARACTER PORTRAIT INITIALIZATION
            this.portraitTexture = portraitTexture;
            portraitRect = new Rectangle(screenWidth - verticalBar.Width + 8, 8, 224, 268);
            #endregion
            #region CHARACTER INVENTORY INITIALIZATION
            inventoryLoc = new Vector2[7] { new Vector2(screenWidth - verticalBar.Width + 73, 345),
                                            new Vector2(1150, 390),
                                            new Vector2(screenWidth - verticalBar.Width + 16, 495),
                                            new Vector2(screenWidth - verticalBar.Width + 16, 525),
                                            new Vector2(screenWidth - verticalBar.Width + 16, 555),
                                            new Vector2(screenWidth - verticalBar.Width + 16, 585),
                                            new Vector2(screenWidth - verticalBar.Width + 58, 620) };
            #endregion
        }

        public void Update(GameTime gameTime)
        {
            intoxBar.setWidth((int)(statRectWidth * (Player.intoxicationLevel / 100)));
            odorBar.setWidth((int)(statRectWidth * (Player.odorLevel / 100)));
            energy.setWidth((int)(statRectWidth * (Player.energy / 100)));
            wantedLevel.setWidth((int)(statRectWidth * (Player.wantedLevel / 10)));
            if (gameTime.TotalGameTime.Milliseconds % 400 < 200)
                reminderColor = Color.Beige;
            else
                reminderColor = Color.Black;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                //I'm a shitty Artist...Need help making this look better
                //possibly changing placement/shape of HUD
            
                //Draw Background for HUD
            foreach (Sprite index in background)
                index.Draw(spriteBatch);

                //Draw Status Bars and Values
            for (int index = 0; index < (int)(Player.intoxicationLevel / 2.35); index++)
            {
                spriteBatch.Draw(smallBeer, new Rectangle((index * beerWidth) + 2, 8, 12, beerHeight), Color.LightGray);
            }
            for (int index = (int)(Player.intoxicationLevel / 2.35); index < 42; index++)
            {
                spriteBatch.Draw(smallBeer, new Rectangle((index * beerWidth) + 2, 8, 12, beerHeight), Color.Gray);
            }
            spriteBatch.DrawString(fntAttribs, "Intoxication Level: " + Player.intoxicationLevel, statLoc[0], Color.White);

            odorBar.Draw(spriteBatch);
            spriteBatch.DrawString(fntAttribs, "Odor Level: " + (int)Player.odorLevel, statLoc[1], Color.WhiteSmoke);

            energy.Draw(spriteBatch);
            spriteBatch.DrawString(fntAttribs, "Energy: " + (int)Player.energy, statLoc[2], Color.Black);

            wantedLevel.DrawFlipped(spriteBatch);
            spriteBatch.DrawString(fntAttribs, "Wanted Level: " + (int)Player.wantedLevel, statLoc[3], Color.WhiteSmoke);

            #region PORTRAIT and DATA

            spriteBatch.Draw(portraitTexture, portraitRect, Color.White);
            int container = 0;
            if (Player.carryingCapacity == 50)
                container = 1;
            else if (Player.carryingCapacity == 150)
                container = 2;
            spriteBatch.Draw(containerImage[container], new Rectangle(1060, 285, 200, 200), Color.White);
            spriteBatch.DrawString(fntAttribs, "  Bottles\nRemaining:",
                                        inventoryLoc[0], Color.Yellow);                
            spriteBatch.DrawString(fntAttribs, "" + Player.INVENTORY.BOTTLES.Count.ToString(),
                                        inventoryLoc[1], Color.Yellow);
            spriteBatch.DrawString(fntAttribs, "Cash on Hand: $ " + Player.cash.ToString(),
                                        inventoryLoc[2], Color.Green);
           spriteBatch.DrawString(fntAttribs, "Tickets: " + Player.VOUCHERS.Count.ToString(),
                                        inventoryLoc[3], Color.White);
            spriteBatch.DrawString(fntAttribs, "Tickets Value: $ " + Player.ticketsValue.ToString(),
                                        inventoryLoc[4], Color.White);
            spriteBatch.DrawString(fntAttribs, "Score: " + Player.score,
                                        inventoryLoc[5], Color.Red);
            if (Player.INVENTORY.BOTTLES.Count == 0)
                spriteBatch.DrawString(fntAttribs, "Don't forget\nyour tickets!", inventoryLoc[6], reminderColor);
            #endregion
        }
    }
}
