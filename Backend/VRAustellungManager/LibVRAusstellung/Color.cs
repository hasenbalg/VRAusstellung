using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibVRAusstellung
{
    public class Color
    {
        private int _r;
        public int R
        {
            get { return _r; }
            set { _r = SetCol(value); }
        }

       

        private int _g;
        public int G
        {
            get { return _g; }
            set { _g = SetCol(value); ; }
        }

        private int _b;
        public int B
        {
            get { return _b; }
            set { _b = SetCol(value); ; }
        }

        public Color()
        {
            this.R = 0;
            this.G = 0;
            this.B = 0;
        }

        public Color(int r, int g, int b) {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        private int SetCol(int value)
        {
            
            return Math.Abs(value) < 255 ? Math.Abs(value) : 255;
        }

        public static string ToHex(Color c) {
            //https://stackoverflow.com/a/13354940
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static Color Hex2Color(string hex) {
            if (string.IsNullOrEmpty(hex))
            {
                return new Color(0,0,0);
            }

            return new Color(
                int.Parse(hex.Substring(1,2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(3,2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(5,2), System.Globalization.NumberStyles.HexNumber)
                );
        }
    }
}
