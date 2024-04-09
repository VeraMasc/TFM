using UnityEngine;


namespace Effect{
    /// <summary>
    /// Agrupa todos los efectos que pueden ser considerados un "coste" y cuya posiblidad de resolución ha de ser comprobada antes de ejecutarlo 
    /// </summary>
    public interface ICost
    {
        /// <summary>
        /// Checks if the cost can be paid
        /// </summary>
        public bool canBePaid(Effect.Context context);
    }

}