#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sprites;
using ButtonSprites;
#endregion

namespace bottleReturn
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        #region sub-Classes
        public class playerHand : Sprite
        {
            public bool isEmpty;
            int offset;

            public playerHand(Texture2D texture, Vector2 pos):
                base(texture, pos)
            {
                isEmpty = true;
                this.mImage = texture;
                this.mPosition.X = pos.X;
                this.mPosition.Y = pos.Y;
            }

            public void Update(GameTime gameTime, Vector2 pos)
            {
                this.mPosition.X = (int)(pos.X - offset);
                this.mPosition.Y = pos.Y;
            }

            public void switchType(Texture2D texture, int align_offset)
            {
                this.offset = align_offset;
                this.mImage = texture;
                this.mPosition.X = (int)(mPosition.X - offset);
                this.mPosition.Y = mPosition.Y;
            }

            public void DrawFade(SpriteBatch sprBatch)
            {
                sprBatch.Draw(this.mImage, this.mPosition, Color.White * 0.65f);
            }
        }
        #endregion

        #region Fields
        gameplayHUD HUD;
        blur BLUR;
        ContentManager content;
        SpriteFont gameFont;
        returnablesLIST.Type currentType;
        bottleBackground bottleCompoBG;
        bottleBackground bottleCompoBG_fade;
        long confrontationInterval;
        long confrontationTimer;

        Random random = new Random();

        float pauseAlpha;
            //Fields for Mouse, Cursor, and Hands
        Texture2D blue;             //Debug-Texture
        Color debugMouseAlpha;
        Texture2D mouseCursor;
        InputHandler input;
        int snapshot;               // This is an update count for hand. Update every 20 'update' calls.
        playerHand HAND;
        playerHand HAND2;           //Hand for 2nd Blurred Image
        Vector2 handPosition1;
        Vector2 handPosition2;
        Texture2D hand_empty;
        Texture2D hand_glass;
        Texture2D hand_plastic;
        Texture2D hand_can;
            
            //Fields for Bottle Machines
        double backgroundScale;
        float hitboxScale;
        float rotation;
        float rotationRadians;
        double machineFadeSpeed_X;
        double machineFadeSpeed_Y;
        Vector2 rotationDirection;
        Vector2 fadeDirection;

            //Fields for HUD
        Texture2D HUDbackground1;
        Texture2D HUDbackground2;
        Texture2D powerBar1;
        Texture2D powerBar2;
        Texture2D powerBar3;
        Texture2D smallBeer;
        Texture2D paperBag;
        Texture2D trashBag;
        Texture2D cart;

        Button btnShop;
        character Player;
        long elapsed;
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(character player)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            input = new InputHandler(GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), Mouse.GetState());
            debugMouseAlpha = new Color(255, 255, 255, (byte)0.5);
            Player = player;
            rotation = 0;
            rotationDirection = new Vector2(-1 , 0);
            fadeDirection = new Vector2(1 , 1);
            backgroundScale = 1.25;
            hitboxScale = MathHelper.Clamp((1 - (Player.intoxicationLevel / 100)), 0.40f, 1.00f);
            machineFadeSpeed_X = 1;
            machineFadeSpeed_Y = 1;
            confrontationInterval = random.Next(30000, 300000);
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            // Should speed up load times:
            content = ScreenManager.Game.Content;

            //Load Fonts
            gameFont = ScreenManager.Font[(int)ScreenManager.fontStyle.game14B];

            //Load Sounds


            //Load Content for HUD
            powerBar1 = content.Load<Texture2D>("gamePlayScreen/HUD/health_bar1");
            powerBar2 = content.Load<Texture2D>("gamePlayScreen/HUD/health_bar2");
            powerBar3 = content.Load<Texture2D>("gamePlayScreen/HUD/health_bar3");
            smallBeer = content.Load<Texture2D>("gamePlayScreen/HUD/corona");
            paperBag = content.Load<Texture2D>("gamePlayScreen/HUD/paperBag");
            trashBag = content.Load<Texture2D>("gamePlayScreen/HUD/trashBag");
            cart = content.Load<Texture2D>("gamePlayScreen/HUD/cart");
            HUDbackground1 = content.Load<Texture2D>("gamePlayScreen/HUD/horizontalBar");
            HUDbackground2 = content.Load<Texture2D>("gamePlayScreen/HUD/verticalBar");
            HUD = new gameplayHUD(HUDbackground1, HUDbackground2, powerBar1, powerBar2, powerBar3, smallBeer,
                        Player.face, paperBag, trashBag, cart,
                        gameFont, ScreenManager.GlobalOptions.SCREEN_WIDTH,
                        ScreenManager.GlobalOptions.SCREEN_HEIGHT, Player);

            //Load Content for Mouse, Cursor, and Hands
            mouseCursor = content.Load<Texture2D>("cursor");
            input.Viewport = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
                // Create a debug texture for the mouses hitbox.
            blue = new Texture2D(ScreenManager.MyDevice, mouseCursor.Width, mouseCursor.Height, false, SurfaceFormat.Color);
            Int32[] toSet = new Int32[mouseCursor.Width * mouseCursor.Height];
            for (int i = 0; i < (mouseCursor.Width * mouseCursor.Height); i++)
            {
                // Cornflower blue.
                toSet[i] = 0x6495ED;
            }
            blue.SetData<Int32>(toSet);
            
            hand_empty = content.Load<Texture2D>("gamePlayScreen/Hands/hand-point");
            hand_glass = content.Load<Texture2D>("gamePlayScreen/Hands/hand-glass");
            hand_plastic = content.Load<Texture2D>("gamePlayScreen/Hands/hand-plastic");
            hand_can = content.Load<Texture2D>("gamePlayScreen/Hands/hand-can");

            handPosition1 = new Vector2(input.Mouse.X, input.Mouse.Y);
            handPosition2 = new Vector2(input.Mouse.X, input.Mouse.Y);
            HAND = new playerHand(hand_empty, handPosition1);
            HAND2 = new playerHand(hand_empty, handPosition2);

            //Load Content for BLUR Visuals
            BLUR = new blur(1040, 580, new Vector2(0, 70), ScreenManager.GraphicsDevice,
                    Player.VISUALS.stat[0], Player.VISUALS.stat[1]);

            //Load Content for Machines
            bottleCompoBG = new bottleBackground(3,
                                new Vector2((int)(base.ScreenManager.GlobalOptions.SCREEN_WIDTH * backgroundScale),
                                (int)(base.ScreenManager.GlobalOptions.SCREEN_HEIGHT * backgroundScale)));
            bottleCompoBG.Load(this.content, ScreenManager.MyDevice, false);
            bottleCompoBG_fade = new bottleBackground(bottleCompoBG);
            bottleCompoBG_fade.Load(this.content, ScreenManager.MyDevice, true);

            //Create button(s)
            btnShop = new Button(new Vector2(1060, 675), content.Load<Texture2D>("gamePlayScreen/shopButton"), content.Load<Texture2D>("gamePlayScreen/shopButtonMO"), content.Load<Texture2D>("gamePlayScreen/shopButton"));
            btnShop.Hide();

            ScreenManager.Game.ResetElapsedTime();
            Mouse.WindowHandle = base.ScreenManager.Game.Window.Handle;
            input.Viewport = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            //content.Unload();
        }
        #endregion

        #region Update and Draw
        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // TODO: Ugly as fuck
            if (Player.energy <= 1)
            {
                ScreenManager.AddScreen(new GameOver(Player.score), ControllingPlayer);
            }

            elapsed = gameTime.ElapsedGameTime.Milliseconds;

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // Scale the hitboxse according to how drunk you are:
                hitboxScale = MathHelper.Clamp((1 - (Player.intoxicationLevel / 100)), 0.40f, 1.00f);
                // Make sure the machines know how to draw themselves:
                bottleCompoBG.Update(gameTime, this, hitboxScale, rotationRadians, Player,
                                    (float)machineFadeSpeed_X, (float)machineFadeSpeed_Y);
                bottleCompoBG.CheckCollisions(gameTime, this, Player, (input.APress || input.LeftMBDown), input.Mouse, HAND, HAND2);

                Player.Update(gameTime);
                HUD.Update(gameTime);
                UpdateRotation(gameTime);

                HandleInput();

                UpdateHands(gameTime);

                if (Player.INVENTORY.BOTTLES.Count == 0)
                {
                    //check for MouseHit to activate highlighted image
                    if (input != null)
                    {
                        btnShop.MouseHit(input.Mouse);
                    }
                    btnShop.Show();
                }
                else
                    btnShop.Hide();

                if (btnShop.WasActivated)
                {
                    //add vouchers to total purse
                    while (Player.VOUCHERS.Count > 0)
                    {
                        Player.cash += (decimal)Player.VOUCHERS[0].value;
                        Player.VOUCHERS.Remove(Player.VOUCHERS[0]);
                    }
                    // Bring up shop screen
                    ScreenManager.AddScreen(new ShopScreen(Player, machineFadeSpeed_X, machineFadeSpeed_Y), ControllingPlayer);
                    this.ExitScreen();
                }

                #region Confrontations
                confrontationTimer += elapsed;
                if (confrontationTimer > confrontationInterval)
                {
                    switch (random.Next(102) % 3)
                    {
                        case 0:
                            #region Segway cop
                            switch ((int)Player.wantedLevel)
                            {
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                    {
                                        ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/copAnim"), 17, 118, 251, false, 1,
                                                                                new string[]{"",
                                                                                     "Excuse me you are\ntroubling other\ncustomers.",
                                                                                     "Can you please\nhurry up and\nfinish?"}, 3), ControllingPlayer);
                                    }
                                    break;
                                case 5:
                                case 6:
                                    {
                                        ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/copAnim"), 17, 118, 251, false, 1,
                                                                                new string[]{"",
                                                                                     "I have gotten more\nreports that you\nhave been making\na scene.",
                                                                                     "Please behave yourself\nand finish quickly."}, 3), ControllingPlayer);
                                    }
                                    break;
                                case 7:
                                    {
                                        if (Player.carryingCapacity == 150)
                                        {
                                            ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/copAnim"), 17, 118, 251, false, 1,
                                                                                new string[]{"",
                                                                                     "I warned you Already!\nI'm taking any property \nyou may have stolen.",
                                                                                     "Next time...you're tickets are MINE!"}, 3), ControllingPlayer);
                                                //Reset Player Carrying Capacity
                                            Player.carryingCapacity = 20;
                                        }
                                        else
                                        {
                                            ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/copAnim"), 17, 118, 251, false, 1,
                                                                                new string[]{"",
                                                                                     "This is your \nlast warning.",
                                                                                     "Next time...you're\ntickets are MINE!"}, 3), ControllingPlayer);
                                        }
                                    }
                                    break;
                                case 8:
                                    {
                                        ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/copAnim"), 17, 118, 251, false, 1,
                                                                                new string[]{"",
                                                                                     "Sir!",
                                                                                     "I am confiscating all\nof the money you\nhave received today!"}, 3), ControllingPlayer);
                                        //delete tickets
                                        Player.VOUCHERS.Clear();
                                        Player.ticketsValue = 0;
                                    }
                                    break;
                                case 9:
                                    {
                                        ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/copAnim"), 17, 118, 251, false, 1,
                                                                                new string[]{"",
                                                                                     "Now you have\ndone it sir!",
                                                                                     "I am confiscating all\nof the money you\nhave as evidence!"}, 3), ControllingPlayer);
                                        Player.cash = 0;
                                    }
                                    break;
                                case 10:
                                    ScreenManager.AddScreen(new GameOver(Player.score), ControllingPlayer);
                                    ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/copAnim"), 17, 118, 251, false, 1,
                                                                                new string[]{"",
                                                                                     "THATS IT YOU BUM!",
                                                                                     "DEAD OR ALIVE,\nYOU'RE COMING\nWITH ME!"}, 3), ControllingPlayer);
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                            confrontationTimer = 0;
                            confrontationInterval = random.Next(30000, 300000);
                            break;

                        case 1:
                            #region Old lady
                            switch ((int)(Player.odorLevel / 10))
                            {
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                    {
                                        ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/Old-ladyAnim"), 1, 280, 296, false, 2,
                                                                                new string[] { "Sir are you going\nto be done any\ntime soon?" }, 1),
                                                                                ControllingPlayer);
                                    }
                                    break;
                                case 5:
                                case 6:
                                    {
                                        ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/Old-ladyAnim"), 1, 280, 296, false, 2,
                                                                                new string[] { "Ohh dear what is\nthat I am smelling?",
                                                                                "It smells atrocious in here!"}, 2),
                                                                                ControllingPlayer);
                                    }
                                    break;
                                case 7:
                                case 8:
                                    {
                                        ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/Old-ladyAnim"), 1, 280, 296, false, 2,
                                                                                new string[] { "Deary me.", 
                                                                                "You know sir that you\nneed a bath really bad.", 
                                                                                "Is there any way you\ncan go take a shower in\nthe local pond?" }, 3),
                                                                                ControllingPlayer);
                                    }
                                    break;
                                case 9:
                                case 10:
                                    {
                                        ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/Old-ladyAnim"), 1, 280, 296, false, 2,
                                                                                new string[] { "If you don't take a\nshower then I am going\nto have to report you\nto security!"
                                                                                }, 1), ControllingPlayer);
                                        Player.wantedLevel++;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                            confrontationTimer = 0;
                            confrontationInterval = random.Next(30000, 300000);
                            break;

                        case 2:
                        default:
                            #region madboy
                            ScreenManager.AddScreen(new StoryScreen(content.Load<Texture2D>("animations/madboyAnim"), 1, 195, 336, false, 1,
                                                                    new string[] { "Shh, I am going to\nharrass the clerk and\nmake the security chase\nafter me.",
                                                                    "That is a bad idea?\nWhy?",
                                                                    "Pfft you are no fun\nbut I'll draw the\nattention of the\ncops away from\nyou anyway." }, 3), ControllingPlayer);
                            //add effect
                            Player.wantedLevel = Math.Max(Player.wantedLevel - 1, 1);
                            #endregion
                            confrontationTimer = 0;
                            confrontationInterval = random.Next(30000, 300000);
                            break;
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// Updates the current rotational state of the bottle-room image,
        /// as well as the double-image effect.
        /// </summary>
        /// <param name="gameTime"></param>
        void UpdateRotation(GameTime gameTime)
        {
            if (rotation > ((float)Player.intoxicationLevel / 100))
            {
                rotationDirection.X *= -1;
            }
            if (rotation < -((float)Player.intoxicationLevel / 100))
            {
                rotationDirection.X *= -1;
            }

            rotation += .010f * (rotationDirection.X * ((float)Player.intoxicationLevel / 100));

            bottleCompoBG.incPos((int)(0.015 * rotationDirection.X * (Player.DATA.stat[5] * 100)), 0);
            bottleCompoBG_fade.incPos((int)(0.015 * rotationDirection.X * (Player.DATA.stat[5] * 100)), 0);
            
            rotationRadians = (float)(Math.PI / 30) * rotation;

            #region FADE CONTROL FOR COMPOSITE BACKGROUND
            if (bottleCompoBG_fade.BgRect.X > (int)(bottleCompoBG.BgRect.X + (1.00 * Player.intoxicationLevel)))
            {
                fadeDirection.X *= -1;
            }
            if (bottleCompoBG_fade.BgRect.X < (int)(bottleCompoBG.BgRect.X - (1.00 * Player.intoxicationLevel)))
            {
                fadeDirection.X *= -1;
            }
            if (bottleCompoBG_fade.BgRect.Y > (int)(bottleCompoBG.BgRect.Y + (0.40 * Player.intoxicationLevel)))
            {
                fadeDirection.Y *= -1;
            }
            if (bottleCompoBG_fade.BgRect.Y < (int)(bottleCompoBG.BgRect.Y - (0.40 * Player.intoxicationLevel)))
            {
                fadeDirection.Y *= -1;
            }
            #endregion
            machineFadeSpeed_X = (Player.intoxicationLevel / 10) * fadeDirection.X * (1 - Player.DATA.stat[0]);
            machineFadeSpeed_Y = (Player.intoxicationLevel / 10) * fadeDirection.Y * (1 - Player.DATA.stat[0]);

            machineFadeSpeed_X = MathHelper.Clamp((float)machineFadeSpeed_X, -6.0f, 6.0f);
            machineFadeSpeed_Y = MathHelper.Clamp((float)machineFadeSpeed_Y, -6.0f, 6.0f);

            bottleCompoBG_fade.incPos((int)(0.80 * machineFadeSpeed_X), (int)(0.45 * machineFadeSpeed_Y));

            BLUR.Update(gameTime, machineFadeSpeed_X, machineFadeSpeed_Y, Player.intoxicationLevel,
                            Player.DATA.stat[0]);
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput()
        {
            if (input == null)
                throw new ArgumentNullException("input");
            else
            {
                Mouse.WindowHandle = base.ScreenManager.Game.Window.Handle;
                Mouse.SetPosition((int)MathHelper.Clamp(Mouse.GetState().X, -10, ScreenManager.GraphicsDevice.Viewport.Width + 10), (int)MathHelper.Clamp(Mouse.GetState().Y, -20, ScreenManager.GraphicsDevice.Viewport.Height + 10));
                input.Update(GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), Mouse.GetState());
            }

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool IsPaused = (input.BackPress || input.StartPress || input.EscPress);

            if (IsPaused)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else if (input.TabPress || input.YPress)
            {
                btnShop.WasActivated = true;
            }
            else if (input.LeftMBDown)
            {
                btnShop.ClickDown(input.Mouse);
            }
            else if (input.LeftMBUp)
            {
                btnShop.ClickUp(input.Mouse);
            }
            else if (input.APress)
            {
                btnShop.ClickDown(input.Mouse);
                btnShop.ClickUp(input.Mouse);
            }
            else if (input.SpacePress || input.LBPress)
            {
                Player.cash += 1.00m;
                Player.wantedLevel++;
            }
            else
            {
                BLUR.setPos((int)(input.Mouse.X - (mouseCursor.Width / 2)), (int)(input.Mouse.Y - (mouseCursor.Height / 2)),
                                machineFadeSpeed_X, machineFadeSpeed_Y);

                #region BOTTLE-MACHINE AND HITBOX MOVEMENT
                if (input.UpHold || input.Gamepad.DPad.Up == ButtonState.Pressed)
                {
                    bottleCompoBG.incPos(0, (int)(7 * Player.DATA.stat[4]));
                    bottleCompoBG_fade.incPos(0, (int)(7 * Player.DATA.stat[4]));
                }
                if (input.DownHold || input.Gamepad.DPad.Down == ButtonState.Pressed)
                {
                    bottleCompoBG.incPos(0, -(int)(7 * Player.DATA.stat[4]));
                    bottleCompoBG_fade.incPos(0, -(int)(7 * Player.DATA.stat[4]));
                }
                if (input.LeftHold || input.Gamepad.DPad.Left == ButtonState.Pressed)
                {
                    bottleCompoBG.incPos((int)(7 * Player.DATA.stat[4]), 0);
                    bottleCompoBG_fade.incPos((int)(7 * Player.DATA.stat[4]), 0);
                }
                if (input.RightHold || input.Gamepad.DPad.Right == ButtonState.Pressed)
                {
                    bottleCompoBG.incPos(-(int)(7 * Player.DATA.stat[4]), 0);
                    bottleCompoBG_fade.incPos(-(int)(7 * Player.DATA.stat[4]), 0);
                }
                #endregion

                #region MACHINE BOUNDARIES
                if (bottleCompoBG.BgRect.X > 0)
                    bottleCompoBG.setPos(0, bottleCompoBG.BgRect.Y);
                if (bottleCompoBG.BgRect.X < -844)
                    bottleCompoBG.setPos(-844, bottleCompoBG.BgRect.Y);
                if (bottleCompoBG.BgRect.Y > HUDbackground1.Height)
                    bottleCompoBG.setPos(bottleCompoBG.BgRect.X, HUDbackground1.Height);
                if (bottleCompoBG.BgRect.Y < -HUDbackground1.Height)
                    bottleCompoBG.setPos(bottleCompoBG.BgRect.X, -HUDbackground1.Height);

                if (bottleCompoBG_fade.BgRect.X > 0)
                    bottleCompoBG_fade.setPos(0, bottleCompoBG_fade.BgRect.Y);
                if (bottleCompoBG_fade.BgRect.X < -844)
                    bottleCompoBG_fade.setPos(-844, bottleCompoBG_fade.BgRect.Y);
                if (bottleCompoBG_fade.BgRect.Y > HUDbackground1.Height)
                    bottleCompoBG_fade.setPos(bottleCompoBG_fade.BgRect.X, HUDbackground1.Height);
                if (bottleCompoBG_fade.BgRect.Y < -HUDbackground1.Height)
                    bottleCompoBG_fade.setPos(bottleCompoBG_fade.BgRect.X, -HUDbackground1.Height);
                #endregion
            }
        }

        /// <summary>
        /// Updates the Texture and Position of the Hand Images
        /// </summary>
        /// <param name="gameTime"></param>
        void UpdateHands(GameTime gameTime)
        {
            if (HAND.isEmpty)
            {
                HAND.switchType(hand_empty, 8);
                HAND2.switchType(hand_empty, 8);
                if (input != null)
                {
                    if ((input.RightMBDown || input.BPress) && Player.INVENTORY.BOTTLES.Count != 0)
                    {
                        HAND.isEmpty = false;
                        currentType = Player.INVENTORY.BOTTLES[0].type;
                        switch (currentType)
                        {
                            case returnablesLIST.Type.glass:
                                HAND.switchType(hand_glass, 44);
                                HAND2.switchType(hand_glass, 44);
                                break;
                            case returnablesLIST.Type.plastic:
                                HAND.switchType(hand_plastic, 41);
                                HAND2.switchType(hand_plastic, 41);
                                break;
                            case returnablesLIST.Type.can:
                                HAND.switchType(hand_can, 39);
                                HAND2.switchType(hand_can, 39);
                                break;
                        }
                    }
                }
            }
            //Update Shaking Below
            #region SHAKING
            Vector2 Direction = new Vector2(1, 1);

            if (snapshot != 5)
            {
                snapshot++;
            } 
            else 
            {
                snapshot = 0;
                if (input != null)
                    handPosition1 = input.Mouse;
                handPosition2 = BLUR.BlurRect2q4;
            }

            handPosition1.X += (float)(Direction.X * 10 * Player.VISUALS.stat[2] * (Player.intoxicationLevel / 100));
            handPosition1.Y += (float)(Direction.Y * 10 * Player.VISUALS.stat[2] * (Player.intoxicationLevel / 100));

            handPosition2.X += (float)(Direction.X * 10 * Player.VISUALS.stat[2] * (Player.intoxicationLevel / 100));
            handPosition2.Y += (float)(Direction.Y * 10 * Player.VISUALS.stat[2] * (Player.intoxicationLevel / 100));
            #endregion

            #region CONTROL OUTER BOUNDS OF HAND
            if (handPosition1.X < 0)
                handPosition1.X = 0;
            if (handPosition1.X > ScreenManager.GlobalOptions.SCREEN_WIDTH - HUDbackground2.Width)
                handPosition1.X = ScreenManager.GlobalOptions.SCREEN_WIDTH - HUDbackground2.Width;
            if (handPosition1.Y < HUDbackground1.Height)
                handPosition1.Y = HUDbackground1.Height;
            if (handPosition1.Y > ScreenManager.GlobalOptions.SCREEN_WIDTH - HUDbackground1.Height)
                handPosition1.Y = ScreenManager.GlobalOptions.SCREEN_WIDTH - HUDbackground1.Height;
            #endregion

            HAND.Update(gameTime, handPosition1);
            HAND2.Update(gameTime, handPosition2);
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background - NOT ANYMORE!!
            ScreenManager.GraphicsDevice.Clear(new Color(50, 50, 50, 50));

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            if (ScreenManager.GlobalOptions.DEBUG_TEXT)
                bottleCompoBG.Draw(spriteBatch, rotationRadians, true);
            else
                bottleCompoBG.Draw(spriteBatch, rotationRadians, false);

            bottleCompoBG_fade.DrawFade(spriteBatch, rotationRadians);

            HAND.Draw(spriteBatch);
            HAND2.DrawFade(spriteBatch);

            bottleCompoBG.DrawDrops(spriteBatch);

            BLUR.Draw(spriteBatch);
            HUD.Draw(spriteBatch);

            btnShop.Draw(spriteBatch);

            if (ScreenManager.GlobalOptions.DEBUG_TEXT)
            {
                if (input != null)
                {
                    spriteBatch.DrawString(gameFont, "Mouse X: " + input.Mouse.X + "\nMouse Y: " + input.Mouse.Y, Vector2.Zero, Color.White);
                    if (input.MouseIsInside(ScreenManager.GlobalOptions.SCREEN_WIDTH, ScreenManager.GlobalOptions.SCREEN_HEIGHT))
                        spriteBatch.Draw(blue, input.Mouse, debugMouseAlpha);
                }
                spriteBatch.DrawString(gameFont, "Rotation: " + rotation, new Vector2(0, 630), Color.White);

                spriteBatch.DrawString(gameFont, "Main - X: " + bottleCompoBG.BgRect.X + "\nMain - Y: " + bottleCompoBG.BgRect.Y +
                            "\nMachOnePos X: " + bottleCompoBG.MachineOnePos.X + "\nMachOnePos Y: " + bottleCompoBG.MachineOnePos.Y,
                            new Vector2(1048, 612), Color.White);
                spriteBatch.DrawString(gameFont, "Hand-X: " + handPosition1.X + "\nHand-Y: " + handPosition1.Y,
                            new Vector2(1048, 564), Color.White);

                spriteBatch.DrawString(gameFont, "Pan 1: " + bottleCompoBG.getSoundPanValues(0).ToString(),
                                                new Vector2(1048, 400), Color.White);
                spriteBatch.DrawString(gameFont, "Pan 2: " + bottleCompoBG.getSoundPanValues(1).ToString(),
                                                new Vector2(1048, 422), Color.White);
                spriteBatch.DrawString(gameFont, "Pan 3: " + bottleCompoBG.getSoundPanValues(2).ToString(),
                                                new Vector2(1048, 444), Color.White);
                if (Player.INVENTORY.BOTTLES.Count != 0)
                {
                    spriteBatch.DrawString(gameFont, "CurBType: " + Player.INVENTORY.BOTTLES[0].type,
                                                new Vector2(1048, 466), Color.White);
                }
                spriteBatch.DrawString(gameFont, "" + confrontationTimer + "\n" + confrontationInterval, new Vector2(0, 70), Color.White);
            }
            else
            {
                //Draw a normal mouse during gameplay, unless debug is on.
                if (input != null)
                {
                    if (ScreenManager.GlobalOptions.CURSOR_ENABLE ||
                        (input.MouseIsInside(ScreenManager.GlobalOptions.SCREEN_WIDTH, ScreenManager.GlobalOptions.SCREEN_HEIGHT)
                         && !input.MouseIsInside(0, 63, 1036, 646)))
                    {
                        spriteBatch.Draw(mouseCursor, input.Mouse, Color.White);
                    }
                }

                
            }

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
