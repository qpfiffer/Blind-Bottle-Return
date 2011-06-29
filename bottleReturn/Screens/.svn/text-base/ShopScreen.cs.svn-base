#region File Description
//-----------------------------------------------------------------------------
// CharacterSelectionScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ButtonSprites;
using Sprites;
#endregion

namespace bottleReturn
{
    /// <summary>
    /// This screen will appear between main gameplay levels.
    /// Here, the player will be able to spend his/her hard earned nickels on items that
    /// affect gameplay, and perform other various functions.
    /// </summary>
    class ShopScreen : GameScreen
    {
        #region Private Classes
        private class Buyable : Button //Inherits from: Sprite
        {
            private decimal cost;
            private float wScale;
            private float hScale;
            private string infoTitle;
            private string infoText;

            public decimal Cost { get { return cost; } }
            public string InfoTitle { get { return infoTitle; } }
            public string InfoText { get { return infoText; } }

            public Buyable(Vector2 location, Texture2D normal, Texture2D mouseover, Texture2D clicked, decimal cost, float wScale, float hScale, string infoTitle, string infoText)
                : base(location, normal, mouseover, clicked)
            {
                this.cost = cost;
                this.wScale = wScale;
                this.hScale = hScale;
                this.infoTitle = infoTitle;
                this.infoText = infoText;
            }

            public new void Draw(SpriteBatch spriteBatch)
            {
                if (visible)
                    spriteBatch.Draw(base.mImage, new Rectangle(base.X, base.Y, (int)(base.Width * wScale), (int)(base.Height * hScale)), null, Color.White, 0, new Vector2(0, base.Height), SpriteEffects.None, 0);
            }

            /// <summary>
            /// Buyable items in the store are not drawn at 100% scale.
            /// This function overrides Button.MouseHit() to take the draw scale of the image into account
            /// </summary>
            /// <param name="mouseLoc"></param>
            /// <returns></returns>
            public override bool MouseHit(Vector2 mouseLoc)
            {
                if (mouseLoc.X >= base.X && mouseLoc.X <= (base.X + (base.Width * wScale))
                        && mouseLoc.Y >= (base.Y - (base.Height * hScale)) && mouseLoc.Y <= base.Y)
                {
                    if (!downclicked)
                        LoadMouseoverImage();
                    return visible; //returns TRUE unless button is invisible
                }
                else
                {
                    if (!downclicked)
                        LoadNormalImage();
                    return false;
                }
            }
        }

        public class miniHUD
        {
            Texture2D bGround_color;
            Rectangle HUD_bGround;
            int stat_width;
            int stat_height;
            Bar intoxBar;
            Bar wantedBar;
            Bar odorBar;
            Bar energyBar;

            public miniHUD(Texture2D bGround, Texture2D bar, Rectangle bGround_rect)
            {
                HUD_bGround = bGround_rect;
                bGround_color = bGround;
                stat_height = 18;
                stat_width = bGround_rect.Width / 2 - 16;
                intoxBar = new Bar(bar, new Vector2(bGround_rect.X + 8, bGround_rect.Y + 28),
                                        new Rectangle(bGround_rect.X + 8, bGround_rect.Y + 28, stat_width, stat_height));
                energyBar = new Bar(bar, new Vector2(bGround_rect.X + 8, bGround_rect.Y + 73),
                                         new Rectangle(bGround_rect.X + 8, bGround_rect.Y + 73, stat_width, stat_height));
                wantedBar = new Bar(bar, new Vector2(bGround_rect.X + 274, bGround_rect.Y + 28),
                                        new Rectangle(bGround_rect.X + 274, bGround_rect.Y + 28, stat_width, stat_height));
                odorBar = new Bar(bar, new Vector2(bGround_rect.X + 274, bGround_rect.Y + 73),
                                        new Rectangle(bGround_rect.X + 274, bGround_rect.Y + 73, stat_width, stat_height));
            }

            public void Update(float intox, float wanted, float energy, float odor)
            {
                intoxBar.setWidth((int)(stat_width * (intox / 100)));
                energyBar.setWidth((int)(stat_width * (energy / 100)));
                wantedBar.setWidth((int)(stat_width * (wanted / 10)));
                odorBar.setWidth((int)(stat_width * (odor / 100)));
            }

