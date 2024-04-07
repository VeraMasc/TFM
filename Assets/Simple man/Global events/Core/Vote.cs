using System;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

namespace SimpleMan.GlobalEvents.Core
{
    public delegate void VoteActionsHandler(UnityEngine.Object sender, VoteTicketsHandler handler);
    public delegate void VoteActionsHandlerArg<T>(UnityEngine.Object sender, VoteTicketsHandler handler, T argument);

    public abstract class Vote : ScriptableObject
    {
        //******            PROPERTIES        	******\\
        public bool Voting { get; private set; }




        //******            FIELDS          	******\\
        [SerializeField]
        private GlobalRequestBase _triggerRequest;

        [SerializeField]
        private GlobalEventBase _onCompleteEvent;

        public bool printLog = true;
        private VoteTicketsHandler _handler = new VoteTicketsHandler();




        //******    	    EVENTS  	  	    ******\\
        private VoteActionsHandler _onVoteBegan;
        private VoteActionsHandler _onVoteCompleted;




        //******    	    METHODS  	  	    ******\\
        private void OnEnable()
        {
            if (_triggerRequest)
            {
                _triggerRequest.AddListener(OnTriggerReceived);
            }
        }

        private void OnDisable()
        {
            if (_triggerRequest)
            {
                _triggerRequest.RemoveListener(OnTriggerReceived);
            }
        }

        public void AddBeginVoteListener(VoteActionsHandler listener, bool unique = true)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            if (unique && _onVoteBegan != null && _onVoteBegan.GetInvocationList().Contains(listener))
                return;

            _onVoteBegan += listener;
        }

        public void RemoveBeginVoteListener(VoteActionsHandler listener)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            _onVoteBegan -= listener;
        }

        public void AddCompleteVoteListener(VoteActionsHandler listener, bool unique = true)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            if (unique && _onVoteCompleted != null && _onVoteCompleted.GetInvocationList().Contains(listener))
                return;

            _onVoteCompleted += listener;
        }

        public void RemoveCompleteVoteListener(VoteActionsHandler listener)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            _onVoteCompleted -= listener;
        }

        public void BeginVote(UnityEngine.Object sender)
        {
            BeginVote(sender, null, null);
        }

        public async void BeginVote(UnityEngine.Object sender, VoteActionsHandler onBegin, VoteActionsHandler onComplete)
        {
            if (Voting)
                throw new InvalidOperationException($"{name}: Can not begin vote before previous complete");

            Voting = true;
            _handler.ClearTickets();
            _onVoteBegan?.Invoke(this, _handler);
            onBegin?.Invoke(sender, _handler);
            await Task.Yield();

            if (printLog)
            {
                string detailedLog = string.Empty;
                foreach (var item in _handler.VoteTickets)
                {
                    detailedLog += item + "\n\n";
                }
                this.PrintLog($"Complete with result: <b>{_handler.Result}</b>.\nSender:<b>{sender}</b>\n\n {detailedLog}");
            }

            Voting = false;
            _onVoteCompleted?.Invoke(this, _handler);
            onComplete?.Invoke(sender, _handler);
            _onCompleteEvent?.Invoke(this);

            OnVoteDone(_handler);
        }

        protected virtual void OnVoteDone(VoteTicketsHandler handler)
        {

        }

        private void OnTriggerReceived(UnityEngine.Object sender)
        {
            BeginVote(this);
        }
    }

    public abstract class VoteArg<TArg> : ScriptableObject
    {
        //******            PROPERTIES        	******\\
        public bool Voting { get; private set; }




        //******            FIELDS          	******\\
        [SerializeField]
        private ScriptableObject _triggerRequest;

        [SerializeField]
        private GlobalEventBase _onCompletedEvent;

        public bool printLog = true;
        private VoteTicketsHandler _handler = new VoteTicketsHandler();




        //******    	    EVENTS  	  	    ******\\
        private VoteActionsHandlerArg<TArg> _onVoteBegan;
        private VoteActionsHandlerArg<TArg> _onVoteCompleted;



        //******    	    METHODS  	  	    ******\\
        private void OnValidate()
        {
            if (_triggerRequest && _triggerRequest is GlobalRequestBaseArg<TArg> == false)
                _triggerRequest = null;
        }

        private void OnEnable()
        {
            if (_triggerRequest)
                (_triggerRequest as GlobalRequestBaseArg<TArg>).AddListener(OnTriggerReceived);
        }

        private void OnDisable()
        {
            if (_triggerRequest)
                (_triggerRequest as GlobalRequestBaseArg<TArg>).RemoveListener(OnTriggerReceived);
        }

        public void AddVoteBeginListener(VoteActionsHandlerArg<TArg> listener, bool unique = true)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            if (unique && _onVoteBegan != null && _onVoteBegan.GetInvocationList().Contains(listener))
                return;

            _onVoteBegan += listener;
        }

        public void RemoveVoteBeginListener(VoteActionsHandlerArg<TArg> listener)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            _onVoteBegan -= listener;
        }

        public void AddCompleteVoteListener(VoteActionsHandlerArg<TArg> listener, bool unique = true)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            if (unique && _onVoteCompleted != null && _onVoteCompleted.GetInvocationList().Contains(listener))
                return;

            _onVoteCompleted += listener;
        }

        public void RemoveCompleteVoteListener(VoteActionsHandlerArg<TArg> listener)
        {
            if (listener == null)
                throw new System.ArgumentNullException("Listener");

            _onVoteCompleted -= listener;
        }

        public void BeginVote(UnityEngine.Object sender, TArg argument)
        {
            BeginVote(sender, null, null, argument);
        }

        public async void BeginVote(UnityEngine.Object sender, VoteActionsHandlerArg<TArg> onBegin, VoteActionsHandlerArg<TArg> onComplete, TArg argument)
        {
            if (Voting)
                throw new InvalidOperationException($"{name}: Can not begin vote before previous complete");

            Voting = true;
            _handler.ClearTickets();
            _onVoteBegan?.Invoke(this, _handler, argument);
            onBegin?.Invoke(sender, _handler, argument);
            await Task.Yield();

            if (printLog)
            {
                string detailedLog = string.Empty;
                foreach (var item in _handler.VoteTickets)
                {
                    detailedLog += item + "\n\n";
                }
                this.PrintLog($"Vote argumented <b>'{argument}'</b> complete with result: <b>{_handler.Result}</b>.\nSender:<b>{sender}</b>\n\n {detailedLog}");
            }

            Voting = false;
            _onVoteCompleted?.Invoke(this, _handler, argument);
            onComplete?.Invoke(sender, _handler, argument);
            _onCompletedEvent?.Invoke(this);

            OnVoteComplete(_handler, argument);
        }

        protected virtual void OnVoteComplete(VoteTicketsHandler handler, TArg argument)
        {

        }

        protected virtual void OnTriggerReceived(UnityEngine.Object sender, TArg argument)
        {
            BeginVote(this, argument);
        }
    }
}