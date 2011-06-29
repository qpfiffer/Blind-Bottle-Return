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
    public class characterVisuals : Microsoft.Xna.Framework.Game
    {
        public float[] stat = new float[3];

        /// <summary>
        /// Class for Visual Elements for Characters
        /// </summary>
        /// <param name="radial"> "Tunnel Vision" Overlay </param>
        /// <param name="fuzz"> Blur Overlay </param>
        /// <param name="shaking"> </param>
        /// <param name="handTexture"> </param>
        public characterVisuals(float radial, float fuzz, float shaking)
        {
            stat[0] = radial;
            stat[1] = fuzz;
            stat[2] = shaking;
        }
    }
}
