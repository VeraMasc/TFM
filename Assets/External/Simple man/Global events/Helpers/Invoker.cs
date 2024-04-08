using UnityEngine;

namespace SimpleMan.GlobalEvents.Helpers
{
    [AddComponentMenu("Simple Man/Global Events/Invokers/Invoker")]
    public sealed class Invoker : MonoBehaviour
    {
        public Core.Handler handler;

        public void Invoke()
        {
            handler?.Invoke(this);
        }
    }
}