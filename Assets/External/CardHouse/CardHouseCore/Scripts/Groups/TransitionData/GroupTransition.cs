using System;

namespace CardHouse
{
    [Serializable]
    public class GroupTransition: ICloneable
    {
        public CardGroup Source;
        public CardGroup Destination;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
