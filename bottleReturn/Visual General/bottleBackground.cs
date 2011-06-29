#define REGIONS_SUCK
#define IM_SO_CLEVER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using bottleReturn;

namespace bottleReturn
{
    class bottleBackground
    {
        #region fields
        private int machineNum;                                     // Number of machines
        private Texture2D cansReject, plasticReject, glassReject;   // Textures for returnable items 
        private Texture2D glass, plastic, cans;                     // Texture for each machine
        private Texture2D bgColor;                                  // Background of the background. Lol.
        private Rectangle bgRect;                                   // The rectangle for the whole thing.
        private List<machine> machineArray;                         // Array of machines:
        private Random WOLOLOLO;                                    // Random object:
        private Texture2D masterTexture;
        SoundEffect snd_cans;
        SoundEffect snd_glass;
        SoundEffect snd_plastic;
        SoundEffect snd_ticket;

        SoundEffect snd_glassDrop;
        SoundEffect snd_plasticDrop;
        SoundEffect snd_canDrop;
        List<drop> fallingItems;
        const int dropSpeed = 15;
        #endregion
        #region properties
        /// <summary>
        /// Gets the position of the background rectangle
        /// </summary>
        public Rectangle BgRect
        {
            get { return bgRect; }
        }

        /// <summary>
        /// Gets the position of the first machine
        /// </summary>
        public Vector2 MachineOnePos
        {
            get { return machineArray[0].Position; }

        }
        #endregion

        /// <summary>
        /// Creates a background texture dynamically.
        /// </summary>
        /// <param name="numMachines">The number of machines to draw.</param>
        /// <param name="screenSize">How big this background should be.</param>
        public bottleBackground(int numMachines, Vector2 screenSize)
        {
            machineNum = numMachines;
            bgRect = new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y);
            WOLOLOLO = new Random();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="bGround"></param>
        public bottleBackground(bottleBackground bGround)
        {
            machineNum = bGround.machineNum;
            bgRect = bGround.bgRect;

            machineArray = new List<machine>();
            foreach (machine index in bGround.machineArray)
            {
                machineArray.Add(new machine(index));
            }
        }

