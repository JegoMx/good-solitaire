using UnityEngine;

namespace Game.Utility
{
    public static class FloatExtensions
    {
        /// <summary> Maps a number from range (a,b) to (c,d), with the resulting number not being clamped to (c,d) </summary>
        /// <remarks><c> return 100f.Map(0, 10, 0, 1) // Returns 10f </c></remarks>
        /// <returns> The mapped number </returns>
        /// <param name="fromSource">The lowest value of the orginal range (inclusive)</param>
        /// <param name="toSource">The highest value of the original range (inclusive)</param>
        /// <param name="fromTarget">The lowest value of the new range (inclusive)</param>
        /// <param name="toTarget">The highest value of the new range (inclusive)</param>
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        /// <summary> Maps a number from range (a,b) to (c,d), clamping the result betwheen (c,d) </summary>
        /// <remarks><c> return 8f.Map(0, 10, 0, 1000); // Returns 800f</c> and <c>return 100f.Map(0, 10, 0, 1); // Returns 1f </c></remarks>
        /// <returns> The mapped number </returns>
        /// <param name="fromSource">The lowest value of the orginal range (inclusive)</param>
        /// <param name="toSource">The highest value of the original range (inclusive)</param>
        /// <param name="fromTarget">The lowest value of the new range (inclusive)</param>
        /// <param name="toTarget">The highest value of the new range (inclusive)</param>
        public static float MapClamped(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            float notClampedResult = (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;

            if (fromTarget < toTarget)
                return Mathf.Clamp(notClampedResult, fromTarget, toTarget);
            else
                return Mathf.Clamp(notClampedResult, toTarget, fromTarget);
        }
    }
}