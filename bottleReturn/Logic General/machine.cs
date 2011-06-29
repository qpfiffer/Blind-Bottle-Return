using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace bottleReturn
{
    public enum machineType
    {
        GLASS, PLASTIC, CANS
    }

    public class machine
    {
        Rectangle rejectHitbox;
        Rectangle machineRect;
        Rectangle hitbox;
        Rectangle ticketHitbox;
        Texture2D hitboxDebug;
        Texture2D machineTexture;
        machineType machineType;
        double arcLength1;
        double arcLength2;
        double rejectArcLength;
        double currentValue;

        bool rejectBottleOccupado = false;
        returnablesLIST.Type rejectBottleType = returnablesLIST.Type.can; // Cans for default. Why not?
        Texture2D[] rejectTextures;

        SoundEffect ticketSound;
        SoundEffect machineSound;
        public SoundEffectInstance currentSound;
        float sndPanMachine;

        /// <summary>
        /// Controls the position of the machine overall, as well as its hitboxes.
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(machineRect.X, machineRect.Y); }
            set
            {
                machineRect.X = (int)value.X;
                machineRect.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// Creates a new bottle return machine
        /// </summary>
        /// <param name="pos">Position on the screen</param>
        /// <param name="texture">What texture it will use</param>
        /// <param name="myMachType">What kind of machine it is</param>
        public machine(Vector2 pos, Texture2D texture, machineType myMachType, GraphicsDevice gDevice,
                        SoundEffect machineSound, SoundEffect ticketSound, Texture2D[] rejectTextures)
        {
            this.rejectTextures = rejectTextures;
            machineTexture = texture;
            machineRect = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            machineType = myMachType;
            this.machineSound = machineSound;
            this.ticketSound = ticketSound;
            currentSound = machineSound.CreateInstance();

            hitbox = new Rectangle(0, 0, 90, 90);
            ticketHitbox = new Rectangle(0, 0, 20, 20);
            rejectHitbox = new Rectangle(0, 0, 90, 90);
            switch (machineType)
            {
                // I had no idea all the bottle machine textures were so similar
                case machineType.CANS:
                    hitbox.X = machineRect.X + 442 - (hitbox.Width / 2);
                    hitbox.Y = machineRect.Y + 244 - (hitbox.Height / 2);
                    ticketHitbox.X = machineRect.X + 258 - (ticketHitbox.Width / 2);
                    ticketHitbox.Y = machineRect.Y + 280 - (ticketHitbox.Height / 2);
                    rejectHitbox.X = machineRect.X + 258 - (ticketHitbox.Width / 2);
                    rejectHitbox.Y = machineRect.Y + 280 - (ticketHitbox.Height / 2);
                    break;
                case machineType.GLASS:
                    hitbox.X = machineRect.X + 442 - (hitbox.Width / 2);
                    hitbox.Y = machineRect.Y + 244 - (hitbox.Height / 2);
                    ticketHitbox.X = machineRect.X + 258 - (ticketHitbox.Width / 2);
                    ticketHitbox.Y = machineRect.Y + 280 - (ticketHitbox.Height / 2);
                    rejectHitbox.X = machineRect.X + 258 - (ticketHitbox.Width / 2);
                    rejectHitbox.Y = machineRect.Y + 280 - (ticketHitbox.Height / 2);
                    break;
                case machineType.PLASTIC:
                    hitbox.X = machineRect.X + 442 - (hitbox.Width / 2);
                    hitbox.Y = machineRect.Y + 244 - (hitbox.Height / 2);
                    ticketHitbox.X = machineRect.X + 258 - (ticketHitbox.Width / 2);
                    ticketHitbox.Y = machineRect.Y + 280 - (ticketHitbox.Height / 2);
                    rejectHitbox.X = machineRect.X + 258 - (ticketHitbox.Width / 2);
                    rejectHitbox.Y = machineRect.Y + 280 - (ticketHitbox.Height / 2);
                    break;
            }

            // Create an orange debug texture for the hitboxes:
            hitboxDebug = new Texture2D(gDevice, hitbox.Width, hitbox.Height);
            UInt32[] SETME = new UInt32[hitboxDebug.Width * hitboxDebug.Height];
            for (int i = 0; i < SETME.Length; i++)
            {
                SETME[i] = 0xFFFF8040;
            }
            hitboxDebug.SetData<UInt32>(SETME);
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toCopy"></param>
        public machine(machine toCopy)
        {
            machineRect = toCopy.machineRect;
            hitbox = toCopy.hitbox;
            hitboxDebug = toCopy.hitboxDebug;
            machineTexture = toCopy.machineTexture;
            machineType = toCopy.machineType;

            machineSound = toCopy.machineSound;
            currentSound = toCopy.currentSound;
        }

        public void Draw(SpriteBatch sprBatch)
        {
            sprBatch.Draw(machineTexture, machineRect, Color.White);
        }

        public void drawReject(SpriteBatch sprBatch)
        {
            if (rejectBottleOccupado)
            {
                sprBatch.Draw(rejectTextures[(int)rejectBottleType], new Vector2(rejectHitbox.X, rejectHitbox.Y), null,
                    Color.White, 0, new Vector2(0, 0), 0.50f, SpriteEffects.None, 0);
            }
        }

        public void DrawFade(SpriteBatch sprBatch)
        {
            sprBatch.Draw(machineTexture, machineRect, Color.White * 0.6f);
        }

        public void DrawHitbox(SpriteBatch sprBatch)
        {
            sprBatch.Draw(hitboxDebug, hitbox, Color.White);
            sprBatch.Draw(hitboxDebug, ticketHitbox, Color.White);
            sprBatch.Draw(hitboxDebug, rejectHitbox, Color.White);
        }

        public void Update(GameTime gTime, float scale, int index, float rotation, Rectangle backGround)
        {

            // Update the hitbox positions:
            arcLength1 = (rotation * (hitbox.X - backGround.X));
            arcLength2 = (rotation * (ticketHitbox.X - backGround.X));
            rejectArcLength = (rotation * (rejectHitbox.X - backGround.X));

            hitbox.Width = (int)(90 * scale);
            hitbox.Height = (int)(90 * scale);

            hitbox.X = (int)(backGround.X + (index * 628) + 442 - (hitbox.Width / 2) - 0.5 * (arcLength1 / Math.PI));
            hitbox.Y = (int)(backGround.Y + 244 - (hitbox.Height / 2) + arcLength1);
            ticketHitbox.X = (int)(backGround.X + (index * 628) + 258 - (ticketHitbox.Width / 2) - 0.5 * (arcLength2 / Math.PI));
            ticketHitbox.Y = (int)(backGround.Y + 280 - (ticketHitbox.Height / 2) + arcLength2);
            rejectHitbox.X = (int)(backGround.X + (index * 628) + 535 - (rejectHitbox.Width / 2) - 0.5 * (rejectArcLength / Math.PI));
            rejectHitbox.Y = (int)(backGround.Y + 567 - (rejectHitbox.Height / 2) + rejectArcLength);

            this.sndPanMachine = ((float)(machineRect.X + (machineRect.Width / 2) - 520) / 520);
            this.sndPanMachine = MathHelper.Clamp(sndPanMachine, -1.0f, 1.0f);
            currentSound.Pan = sndPanMachine;
        }

        public bool CheckCollisions(GameTime gTime, GameplayScreen screen, character Player, Vector2 mousePos,
                                        GameplayScreen.playerHand hand)
        {
            #region MACHINE HIT
            if (mouseIsInside(mousePos, hitbox) && currentSound.State != SoundState.Playing && !hand.isEmpty)
            {
                if (this.machineType == global::bottleReturn.machineType.CANS)
                {
                    if (Player.INVENTORY.BOTTLES[0].type == returnablesLIST.Type.can)
                    {
                        Player.INVENTORY.BOTTLES.RemoveAt(0);       // Remove Current Bottle From Player's Inventory
                        if (screen.ScreenManager.GlobalOptions.SOUND_ENABLE)
                        {
                            currentSound.Play();
                        }
                        currentValue += .05;
                        hand.isEmpty = true;
                        //SCORE MARKER
                        Player.score += 5 * ((int)Player.intoxicationLevel + (int)Player.wantedLevel + (int)Player.odorLevel);
                        Player.energy -= 0.75f * (Player.odorLevel / 100);
                        Player.odorLevel += 0.25f;
                    }
                    else
                    {
                        rejectBottleOccupado = true;
                        rejectBottleType = Player.INVENTORY.BOTTLES[0].type;
                        hand.isEmpty = true;
                        Player.INVENTORY.BOTTLES.RemoveAt(0);
                        Player.energy -= 0.375f * (Player.odorLevel / 100);
                        Player.odorLevel += 0.25f;
                        // Play bottle reject sound
                    }
                }
                if (this.machineType == global::bottleReturn.machineType.GLASS)
                {
                    if (Player.INVENTORY.BOTTLES[0].type == returnablesLIST.Type.glass)
                    {
                        Player.INVENTORY.BOTTLES.RemoveAt(0);       // Remove Current Bottle From Player's Inventory
                        if (screen.ScreenManager.GlobalOptions.SOUND_ENABLE)
                        {
                            currentSound.Play();
                        }
                        currentValue += .05;
                        hand.isEmpty = true;
                        //SCORE MARKER
                        Player.score += 5 * ((int)Player.intoxicationLevel + (int)Player.wantedLevel);
                        Player.energy -= 0.75f * (Player.odorLevel / 100);
                        Player.odorLevel += 0.50f;
                    }
                    else
                    {
                        rejectBottleOccupado = true;
                        rejectBottleType = Player.INVENTORY.BOTTLES[0].type;
                        hand.isEmpty = true;
                        Player.INVENTORY.BOTTLES.RemoveAt(0);
                        Player.energy -= 0.375f * (Player.odorLevel / 100);
                        Player.odorLevel += 0.50f;
                        // Play bottle reject sound
                    }
                }
                if (this.machineType == global::bottleReturn.machineType.PLASTIC)
                {
                    if (Player.INVENTORY.BOTTLES[0].type == returnablesLIST.Type.plastic)
                    {
                        Player.INVENTORY.BOTTLES.RemoveAt(0);       // Remove Current Bottle From Player's Inventory
                        if (screen.ScreenManager.GlobalOptions.SOUND_ENABLE)
                        {
                            currentSound.Play();
                        }
                        currentValue += .05;
                        hand.isEmpty = true;
                        //SCORE MARKER
                        Player.score += 5 * ((int)Player.intoxicationLevel + (int)Player.wantedLevel);
                        Player.energy -= 0.75f * (Player.odorLevel / 100);
                        Player.odorLevel += 0.50f;
                    }
                    else
                    {
                        rejectBottleOccupado = true;
                        rejectBottleType = Player.INVENTORY.BOTTLES[0].type;
                        hand.isEmpty = true;
                        Player.INVENTORY.BOTTLES.RemoveAt(0);
                        Player.energy -= 0.375f * (Player.odorLevel / 100);
                        Player.odorLevel += 0.50f;
                        // Play bottle reject sound
                    }
                }
                return true;
            }
            #endregion
            #region TICKET HIT
            if (mouseIsInside(mousePos, ticketHitbox) && currentValue != 0 && hand.isEmpty)
            {
                Player.VOUCHERS.Add(new ticket(currentValue));
                if (screen.ScreenManager.GlobalOptions.SOUND_ENABLE)
                {
                    ticketSound.Play();
                }
                currentValue = 0;
                //SCORE MARKER
                Player.score += 10 * ((int)Player.intoxicationLevel + (int)Player.wantedLevel);
                Player.energy -= 0.10f * (int)(Player.odorLevel / 100);
                Player.odorLevel += 0.125f;
                return true;
            }
            #endregion
            #region BOTTLE REJECT HIT
            if (mouseIsInside(mousePos, rejectHitbox) && hand.isEmpty)
            {
                // Picking a bottle back up.
                rejectBottleOccupado = false;
                Player.INVENTORY.BOTTLES.Add(new returnableItem(rejectBottleType));
                //SCORE MARKER
                Player.score += 2 * ((int)Player.intoxicationLevel + (int)Player.wantedLevel);
                Player.energy -= 0.25f * (int)(Player.odorLevel / 100);
                Player.odorLevel += 0.125f;
            }
            #endregion
            #region MISS
            // If none of the above happen, we will always return false.
            if (!hand.isEmpty)
                return false;
            #endregion
            // If we got through all of the above, then our
            // hand is empty and we are just clicking randomly.
            return true;
        }

        bool mouseIsInside(Vector2 mousePos, Rectangle rect)
        {
            return (mousePos.X > rect.Left && mousePos.X < rect.Right
                    && mousePos.Y > rect.Top && mousePos.Y < rect.Bottom);
        }

    }
}
