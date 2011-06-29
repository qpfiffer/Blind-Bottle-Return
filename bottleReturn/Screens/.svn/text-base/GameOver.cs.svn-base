using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace bottleReturn
{
    class GameOver : GameScreen
    {
        const string title = "GAME OVER";
        int score;

        public GameOver(int score)
        {
            this.score = score;
        }

        public override void HandleInput(InputState input)
        {
            PlayerIndex temp;
            if (input.IsNewButtonPress(Buttons.A, ControllingPlayer, out temp))
            {
                ScreenManager.Game.Exit();
            }

            if (input.IsNewKeyPress(Keys.Enter, ControllingPlayer, out temp))
            {
                ScreenManager.Game.Exit();
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font[(int)ScreenManager.fontStyle.menu23];

            spriteBatch.Begin();

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 160);
            Vector2 titleOrigin = font.MeasureString(title) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 4.0f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, title, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            titlePosition = new Vector2(titlePosition.X, 400);
            titleOrigin = font.MeasureString("Score: " + score);
            spriteBatch.DrawString(font, "Score: " + score, titlePosition, Color.Red * TransitionAlpha, 0,
                                   titleOrigin, 1.25f, SpriteEffects.None, 0);

            titlePosition = new Vector2(titlePosition.X, 440);
            titleOrigin = font.MeasureString("You're terrible!");
            spriteBatch.DrawString(font, "You're terrible!", titlePosition, Color.Red * TransitionAlpha, 0,
                                   titleOrigin, 1.0f, SpriteEffects.None, 0);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}