            public void Draw(SpriteBatch sprBatch, SpriteFont attribFont)
            {
                sprBatch.Draw(bGround_color, HUD_bGround, new Color(142, 65, 33, 0x99));
                sprBatch.DrawString(attribFont, "Intoxication", new Vector2(HUD_bGround.X + 8, HUD_bGround.Y + 5), Color.Khaki);
                intoxBar.Draw(sprBatch);
                sprBatch.DrawString(attribFont, "Energy", new Vector2(HUD_bGround.X + 8, HUD_bGround.Y + 50), Color.Khaki);
                energyBar.Draw(sprBatch);
                sprBatch.DrawString(attribFont, "Wanted level", new Vector2(HUD_bGround.X + 274, HUD_bGround.Y + 5), Color.Khaki);
                wantedBar.Draw(sprBatch);
                sprBatch.DrawString(attribFont, "Body odor", new Vector2(HUD_bGround.X + 274, HUD_bGround.Y + 50), Color.Khaki);
                odorBar.Draw(sprBatch);
            }
        }
        #endregion

        #region fields

        ContentManager content;

        public ContentManager Content
        {
            get { return content; }
        }

        miniHUD shopHUD;

        Texture2D blank;
        Texture2D powerBar;
        Texture2D txBackground;
        Texture2D txCursor;
        Texture2D mouseCursor;         //debugging tool
        Texture2D blackout;
        blur shopBLUR;
        double fadeSpeed_X;
        double fadeSpeed_Y;

        Sprite clerk;
        Sprite voucher; 
        List<Buyable> shopItems = new List<Buyable>();
        enum items { bread = 0, alcohol, cigarettes, lotteryTicket, soap, trashBags,
                     harassClerk, stealSaltines, stealCart, exit }

        SpriteFont fntTitle;
        SpriteFont fntInfo;

        Song musicBG;

        InputHandler input;
        bool IsPaused;
        float pauseAlpha;
        Random random = new Random();
        character Player;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopScreen(character player, double fadeSpeed_X, double fadeSpeed_Y)
        {
            TransitionOnTime = TimeSpan.FromSeconds(.5);
            TransitionOffTime = TimeSpan.FromSeconds(.5);
            this.Player = player;
            this.fadeSpeed_X = fadeSpeed_X;
            this.fadeSpeed_Y = fadeSpeed_Y;
            IsPaused = false;
        }

        public override void UnloadContent()
        {
            //if (content != null)
            //    content.Unload();
            base.UnloadContent();
        }

        /// <summary>
        /// Load visual/audio content for this screen
        /// </summary>
        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            blank = content.Load<Texture2D>("blank");
            powerBar = content.Load<Texture2D>("gamePlayScreen/HUD/health_bar1");

            txBackground = content.Load<Texture2D>("Shop/store");
            txCursor = content.Load<Texture2D>("Shop/hand point");
            mouseCursor = content.Load<Texture2D>("Cursor");

