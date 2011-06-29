using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace bottleReturn
{
    public class charactersData : Microsoft.Xna.Framework.Game
    {
        #region CHARACTER STATIC VARIABLES
        //ATTRIBUTES:
            //Values will range between 0 and 1, and are used as constants
            //of proportionality to alter the character's stats throughout gameplay.
        public float[] stat = new float[6];
        public string name;
        #endregion

        /// <summary>
        /// Class for selectable Characters attributes.
        /// </summary>
        /// <param name="alcoholTolerance"></param>
        /// <param name="bodyOdor"></param>
        /// <param name="temperment"></param>
        /// <param name="courage"></param>
        /// <param name="carryingCapacity"></param>
        /// <param name="speed"></param>
        /// <param name="clumsiness"></param>
        public charactersData(float alcoholTolerance, float bodyOdor, float temperment, float courage, float speed,
                                float clumsiness, string name)
        {
            stat[0] = alcoholTolerance;
            stat[1] = bodyOdor;
            stat[2] = temperment;
            stat[3] = courage;
            stat[4] = speed;
            stat[5] = clumsiness;
            this.name = name;
        }
    }
}
