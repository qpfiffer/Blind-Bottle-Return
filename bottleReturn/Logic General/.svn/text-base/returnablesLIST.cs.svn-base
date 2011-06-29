using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace bottleReturn
{
    public class returnablesLIST
    {
        public enum Type { glass, plastic, can };
        Type newType;
        Random randObj1 = new Random();

        public List<returnableItem> BOTTLES;

        /// <summary>
        /// Creates a new List of Bottles for Player to Return.
        /// Returnable Items for the list are generated 1 by 1 until the
        /// size has been reached, with each item type being randomly selected.
        /// </summary>
        /// <param name="size"></param>
        public returnablesLIST(int size)
        {
            BOTTLES = new List<returnableItem>();
            Fill(size);
        }

        public void Fill(int size)
        {
            for (int index = BOTTLES.Count; index < size; index++)
            {
                #region SWITCH FOR TYPE OF ITEM TO CREATE
                switch (randObj1.Next(1, 4))
                {
                    case 1:
                        newType = returnablesLIST.Type.glass;
                        break;
                    case 2:
                        newType = returnablesLIST.Type.plastic;
                        break;
                    case 3:
                        newType = returnablesLIST.Type.can;
                        break;
                }
                #endregion
                returnableItem temp = new returnableItem(newType);
                BOTTLES.Add(temp);
            }
        }
    }
    public class returnableItem
    {
        public returnablesLIST.Type type;

        /// <summary>
        /// Creates a new Returnable Item.
        /// Textures and attributes of the Item are assigned by passed-in Type
        /// from list constructor (above) through a switch statement.
        /// </summary>
        /// <param name="type">Type of Returnable Item</param>
        public returnableItem(returnablesLIST.Type newType)
        {
            #region SWITCH FOR TYPE OF RETURNABLE
            switch (newType)
            {
                case returnablesLIST.Type.glass:
                    type = newType;
                    break;
                case returnablesLIST.Type.plastic:
                    type = newType;
                    break;
                case returnablesLIST.Type.can:
                    type = newType;
                    break;
            }
            #endregion
        }
    }
}
