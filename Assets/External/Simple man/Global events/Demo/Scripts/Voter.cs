using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMan.GlobalEvents.Demo
{
    public class Voter : MonoBehaviour
    {
        //******            FIELDS          	******\\
        public bool myAnswer;
        public VoteSimple vote;



        //******    	    METHODS  	  	    ******\\
        private void OnEnable()
        {
            vote.AddBeginVoteListener(OnVoteBegin);
        }


        private void OnDisable()
        {
            vote.RemoveBeginVoteListener(OnVoteBegin);
        }

        private void OnVoteBegin(UnityEngine.Object sender, VoteTicketsHandler handler)
        {
            VoteTicket ticket = new VoteTicket(this, myAnswer);
            handler.AddTicket(ticket);
        }
    }
}