using UnityEngine;

namespace CardHouse
{
    /// <summary>
    /// Hace que la carta vaya a una dirección concreta
    /// </summary>
    public class Homing : BaseSeekerComponent<Vector3>
    {
        protected override Seeker<Vector3> GetDefaultSeeker()
        {
            return new ExponentialVector3Seeker();
        }

        protected override Vector3 GetCurrentValue()
        {
            return UseLocalSpace ? transform.localPosition : transform.position;
        }

        protected override void SetNewValue(Vector3 value)
        {
            if (UseLocalSpace)
            {
                transform.localPosition = value;
            }
            else
            {
                transform.position = value;
            }
        }
    }
}
