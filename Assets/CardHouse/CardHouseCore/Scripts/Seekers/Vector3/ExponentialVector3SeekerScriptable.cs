using UnityEngine;

namespace CardHouse
{
    [CreateAssetMenu(menuName = "CardHouse/Seekers/Vector3/Exponential")]
    public class ExponentialVector3SeekerScriptable : SeekerScriptable<Vector3>
    {
        public float XYGain = 8f;
        public float ZGain = 10f;
        public float ArrivalDistance = 0.01f;
        /// <summary>
        /// Offset en la función de la curva de velocidad XY
        /// </summary>
        [Min(0)]
        public float XYOffset = 0f;

        /// <summary>
        /// Offset en la función de la curva de velocidad Z
        /// </summary>
        [Min(0)]
        public float ZOffset = 0f;

        public override Seeker<Vector3> GetStrategy(params object[] args)
        {
            return new ExponentialVector3Seeker(XYGain, ZGain, ArrivalDistance, XYOffset, ZOffset);
        }
    }
}
