using System;
using UnityEngine;

namespace CardHouse
{
    public class ExponentialVector3Seeker : Seeker<Vector3>
    {
        float XYGain = 8f;
        float ZGain = 3f; // Want to home Z faster than X and Y so that cards don't slide through each other as much
        float ArrivalDistance;

        /// <summary>
        /// Offset en la función de la curva de velocidad XY
        /// </summary>
        public float XYOffset = 0f;

        /// <summary>
        /// Offset en la función de la curva de velocidad Z
        /// </summary>
        public float ZOffset = 0f;

        public ExponentialVector3Seeker(float xyGain = 8f, float zGain = 10f, float arrivalDist = 0.01f, float xyOffset = 0f, float zOffset = 0f)
        {
            XYGain = xyGain;
            ZGain = zGain;
            ArrivalDistance = arrivalDist;
            XYOffset = xyOffset;
            ZOffset = zOffset;
        }

        public override Seeker<Vector3> MakeCopy()
        {
            return new ExponentialVector3Seeker(XYGain, ZGain, ArrivalDistance, XYOffset, ZOffset);
        }

        public override Vector3 Pump(Vector3 currentValue, float TimeSinceLastFrame)
        {
            var diff = End -currentValue;
            Vector2 diff2 = (Vector2)diff;
            
                
            Vector3 offset = diff2.normalized * XYOffset;//XYOffset
            
            //Math.Sign(0) ==> 0
            offset.z = System.Math.Sign(diff.z) * ZOffset;//ZOffset
                
            var displace = diff + offset;
           
            var gain = new Vector3(XYGain, XYGain, ZGain) * TimeSinceLastFrame;
            displace.Scale(gain);

            //Trim Excess
            var excess = displace-diff;
            for(var i =0; i<3;i++){
                if(Math.Sign(excess[i]) != Math.Sign(diff[i]))
                    excess[i]=0;
            }

            if(excess.magnitude >0.1f){
                Debug.Log($"{excess} : {displace} {diff}");
            }
                
            displace-=excess;
            return currentValue + displace;
        }

        public override bool IsDone(Vector3 currentValue)
        {
            return (currentValue - End).magnitude <= ArrivalDistance;
        }
    }
}
