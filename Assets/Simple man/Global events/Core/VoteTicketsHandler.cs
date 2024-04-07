using System.Collections.Generic;
using UnityEngine;

namespace SimpleMan.GlobalEvents
{
    public struct VoteTicket
    {
        public UnityEngine.Object sender;
        public bool answer;
        public string message;

        public VoteTicket(Component sender, bool answer, string message)
        {
            this.sender = sender;
            this.answer = answer;
            this.message = string.IsNullOrWhiteSpace(message) ? "None" : message;
        }

        public VoteTicket(UnityEngine.Object sender, bool answer)
        {
            this.sender = sender;
            this.answer = answer;
            this.message = "None";
        }

        public override string ToString()
        {
            string gameObjectName = sender ? sender.name : "Anonym";
            string componentName = sender ? sender.GetType().Name : "Anonym";
            return $"Game object: {gameObjectName}\n" +
                   $"Component: {componentName}\n" +
                   $"Answer: {answer}\n" +
                   $"Message: {message}";
        }
    }

    public struct VoteResult
    {
        public int positiveAnswers;
        public int elementsAmount;

        public VoteResult(int positiveAnswers, int elementsAmount)
        {
            this.positiveAnswers = positiveAnswers;
            this.elementsAmount = elementsAmount;
        }

        public override string ToString()
        {
            return $"{positiveAnswers} : {elementsAmount}";
        }

        public bool UnanimouslyTrue
        {
            get => positiveAnswers == elementsAmount;
        }
        public bool UnanimouslyFalse
        {
            get => positiveAnswers == 0;
        }
    }

    public class VoteTicketsHandler
    {
        //******            FIELDS          	******\\
        private List<VoteTicket> _voteTickets = new List<VoteTicket>();




        //******            PROPERTIES        	******\\
        public IReadOnlyList<VoteTicket> VoteTickets => _voteTickets;
        public VoteResult Result
        {
            get
            {
                int positiveAnswers = 0;
                foreach (var item in _voteTickets)
                {
                    if (item.answer == true)
                        positiveAnswers++;
                }

                return new VoteResult(positiveAnswers, _voteTickets.Count);
            }
        }




        //******    	    METHODS  	  	    ******\\
        public void AddTicket(VoteTicket ticket)
        {
            _voteTickets.Add(ticket);
        }
        public void ClearTickets()
        {
            _voteTickets.Clear();
        }
    }
}