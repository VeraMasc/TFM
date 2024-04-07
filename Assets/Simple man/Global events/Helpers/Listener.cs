using UnityEngine;
using UnityEngine.Events;

namespace SimpleMan.GlobalEvents.Helpers
{
    [AddComponentMenu("Simple Man/Global Events/Listeners/Listener")]
    public sealed class Listener : MonoBehaviour
    {
        public Core.Handler handler;

        [SerializeField]
        private UnityEvent _onReceived = new UnityEvent();
        public event UnityAction OnReceived
        {
            add => _onReceived.AddListener(value);
            remove => _onReceived.RemoveListener(value);
        }

        private void OnEnable()
        {
            if (!handler)
                throw new System.NullReferenceException($"{name}: Handler is required");

            handler.AddListener(OnReceive);
        }

        private void OnDisable()
        {
            if (!handler)
                throw new System.NullReferenceException($"{name}: Handler is required");

            handler.RemoveListener(OnReceive);
        }

        private void OnReceive(UnityEngine.Object sender)
        {
            _onReceived.Invoke();
        }
    }
}