            blackout = new Texture2D(ScreenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blackout.SetData<UInt32>(new UInt32[]{0xAA333333});

            clerk = new Sprite(content.Load<Texture2D>("Shop/Clerk"), new Vector2(237, 261), new Vector2(0,0));
            voucher = new Sprite(content.Load<Texture2D>("Shop/Receipt"), new Vector2(1, 1), new Vector2(0, 0));

            shopHUD = new miniHUD(blank, powerBar, new Rectangle(160, 20, 532, 100));
            shopBLUR = new blur(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height,
                                new Vector2(0, 0), ScreenManager.GraphicsDevice, Player.VISUALS.stat[0], Player.VISUALS.stat[1]);
                                                                    

            //*******************************************************************************
            //*Buyables must be added in the order they appear in the enumerated "items" type
            //*******************************************************************************
            #region Generate Buyables List
            //Bread
            shopItems.Add(new Buyable(new Vector2(795, 362),
                                      content.Load<Texture2D>("Shop/breadUP"), content.Load<Texture2D>("Shop/breadMO"), content.Load<Texture2D>("Shop/breadDN"),
                                      2.49m, 0.50f, 0.50f,
                                      "$2.49 - Bread", "Yum\n++Energy   -Intoxication"));
            //Alcohol
            shopItems.Add(new Buyable(new Vector2(235, 690),
                                      content.Load<Texture2D>("Shop/alcoholUP"), content.Load<Texture2D>("Shop/alcoholMO"), content.Load<Texture2D>("Shop/alcoholDN"),
                                      2.34m, 0.18f, 0.18f,
                                      "$2.34 - Alcohol", "Bottoms up!\n+Intoxication"));
            //Cigarettes
            shopItems.Add(new Buyable(new Vector2(425, 379),
                                      content.Load<Texture2D>("Shop/smokesUP"), content.Load<Texture2D>("Shop/smokesMO"), content.Load<Texture2D>("Shop/smokesDN"),
                                      4.50m, 1.00f, 1.00f,
                                      "$4.50 - Cigarettes", "Smoke 'em if ya got 'em\n+Odor"));
            //Lotto ticket
            shopItems.Add(new Buyable(new Vector2(1120, 418),
                                      content.Load<Texture2D>("Shop/lottoTicketUP"), content.Load<Texture2D>("Shop/lottoTicketMO"), content.Load<Texture2D>("Shop/lottoTicketDN"),
                                      1.00m, 0.50f, 0.50f,
                                      "$1.00 - Lottery Ticket", "Gotta spend money to make money\n+Energy   +Extra cash (low chance)"));
            //Soap
            shopItems.Add(new Buyable(new Vector2(775, 421),
                                      content.Load<Texture2D>("Shop/soapUP"), content.Load<Texture2D>("Shop/soapMO"), content.Load<Texture2D>("Shop/soapDN"),
                                      .79m, 0.15f, 0.15f,
                                      "$0.79 - Soap", "Take a bum shower\n--Odor"));
            //Garbage bags
            shopItems.Add(new Buyable(new Vector2(10, 655),
                                      content.Load<Texture2D>("Shop/trashBagsUP"), content.Load<Texture2D>("Shop/trashBagsMO"), content.Load<Texture2D>("Shop/trashBagsDN"),
                                      1.99m, 0.17f, 0.17f,
                                      "$1.99 - Trash Bags", "Increase your carrying capacity\n+Carrying capacity"));
            //Harass the clerk
            shopItems.Add(new Buyable(new Vector2(144, 250),
                                      content.Load<Texture2D>("Shop/harassUP"), content.Load<Texture2D>("Shop/harassMO"), content.Load<Texture2D>("Shop/harassDN"),
                                      0m, 1.00f, 1.00f,
                                      "Harass the clerk...", "You think you're better than me!?\nSuccess: +Energy   +Wanted level\nFailure: -Energy"));
            //Steal crackers
            shopItems.Add(new Buyable(new Vector2(442, 243),
                                      content.Load<Texture2D>("Shop/crackersUP"), content.Load<Texture2D>("Shop/crackersMO"), content.Load<Texture2D>("Shop/crackersDN"),
                                      0m, 1.00f, 1.00f,
                                      "Attempt to steal some crackers...", "Get in meh belly!\nSuccess: +Energy \n-Intoxication"));
            //Steal cart
            shopItems.Add(new Buyable(new Vector2(747, 236),
                                      content.Load<Texture2D>("Shop/cartUP"), content.Load<Texture2D>("Shop/cartMO"), content.Load<Texture2D>("Shop/cartDN"),
                                      0m, 1.00f, 1.00f,
                                      "Attempt to steal a shopping cart...", "This thing holds a lotta cans!\nSuccess: Max carrying capacity\n(Don't get caught!)"));
            //Exit
            shopItems.Add(new Buyable(new Vector2(1105, 93),
                                      content.Load<Texture2D>("Shop/exitUP"), content.Load<Texture2D>("Shop/exitMO"), content.Load<Texture2D>("Shop/exitDN"),
                                      0m, 0.50f, 0.50f,
                                      "Exit shop", "Return more bottles and cans"));
            #endregion

            fntTitle = ScreenManager.Font[(int)ScreenManager.fontStyle.game14B];
            fntInfo = ScreenManager.Font[(int)ScreenManager.fontStyle.shopinfo12I];


            if (ScreenManager.GlobalOptions.SOUND_ENABLE)
            {
                musicBG = ScreenManager.ShopMusic;
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(musicBG);
                }
            }

