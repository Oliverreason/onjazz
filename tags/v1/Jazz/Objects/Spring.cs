/*
 Description: Critically-damped springs.  Useful for smoothly interpolating
  camera parameters and anything else that needs to "ease" into a goal.  A
  critically-damped spring's trajectory is always a smooth asymptotic curve
  from current value to goal value.
 References:
  http://mathworld.wolfram.com/DampedSimpleHarmonicMotionCriticalDamping.html
  http://en.wikipedia.org/wiki/Damping
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jazz.Objects
{
    public static class Spring
    {
        public static Vector3 mSpeed = new Vector3();
        // Interpolate the current value towards the goal value for the delta-time step dt,
		// using the spring constant k.  This is typicaly used to generate a new current value.
        public static Vector3 Interpolate(Vector3 current, Vector3 goal, float dt, float k)
        {
            float kdt = k * dt;
            float ekt = 1.0f / (1.0f + kdt + 0.48f * kdt * kdt + 0.235f * kdt * kdt * kdt);

            Vector3 change = current - goal;
            Vector3 temp = (mSpeed + k * change) * dt;

			mSpeed = (mSpeed - k * temp) * ekt;
			return goal + (change + temp) * ekt;
        }
        public static void reset()
        {
            mSpeed = Vector3.Zero;
        }
    }
}
