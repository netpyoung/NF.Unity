using System;
using System.Runtime.CompilerServices;

namespace NF.Mathematics
{
    [Serializable]
    public struct Int3 : IEquatable<Int3>, IFormattable
    {
        public int X;
        public int Y;
        public int Z;

        public Int3(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static readonly Int3 One = new Int3(1, 1, 1);
        public static readonly Int3 Zero = new Int3(0, 0, 0);

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Int2 XY
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int2(X, Y);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Int3 XYZ
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int3(X, Y, Z);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int3 operator +(Int3 a, Int3 b)
        {
            return new Int3 { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int3 operator -(Int3 a, Int3 b)
        {
            return new Int3 { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int3 operator *(Int3 a, int val)
        {
            return new Int3 { X = a.X * val, Y = a.Y * val, Z = a.Z * val };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int3 operator /(Int3 a, int val)
        {
            return new Int3 { X = a.X / val, Y = a.Y / val, Z = a.Z / val };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Int3 a, Int3 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Int3 a, Int3 b)
        {
            return !(a == b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return Equals((Int2)obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"Int3({X}, {Y}, {Z})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Int3 other)
        {
            return this == other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"Int3({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)}, {Z.ToString(format, formatProvider)})";
        }
    }
}
