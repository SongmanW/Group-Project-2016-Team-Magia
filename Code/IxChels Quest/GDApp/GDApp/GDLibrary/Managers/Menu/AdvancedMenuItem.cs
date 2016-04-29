
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class AdvancedMenuItem : MenuItem
    {

        public AdvancedMenuItem(string name, string text,
            Rectangle bounds, Color inactiveColor, Color activeColor)
            : base(name, text, bounds, inactiveColor, activeColor)
        {
        }
    }
}
