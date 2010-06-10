using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jazz.Objects
{
    public static class Math
    {
        public static Vector3 Vector3MultiplyMatrix(Vector3 position, Matrix matrix)
        {
            Vector3 result = new Vector3();

            result.X = (position.X * matrix.M11) + (position.Y * matrix.M21) + (position.Z * matrix.M31) + matrix.M41;

            result.Y = (position.X * matrix.M12) + (position.Y * matrix.M22) + (position.Z * matrix.M32) + matrix.M42;

            result.Z = (position.X * matrix.M13) + (position.Y * matrix.M23) + (position.Z * matrix.M33) + matrix.M43;

            return result;

        }

        public static Matrix LookAt(Vector3 left,Vector3 front, Vector3 up)
        {
            return Matrix.Identity;
        }



    }
}
