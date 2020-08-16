using UnityEngine;

namespace NFRuntime.Util
{
    public static class Curve
    {
        // Bezier 1 / 2 / 3
        // B-spline
        // Cubic Hermite Splines
        // Cardinal Splines
        // Kochanek–Bartels (KB) 

        public static Vector3 CatmulRom(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, float f)
        {
            // http://www.lighthouse3d.com/tutorials/maths/catmull-rom-spline/
            // M
            // |  0.0  1.0  0.0  0.0  |
            // | -0.5  0.0  0.5  0.0  |
            // |  1.0 -2.5  2.0 -0.5  |
            // | -0.5  1.5 -1.5 -0.5  |

            const double M10 = -0.5;
            const double M12 = 0.5;

            const double M21 = -2.5;
            const double M22 = 2;
            const double M23 = -0.5;

            const double M30 = -0.5;
            const double M31 = 1.5;
            const double M32 = -1.5;
            const double M33 = 0.5;

            double c2x = M10 * v0.x + M12 * v2.x;
            double c2y = M10 * v0.y + M12 * v2.y;
            double c2z = M10 * v0.z + M12 * v2.z;

            double c3x = v0.x + M21 * v1.x + M22 * v2.x + M23 * v3.x;
            double c3y = v0.y + M21 * v1.y + M22 * v2.y + M23 * v3.y;
            double c3z = v0.z + M21 * v1.z + M22 * v2.z + M23 * v3.z;

            double c4x = M30 * v0.x + M31 * v1.x + M32 * v2.x + M33 * v3.x;
            double c4y = M30 * v0.y + M31 * v1.y + M32 * v2.y + M33 * v3.y;
            double c4z = M30 * v0.z + M31 * v1.z + M32 * v2.z + M33 * v3.z;

            return new Vector3(
                (float)(v1.x + f * (c2x + f * (c3x + f * c4x))),
                (float)(v1.y + f * (c2y + f * (c3y + f * c4y))),
                (float)(v1.z + f * (c2z + f * (c3z + f * c4z)))
            );
        }
    }
}
