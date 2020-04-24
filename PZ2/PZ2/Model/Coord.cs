using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ2.Model
{
    public class Coord : IEquatable<Coord>
    {
        private int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Coord other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Coord);
        }
        public override int GetHashCode()
        {
            return X;
        }
    }
}
