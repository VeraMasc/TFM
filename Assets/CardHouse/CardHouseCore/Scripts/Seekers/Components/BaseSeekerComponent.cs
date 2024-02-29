using UnityEngine;
using UnityEngine.Events;

namespace CardHouse
{
    public abstract class BaseSeekerComponent<T> : MonoBehaviour
    {
        protected Seeker<T> MyStrategy;

        protected bool IsSeeking;
        protected bool UseLocalSpace;

        public SeekerScriptable<T> Strategy;

        void Awake()
        {
            MyStrategy = Strategy?.GetStrategy() ?? GetDefaultSeeker();
        }

        public void StartSeeking(T destination, Seeker<T> strategy = null, bool useLocalSpace = false)
        {
            IsSeeking = true;
            UseLocalSpace = useLocalSpace;
            MyStrategy = strategy?.MakeCopy() ?? Strategy?.GetStrategy() ?? GetDefaultSeeker();
            MyStrategy.StartSeeking(GetCurrentValue(), destination);
        }

        void Update()
        {
            if (!IsSeeking)
                return;

            var newValue = MyStrategy.Pump(GetCurrentValue(), Time.deltaTime);
            SetNewValue(newValue);

            if (MyStrategy.IsDone(newValue))
            {
                SetNewValue(MyStrategy.End);
                IsSeeking = false;
                //Ejecutar eventos
                OnNextFinish.Invoke();
                OnNextFinish.RemoveAllListeners();
            }
        }

        protected abstract Seeker<T> GetDefaultSeeker();

        protected abstract T GetCurrentValue();

        protected abstract void SetNewValue(T value);

        /// <summary>
        /// Ejecuta un evento la próxima vez que se complete el seeking.
        /// </summary>
        public UnityEvent OnNextFinish;
    }
}
