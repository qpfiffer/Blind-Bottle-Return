using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace bottleReturn
{
    public class characterLIST
    {
        public List<character> CHARACTERS;
        public int listSize = 4;

        /// <summary>
        /// Creates a new List of Availiable Characters for use by the player.
        /// </summary>
        /// <param name="texture1"></param>
        /// <param name="texture2"></param>
        /// <param name="texture3"></param>
        /// <param name="texture4"></param>
        public characterLIST(Texture2D texture1, Texture2D texture2,
                                Texture2D texture3, Texture2D texture4)
        {
            CHARACTERS = new List<character>();
            character[] temp = new character[listSize];

            temp[0] = new character(0.375f, 0.80f, 0.40f, 0.65f, 12, 0.85f, 0.90f, "Muddy Mike",
                                        0.70f, 0.25f, 0.35f, texture1);
            temp[1] = new character(0.500f, 0.60f, 0.80f, 0.15f, 18, 0.75f, 0.50f, "John Doe",
                                        0.50f, 0.55f, 0.60f, texture2);
            temp[2] = new character(0.675f, 0.40f, 0.20f, 0.40f, 16, 0.60f, 0.25f, "Bob",
                                        0.10f, 0.90f, 0.25f, texture3);
            temp[3] = new character(0.755f, 0.20f, 0.60f, 0.90f, 20, 0.50f, 0.75f, "Dirty Sanchez",
                                        0.25f, 0.75f, 0.80f, texture4);

            for (int index = 0; index < listSize; index++)
            {
                CHARACTERS.Add(temp[index]);
            }
        }
    }
    public class character
    {
        #region CHARACTER DYNAMIC VARIABLES AND ATTRIBUTES
        public charactersData DATA;
        public characterVisuals VISUALS;
        public returnablesLIST INVENTORY;
            //STATS VARIABLES
            //These variables will hold the values for how drunk, how much energy he/she has,  
            //and how smelly the character is at a specific point in the game.
            //These values will change as the game is played, as opposed to the
            //values of the vairbales held in the charactersData Class, which are used
            //as constants to alter these values accordingly.
        public int intoxicationLevel;     //(will range between 1 and 100, initialized to 15 due to hangover)
        public float odorLevel;             //(will range between 1 and 100, initialized to 25 due to lack of shower)
        public float energy;                //(will range between 1 and 100, initialized to 100)
        public float wantedLevel;           //(will range between 1 and 10, initiaized to 0)
        public int carryingCapacity;        
        public decimal cash;                  //(initialized to 0, bums are broke)
        public int hoboNumber;
        public List<ticket> VOUCHERS;       //(List to hold bottle return vouchers)
        public double ticketsValue;
        public Texture2D face;
        public Int32 score;
        private int scoreThreshold;

        #endregion

        /// <summary>
        /// This function creates a new character for use by the player, based on the
        /// parameters passed in from the "createCharacter" function in the
        /// "characterSelection" class.
        /// </summary>
        /// <param name="alcoholTolerance"></param>
        /// <param name="bodyOdor"></param>
        /// <param name="temperment"></param>
        /// <param name="courage"></param>
        /// <param name="speed"></param>
        /// <param name="clumsiness"></param>
        /// <param name="radial"></param>
        /// <param name="fuzz"></param>
        /// <param name="shaking"></param>
        /// <param name="handTexture"></param>
        public character(float alcoholTolerance, float bodyOdor, float temperment, float courage, 
                            int carryingCapacity, float speed, float clumsiness, string name, float radial,
                            float fuzz, float shaking, Texture2D portrait)
        {
            DATA = new charactersData(alcoholTolerance, bodyOdor, temperment, courage, speed, clumsiness, name);
            VISUALS = new characterVisuals(radial, fuzz, shaking);
            INVENTORY = new returnablesLIST(carryingCapacity);
            VOUCHERS = new List<ticket>();

            intoxicationLevel = 15;
            odorLevel = 25;
            energy = 100;
            wantedLevel = 1;

            cash = 0;
            this.carryingCapacity = carryingCapacity;
            face = portrait;
            ticketsValue = 0;
            score = 0;
            scoreThreshold = 15000;
        }

        public void Update(GameTime gameTime)
        {
            if (intoxicationLevel > 100)
            {
                intoxicationLevel = 100;
                //game ends...player dies of alcohol poisoning
            }
            if (intoxicationLevel < 1)
                intoxicationLevel = 1;
            if (energy < 1)
            {
                energy = 1;
                //game ends...player dies of hunger
            }
            if (energy > 100)
                energy = 100;
            if (wantedLevel > 10)
                wantedLevel = 10;
            if (wantedLevel < 1)
                wantedLevel = 1;
            if (odorLevel > 100)
                odorLevel = 100;

            ticketsValue = 0;
            foreach (ticket index in VOUCHERS)
            {
                ticketsValue += index.value;
            }

            if (score >= scoreThreshold)
            {
                wantedLevel -= 1;
                scoreThreshold += scoreThreshold / 2;
            }
        }
    }
    class ticketLIST : Microsoft.Xna.Framework.Game
    {
        public List<ticket> TICKETS;
        Random random = new Random();

        /// <summary>
        /// Create a new List of Tickets for the Player
        /// </summary>
        public ticketLIST()
        {
            TICKETS = new List<ticket>();

            //Add 1 ticket with random value from $.05 to $1.00
            //Leftover from previous day's work.
            ticket temp = new ticket(0.05f * random.Next(1, 20));
            TICKETS.Add(temp);
        }
    }
    public class ticket : Microsoft.Xna.Framework.Game
    {
        public double value;

        /// <summary>
        /// Create a New Ticket with the Specified Value
        /// </summary>
        /// <param name="value">Value of Ticket: must be multiple of 0.05</param>
        public ticket(double value)
        {
            this.value = value;
        }
    }
}
