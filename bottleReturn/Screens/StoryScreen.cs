#region File Description
//-----------------------------------------------------------------------------
// StoryScreen.cs
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
#endregion

namespace bottleReturn
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public class StoryScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        InputHandler input;

        Texture2D spriteSheet;
        Rectangle destRect;
        Rectangle sourceRect;
        int panelWidth;
        int panelHeight;
        int frame;
        int maxFrames;
        long frameCounter;

        Texture2D[] bubble;
        Vector2[] speechOffset;
        Vector2 bubbleLoc;
        int bubbleStyle;
        string[] dialog;
        int line;
        int maxLines;
        long speechCounter;

        bool looping;
        bool firstPass;
        SpriteFont speechFont;
        float pauseAlpha;

        #endregion

        #region Initialization
        /// <summary>
        /// Constructor.
        /// </summary>
        public StoryScreen(Texture2D spritesheet, int maxframes, int panelwidth, int panelheight, bool looping,
                           int style, string[] dialog, int maxlines)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0);
            TransitionOffTime = TimeSpan.FromSeconds(0);

            spriteSheet = spritesheet;
            panelWidth = panelwidth;
            panelHeight = panelheight;
            frame = 0;
            maxFrames = maxframes;

            this.dialog = dialog;
            line = 0;
            maxLines = maxlines;
            speechOffset = new Vector2[4] { new Vector2(45, 40),
                                            new Vector2(55, 75),
                                            new Vector2(55, 75),
                                            new Vector2(50, 50) };

            this.looping = looping;
            firstPass = true;
            bubbleStyle = style;
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            bubble = new Texture2D[4] { content.Load<Texture2D>("animations/speech"),
                                        content.Load<Texture2D>("animations/speech7"),
                                        content.Load<Texture2D>("animations/speech4"),
                                        content.Load<Texture2D>("animations/speech6") };
            //Load Fonts
            speechFont = ScreenManager.Font[(int)ScreenManager.fontStyle.hoboDialog20I];

            //Load Sounds

            destRect = new Rectangle((int)(ScreenManager.GraphicsDevice.Viewport.Width * .2), (int)(ScreenManager.GraphicsDevice.Viewport.Height * .15),
                                     (int)(ScreenManager.GraphicsDevice.Viewport.Width * .35), (int)(ScreenManager.GraphicsDevice.Viewport.Height * .7));
            sourceRect = new Rectangle(0, 0, panelWidth, panelHeight);
            bubbleLoc = new Vector2(destRect.Right + 10, destRect.Top - 20);
            ScreenManager.Game.ResetElapsedTime();
            Mouse.WindowHandle = base.ScreenManager.Game.Window.Handle;
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
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

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, .1f);

            if (IsActive)
            {
                long time = gameTime.ElapsedGameTime.Milliseconds;
                frameCounter += time;
                speechCounter += time;

                if (frameCounter > 75)
                {
                    frameCounter -= 75;
                    if (looping)
                    {
                        frame = (frame + 1) % maxFrames;
                    }
                    else
                    {
                        if (firstPass)
                        {
                            frame = (frame + 1) % maxFrames;
                            if (frame == maxFrames - 1)
                                firstPass = false;
                        }
                        else
                            frame = maxFrames - 1;
                    }

                    sourceRect.X = frame * (panelWidth + 2);
                    sourceRect.Y = 0;
                }

                if (speechCounter > 2500)
                {
                    speechCounter -= 2500;
                    if (++line >= maxLines)
                    {
                        --line;
                        ExitScreen();
                    }
                }
                
                HandleInput();
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput()
        {
            if (input == null)
            {
                input = new InputHandler(GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), Mouse.GetState());
                input.Viewport = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
            }
            else
            {
                Mouse.SetPosition((int)MathHelper.Clamp(Mouse.GetState().X, -10, ScreenManager.GraphicsDevice.Viewport.Width + 10), (int)MathHelper.Clamp(Mouse.GetState().Y, -20, ScreenManager.GraphicsDevice.Viewport.Height + 10));
                input.Update(GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), Mouse.GetState());
            }

            if (input.EnterPress || input.BPress)
                ExitScreen();
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            
            spriteBatch.Begin();
            spriteBatch.Draw(spriteSheet, destRect, sourceRect, Color.White);
            spriteBatch.Draw(bubble[bubbleStyle], bubbleLoc, Color.White);
            spriteBatch.DrawString(speechFont, dialog[line], bubbleLoc + speechOffset[bubbleStyle], Color.Navy);

            if (ScreenManager.GlobalOptions.DEBUG_TEXT)
            {
                spriteBatch.DrawString(ScreenManager.Font[0], "" + looping + "\n" + frameCounter + "\n" + frame,
                                       Vector2.Zero, Color.Aquamarine);
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