            Mouse.WindowHandle = base.ScreenManager.Game.Window.Handle;
        }
        #endregion

        #region Update, Handle Input, and Draw

        /// <summary>
        /// Move stuff...
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                
                #region BUTTON ACTIVATIONS
                if (shopItems[(int)items.bread].WasActivated && Player.cash >= shopItems[(int)items.bread].Cost)
                {   //buy Bread
                    Player.cash -= shopItems[(int)items.bread].Cost;
                    Player.energy += 15;
                    Player.intoxicationLevel -= (int)(20 * (1 - Player.DATA.stat[0]));
                }
                if (shopItems[(int)items.alcohol].WasActivated && Player.cash >= shopItems[(int)items.alcohol].Cost)
                {   //buy Alcohol
                    Player.cash -= shopItems[(int)items.alcohol].Cost;
                    Player.intoxicationLevel += (int)(30 * (1 - Player.DATA.stat[0]));
                }
                if (shopItems[(int)items.cigarettes].WasActivated && Player.cash >= shopItems[(int)items.cigarettes].Cost)
                {   //buy Cigarettes
                    Player.cash -= shopItems[(int)items.cigarettes].Cost;
                    Player.odorLevel += 15 * Player.DATA.stat[1];
                }
                if (shopItems[(int)items.lotteryTicket].WasActivated && Player.cash >= shopItems[(int)items.lotteryTicket].Cost)
                {   //buy Lotto ticket
                    Player.cash -= shopItems[(int)items.lotteryTicket].Cost;
                    Player.energy += 5;
                    if (random.Next(0, 100) > 95)
                    {
                        Player.cash += 1.5m * (random.Next(1, 10));
                    }
                }
                if (shopItems[(int)items.soap].WasActivated && Player.cash >= shopItems[(int)items.soap].Cost)
                {   //buy Soap
                    Player.cash -= shopItems[(int)items.soap].Cost;
                    Player.odorLevel -= 25 * Player.DATA.stat[1];
                }
                if (shopItems[(int)items.trashBags].WasActivated && Player.cash >= shopItems[(int)items.trashBags].Cost)
                {   //buy Trash bags
                    Player.cash -= shopItems[(int)items.trashBags].Cost;
                    Player.carryingCapacity = 50;
                    ScreenManager.AddScreen(new Backdrop(txBackground,
                                                         new StoryScreen(content.Load<Texture2D>("animations/hobo" + Player.hoboNumber + "Anim"), 8, 223, 267, true, 0,
                                                                         new string[]{"I got some\nbig ol' trash\nbags!",
                                                                                      "Now I can get\nme some more\nnickels."}, 2)),
                                            ControllingPlayer);
                }
                if (shopItems[(int)items.harassClerk].WasActivated)
                {   //harass Clerk
                    #region SUCCESS
                    if (random.Next(0, 100 ) < Player.DATA.stat[2] * 100)
                    {
                        //Clerk gets pissed and calls manager
                        //given choice to leave or stay and face the bossman?
                        Player.energy += 10 * Player.DATA.stat[4];
                        Player.wantedLevel += 1;
                        ScreenManager.AddScreen(new Backdrop(txBackground,
                                                             new StoryScreen(content.Load<Texture2D>("animations/clerkAnim"), 8, 111, 175, true, 0,
                                                                             new string[]{"I don't think\nthat's very\nfunny.",
                                                                                          "Please leave.",
                                                                                          "(The clerk\nglares at you\nintensely)"}, 3)),
                                                ControllingPlayer);
                    }
                    #endregion
                    #region FAIL
                    else
                    {
                        //Clerk pwns you with comeback
                        //play comeback(s).wav?
                        Player.energy -= 10 * Player.DATA.stat[4];
                        ScreenManager.AddScreen(new Backdrop(txBackground,
                                                             new StoryScreen(content.Load<Texture2D>("animations/cryingClerk"), 4, 249, 357, true, 0,
                                                                             new string[]{"You hurt my\nfeelings, you\nbig meanie.",
                                                                                          "(The clerk\nruns away\nand cries in\nthe corner for\na while)"}, 2)),
                                                ControllingPlayer);
                    }
                    #endregion
                }
                if (shopItems[(int)items.stealSaltines].WasActivated)
                {   //steal Crackers
                    #region SUCCESS
                    if ((random.Next(0, 100) < Player.DATA.stat[3] * 100) &&
                        (random.Next(0, 100) < Player.DATA.stat[4] * 100))
                    {
                        Player.energy += 5;                
                        Player.intoxicationLevel -= 5;
                        ScreenManager.AddScreen(new Backdrop(txBackground,
                                                             new StoryScreen(content.Load<Texture2D>("animations/cryingClerk"), 4, 250, 357, true, 0,
                                                                             new string[]{"That was my\nlunch...",
                                                                                          "You suck!"}, 2)),
                                                ControllingPlayer);
                    }
                    #endregion
                    #region FAIL
                    else
                    {
                        Player.wantedLevel += 0.25f;
                        Player.energy -= 5 * Player.DATA.stat[4];
                    }
                    #endregion
                }
                if (shopItems[(int)items.stealCart].WasActivated)
                {   //steal Cart
                    #region SUCCESS
                    if ((random.Next(0, 100) < Player.DATA.stat[3] * 100) &&
                        (random.Next(0, 100) < Player.DATA.stat[4] * 100))
                    {
                        //Add Cart to Player Inventory
                        Player.carryingCapacity = 150;
                        Player.energy -= 15;
                        ScreenManager.AddScreen(new Backdrop(txBackground,
                                                             new StoryScreen(content.Load<Texture2D>("animations/hobo" + Player.hoboNumber + "Anim"), 8, 223, 267, true, 0,
                                                                             new string[]{"I stole a cart!",
                                                                                          "This thing will\ncarry a ton of\nbottles and cans.",
                                                                                          "Now I can fulfill\nmy dreams.\n\n*Grins*"}, 3)),
                                                ControllingPlayer);
                    }
                    #endregion
                    #region FAIL
                    else
                    {
                        Player.wantedLevel += 2;
                        Player.energy -= 7.5f;
                    }
                    #endregion
                }
                if (shopItems[(int)items.exit].WasActivated)
                {
                    if (ScreenManager.GlobalOptions.SOUND_ENABLE)
                        MediaPlayer.Stop();
                    //Refill bottles before exiting
                    if (Player.INVENTORY.BOTTLES.Count == 0)
                    {
                        Player.INVENTORY.Fill(Player.carryingCapacity);
                        Player.energy *= 0.75f;
                        Player.odorLevel += 5;
                    }
                    else
                    {
                        Player.energy -= 2.50f;
                    }
                    ScreenManager.AddScreen(new GameplayScreen(Player), ControllingPlayer);
                    ExitScreen();
                }
                #endregion
                Player.Update(gameTime);
                shopHUD.Update(Player.intoxicationLevel, Player.wantedLevel, Player.energy, Player.odorLevel);
                shopBLUR.Update(gameTime, fadeSpeed_X, fadeSpeed_Y, Player.intoxicationLevel,
                                Player.DATA.stat[0]);