        public void Load(ContentManager globManager, GraphicsDevice gDevice, bool transparency)
        {
            if (!transparency)
            {
                snd_glassDrop = globManager.Load<SoundEffect>("Sounds/glassBreak");
                snd_plasticDrop = globManager.Load<SoundEffect>("Sounds/plasticDrop");
                snd_canDrop = globManager.Load<SoundEffect>("Sounds/canDrop");
                snd_cans = globManager.Load<SoundEffect>("Sounds/RVM-can");
                snd_glass = globManager.Load<SoundEffect>("Sounds/RVM-glass");
                snd_plastic = globManager.Load<SoundEffect>("Sounds/RVM-plastic");
                snd_ticket = globManager.Load<SoundEffect>("Sounds/RVM-ticket");
                glass = globManager.Load<Texture2D>("gamePlayScreen/glass");
                plastic = globManager.Load<Texture2D>("gamePlayScreen/plastic");
                cans = globManager.Load<Texture2D>("gamePlayScreen/cans");
                
                cansReject = globManager.Load<Texture2D>("gamePlayScreen/Returnables/can");
                plasticReject = globManager.Load<Texture2D>("gamePlayScreen/Returnables/plastic");
                glassReject = globManager.Load<Texture2D>("gamePlayScreen/Returnables/glass_empty");
                Texture2D[] rejectArray = new Texture2D[3] { glassReject, plasticReject, cansReject };

                fallingItems = new List<drop>();

                bgRect.Width = 628 * machineNum;
                bgRect.Height = 719;

                RenderTarget2D backgroundRender = new RenderTarget2D(gDevice, bgRect.Width, bgRect.Height);
                SpriteBatch sprBatch = new SpriteBatch(gDevice);
                gDevice.SetRenderTarget(backgroundRender);
                bgColor = (Texture2D)backgroundRender;

                machineArray = new List<machine>();
                List<machine> tempList = new List<machine>();
                // Add new machines to a the array:
                for (int i = 0; i < machineNum; i++) 
                {
                    switch (i % 3)
                    {
                        // NOTE: Each texture is 628 pixels wide.
                        case 0:
                            tempList.Add(new machine(Vector2.Zero, glass, machineType.GLASS, gDevice,
                                            snd_glass, snd_ticket, rejectArray));
                            break;
                        case 1:
                            tempList.Add(new machine(Vector2.Zero, plastic, machineType.PLASTIC, gDevice,
                                            snd_plastic, snd_ticket, rejectArray));
                            break;
                        case 2:
                            tempList.Add(new machine(Vector2.Zero, cans, machineType.CANS, gDevice,
                                            snd_cans, snd_ticket, rejectArray));
                            break;
                    }
                }
                // Shuffle the array:
                // (Fisher-Yates shuffle)
                for (int i=0; i< machineNum;i++)
                {
                    int swapIndex = WOLOLOLO.Next(0, tempList.Count);
                    machineArray.Add(tempList[swapIndex]);
                    tempList.Remove(tempList[swapIndex]);
                    machineArray[i].Position = new Vector2(i * 628, 0);
                }

                // Create MasterTexture:
                sprBatch.Begin();
                gDevice.Clear(Color.Blue);
                RenderTarget2D renderTarget;
                renderTarget = new RenderTarget2D(gDevice, bgRect.Width, bgRect.Height);
                gDevice.SetRenderTarget(renderTarget);
                gDevice.Clear(new Color(0, 0, 0, 0));
                foreach (machine index in machineArray)
                {
                    index.Draw(sprBatch);
                }
                sprBatch.End();
                masterTexture = (Texture2D)renderTarget;
                gDevice.SetRenderTarget(null);
            }
            else
            {
                bgRect.X = 0;
                bgRect.Y = 0;
                bgRect.Width = 628 * machineNum;
                bgRect.Height = 719;

                // Create RenderTarget
                SpriteBatch sprBatch = new SpriteBatch(gDevice);
                sprBatch.Begin();
                RenderTarget2D renderTarget;
                renderTarget = new RenderTarget2D(gDevice, bgRect.Width, bgRect.Height);
                gDevice.SetRenderTarget(renderTarget);
                gDevice.Clear(new Color(0, 0, 0, 0));
                foreach (machine index in machineArray)
                {
                    index.DrawFade(sprBatch);
                }
                sprBatch.End();
                masterTexture = (Texture2D)renderTarget;
                gDevice.SetRenderTarget(null);
            }
        }

        public void Update(GameTime gTime, GameplayScreen screen, float hitBoxScale, float rotation,
                            character player, float fadeSpeed_X, float fadeSpeed_Y)
        {
            for (int index = 0; index < machineArray.Count; index++)
            {
                machineArray[index].Update(gTime, hitBoxScale, index, rotation, BgRect);
            }

            //foreach (drop index in fallingItems)
            for (int i=0;i<fallingItems.Count;i++)
            {
                if (fallingItems[i].fade)
                {
                    fallingItems[i].UpdateFade(gTime, new Vector2(0, dropSpeed));//new Vector2(fadeSpeed_X , fadeSpeed_Y));
                }
                else
                {
                    fallingItems[i].Update(gTime, new Vector2(0, dropSpeed));
                }
                if (fallingItems[i].getLocation().Y > 720)
                {
                    if (screen.ScreenManager.GlobalOptions.SOUND_ENABLE)
                    {
                        fallingItems[i].PlaySound();
                    }

                    fallingItems.RemoveAt(i);
                }
            }
        }

