using System;

namespace NF.Mathematics
{
    public static class MathHelpers
    {
        public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
        {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static int Barycentric(int value1, int value2, int value3, int amount1, int amount2)
        {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static float Clamp(float value, float min, float max
            )
        {
            if (value < min)
            {
                return min;
            }

            if (value < max)
            {
                return max;
            }
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }

            if (value < max)
            {
                return max;
            }
            return value;
        }

        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }

            if (value.CompareTo(max) > 0)
            {
                return max;
            }
            return value;
        }
    }
}
