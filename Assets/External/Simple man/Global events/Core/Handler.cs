using System.Linq;
using UnityEngine;


namespace SimpleMan.GlobalEvents.Core
{
    /// <summary>
    /// Base class of all events and orders
    /// </summary>
    public abstract class Handler : ScriptableObject
    {
        //******            PROPERTIES        	******\\
        public string Description => _description;




        //******            FIELDS          	******\\
        public bool printLog = true;

        [SerializeField]
        [TextArea(3, 6)]
        private string _description;
        private GameEventHandler _eventHandler;





        //******    	    METHODS  	  	    ******\\
        public void AddListener(GameEventHandler listener, bool unique = true)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            if (unique && _eventHandler != null && _eventHandler.GetInvocationList().Contains(listener))
                return;

            _eventHandler += listener;
        }

        public void RemoveListener(GameEventHandler listener)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            _eventHandler -= listener;
        }

        public void Invoke(UnityEngine.Object sender)
        {
            _eventHandler?.Invoke(sender);

            if (printLog)
                this.PrintLog($"Invoked.\nSender: <b>{sender}</b>");
        }
        /// <summary>
        /// Invoca el evento solo en el listener concreto
        /// </summary>
        /// <param name="sender"></param>
        public void InvokeOnListener(UnityEngine.Object sender)
        {
            _eventHandler?.Invoke(sender);

            if (printLog)
                this.PrintLog($"Invoked.\nSender: <b>{sender}</b>");
        }
    }

    /// <summary>
    /// Base class of all events and orders thas uses argument
    /// </summary>
    public abstract class HandlerArg<TArg1> : ScriptableObject
    {
        //******            PROPERTIES        	******\\
        public string Description => _description;





        //******            FIELDS          	******\\
        public bool printLog = true;

        [SerializeField]
        [TextArea(3, 6)]
        private string _description;
        private GameEventHandler<TArg1> _eventHandler;





        //******    	    METHODS  	  	    ******\\
        public void AddListener(GameEventHandler<TArg1> listener, bool unique = true)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            if (unique && _eventHandler != null && _eventHandler.GetInvocationList().Contains(listener))
                return;

            _eventHandler += listener;
        }

        public void RemoveListener(GameEventHandler<TArg1> listener)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            _eventHandler -= listener;
        }

        public void Invoke(UnityEngine.Object sender, TArg1 arg)
        {
            _eventHandler?.Invoke(sender, arg);

            if (printLog)
                this.PrintLog($"Invoked with <b>'{arg}'</b> parameter.\nSender: <b>{sender}</b>");
        }
    }

    /// <summary>
    /// Base class of requests
    /// </summary>
    public abstract class GlobalRequestBase : Handler
    {

    }

    /// <summary>
    /// Base class of requests with argument
    /// </summary>
    public abstract class GlobalRequestBaseArg<TArg1> : HandlerArg<TArg1>
    {

    }

    /// <summary>
    /// Base class of events
    /// </summary>
    public abstract class GlobalEventBase : Handler
    {

    }

    /// <summary>
    /// Base class of events with argument
    /// </summary>
    public abstract class GlobalEventBaseArg<TArg1> : HandlerArg<TArg1>
    {

    } 
}