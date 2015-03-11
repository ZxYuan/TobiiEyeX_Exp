using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentProgram
{
    public struct EyePoint
    {
        public double timeStamp;
        public int X;
        public int Y;
        public bool isfixed;

        public EyePoint(int x, int y, bool isf)
        {
            this.timeStamp = 0;
            this.X = x;
            this.Y = y;
            this.isfixed = isf;
        }

        public EyePoint(double t, int x, int y, bool isf)
        {
            this.timeStamp = t;
            this.X = x;
            this.Y = y;
            this.isfixed = isf;
        }
    }
}