        public void CheckCollisions(GameTime gTime, GameplayScreen screen, character Player, bool buttonClick, Vector2 mousePos,
                                        GameplayScreen.playerHand hand, GameplayScreen.playerHand hand2)
        {
            bool hit = false;

            if (buttonClick)
            {
                foreach (machine index in machineArray)
                {
                    bool tempBool = index.CheckCollisions(gTime, screen, Player, mousePos, hand);
                    if (tempBool)
                        hit = tempBool;
                }
                if (hit == false && hand.isEmpty == false)
                {
                    #region RANDOM CHANCE OF DROP
                    if ((WOLOLOLO.NextDouble() < Player.DATA.stat[5]) &&
                            Player.INVENTORY.BOTTLES.Count > 0)
                    {
                        drop fallingItem = null;
                        drop fallingItem2;
                        if (Player.INVENTORY.BOTTLES[0].type == returnablesLIST.Type.glass)
                        {
                            hand.isEmpty = true;
                            fallingItem = new drop(glassReject, mousePos, snd_glassDrop, true, false);
                            fallingItem2 = new drop(glassReject, new Vector2(hand2.X, hand2.Y), snd_glassDrop, true, true);
                            fallingItems.Add(fallingItem);
                            fallingItems.Add(fallingItem2);
                        }
                        else if (Player.INVENTORY.BOTTLES[0].type == returnablesLIST.Type.can)
                        {
                            hand.isEmpty = true;
                            fallingItem = new drop(cansReject, mousePos, snd_canDrop, false, false);
                            fallingItem2 = new drop(cansReject, new Vector2(hand2.X, hand2.Y), snd_canDrop, false, true);
                            fallingItems.Add(fallingItem);
                            fallingItems.Add(fallingItem2);
                        }
                        else if (Player.INVENTORY.BOTTLES[0].type == returnablesLIST.Type.plastic)
                        {
                            hand.isEmpty = true;
                            fallingItem = new drop(plasticReject, mousePos, snd_plasticDrop, false, false);
                            fallingItem2 = new drop(plasticReject, new Vector2(hand2.X, hand2.Y), snd_plasticDrop, false, true);
                            fallingItems.Add(fallingItem);
                            fallingItems.Add(fallingItem2);
                        }

                        if (fallingItem != null && fallingItem.breakable)
                        {
                            Player.INVENTORY.BOTTLES.RemoveAt(0);
                        }
                        else if (Player.INVENTORY.BOTTLES[0] != null)
                        {
                            Player.INVENTORY.BOTTLES.Add(Player.INVENTORY.BOTTLES[0]);
                            Player.INVENTORY.BOTTLES.RemoveAt(0);
                        }

                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Increments the position of this background.
        /// </summary>
        /// <param name="x">X to add</param>
        /// <param name="y">Y to add</param>
        public void incPos(int x, int y)
        {
            bgRect.X += x;
            bgRect.Y += y;

            for (int index = 0; index < machineNum; index++)
            {
                machineArray[index].Position = new Vector2(bgRect.X + (index * 628), bgRect.Y);
            }
        }

        /// <summary>
        /// Sets the position of this background to a 
        /// static value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void setPos(int x, int y)
        {
            bgRect.X = x;
            bgRect.Y = y;
            for (int index = 0; index < machineNum; index++)
            {
                machineArray[index].Position = new Vector2(x + (index * 628), y);
            }
        }

        /// <summary>
        /// Standard Draw Method
        /// </summary>
        /// <param name="sprBatch"></param>
        public void Draw(SpriteBatch sprBatch, float rotation, bool debug)
        {
            foreach (machine index in machineArray)
            {
                index.drawReject(sprBatch);
            }
            sprBatch.Draw(masterTexture, bgRect, null, Color.White, rotation,
                new Vector2(0, 0), SpriteEffects.None, 1);

            foreach (machine index in machineArray)
            {
                if (debug)
                    index.DrawHitbox(sprBatch);
            }
        }

        /// <summary>
        /// Draw Method For Dropped Bottles
        /// </summary>
        /// <param name="sprBatch"></param>
        public void DrawDrops(SpriteBatch sprBatch)
        {
            foreach (drop index in fallingItems)
            {
                if (index.fade)
                {
                    index.DrawFade(sprBatch);
                }
                else
                {
                    index.Draw(sprBatch);
                }
            }
        }

        /// <summary>
        /// Draw Method for Fade Image
        /// </summary>
        /// <param name="sprBatch"></param>
        public void DrawFade(SpriteBatch sprBatch, float rotation)
        {
            sprBatch.Draw(masterTexture, bgRect, null, Color.White, rotation,
                new Vector2(0, 0), SpriteEffects.None, 1);
        }

        public float getSoundPanValues(int machineNumber)
        {
            return machineArray[machineNumber].currentSound.Pan;
        }
    }
}
