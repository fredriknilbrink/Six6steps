using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;



namespace SixKstep
{
    public class IosDisplaySettings : IDisplaySettings
    {
        public int GetHeight()
        {
            return (int)UIScreen.MainScreen.Bounds.Height;
        }

        public int GetWidth()
        {
            return (int)UIScreen.MainScreen.Bounds.Width;
        }
    }
}