                //Hide trash bags?
                if (Player.carryingCapacity >= 50)
                    shopItems[(int)items.trashBags].Hide();
                else
                    shopItems[(int)items.trashBags].Show();
                //Hide steal cart?
                if (Player.carryingCapacity >= 150)
                    shopItems[(int)items.stealCart].Hide();
                else
                    shopItems[(int)items.stealCart].Show();

                HandleInput();
            }
        }

        /// <summary>
        /// Let the user do stuff...
        /// </summary>
        public override void HandleInput()
        {
            if (input == null)
            {
                Mouse.WindowHandle = base.ScreenManager.Game.Window.Handle;
                input = new InputHandler(GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), Mouse.GetState());
                input.Viewport = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
            }
            else
            {
                Mouse.WindowHandle = base.ScreenManager.Game.Window.Handle;
                Mouse.SetPosition((int)MathHelper.Clamp(Mouse.GetState().X, -10, ScreenManager.GraphicsDevice.Viewport.Width + 10), (int)MathHelper.Clamp(Mouse.GetState().Y, -20, ScreenManager.GraphicsDevice.Viewport.Height + 10));
                input.Update(GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), Mouse.GetState());
            }

            IsPaused = (input.BackPress || input.StartPress || input.EscPress);

            if (IsPaused)
            {
                ScreenManager.AddScreen(new Backdrop(txBackground, new PauseMenuScreen()), ControllingPlayer);
            }
            else if (input.LeftMBDown)
            {
                foreach (Buyable btn in shopItems)
                    btn.ClickDown(input.Mouse);
            }
            else if (input.LeftMBUp)
            {
                foreach (Buyable btn in shopItems)
                    btn.ClickUp(input.Mouse);
            }
            else if (input.APress || input.LTPress || input.RTPress)
            {
                foreach (Buyable btn in shopItems)
                {
                    btn.ClickDown(input.Mouse);
                    btn.ClickUp(input.Mouse);
                }
            }
            else if (input.SpacePress || input.LBPress)
            {
                Player.cash += 3.05m;
                Player.wantedLevel++;
            }

            if (input != null)
            {
                shopBLUR.setPos((int)(input.Mouse.X - (mouseCursor.Width / 2)), (int)(input.Mouse.Y - (mouseCursor.Height / 2)),
                                 fadeSpeed_X, fadeSpeed_Y);
            }
        }

        /// <summary>
        /// Show stuff on the screen...
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(txBackground, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Height), Color.White);

            clerk.Draw(spriteBatch);

            foreach (Buyable item in shopItems)
                item.Draw(spriteBatch);

            //Cursor
            if (input != null)
                spriteBatch.Draw(txCursor, new Vector2(input.Mouse.X - 5, input.Mouse.Y - 3), null, Color.White, 0, new Vector2(0, 0), .5f, SpriteEffects.None, 0);

            shopBLUR.Draw(spriteBatch);

            voucher.Draw(spriteBatch);
            spriteBatch.DrawString(fntTitle, "$" + Player.cash, new Vector2(130 - fntTitle.MeasureString("$" + Player.cash).X, 47), Color.Black);
            shopHUD.Draw(spriteBatch, fntTitle);
            
            //Info text
            foreach (Buyable item in shopItems)
            {
                if (input != null)
                {
                    if (item.MouseHit(input.Mouse))
                    {   //item title
                        spriteBatch.Draw(blackout, new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 2 - ((int)fntTitle.MeasureString(item.InfoTitle).X / 2 + 10), 600, (int)fntTitle.MeasureString(item.InfoTitle).X + 20, 30), Color.White);
                        spriteBatch.DrawString(fntTitle, item.InfoTitle, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - (int)fntTitle.MeasureString(item.InfoTitle).X / 2, 603), Color.DarkCyan);
                        //description
                        spriteBatch.Draw(blackout, new Rectangle(425, 634, 430, 81), Color.White);
                        spriteBatch.DrawString(fntInfo, item.InfoText, new Vector2(435, 637), Color.LightYellow);
                    }
                }
            }

            #region DEBUGGING STUFF
            if (ScreenManager.GlobalOptions.DEBUG_TEXT && input != null)
            {
                spriteBatch.DrawString(ScreenManager.Font[1], "Mouse X: " + input.Mouse.X + "\nMouse Y: " + input.Mouse.Y, Vector2.Zero, Color.DarkTurquoise);
                spriteBatch.Draw(mouseCursor, new Vector2(input.Mouse.X - mouseCursor.Width / 2, input.Mouse.Y - mouseCursor.Height / 2), Color.White);

                spriteBatch.DrawString(ScreenManager.Font[1], "" + Player.carryingCapacity + " " + shopItems[(int)items.trashBags].IsVisible, new Vector2(2, 70), Color.DarkTurquoise);

            }
            #endregion
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        #endregion
    }
}
