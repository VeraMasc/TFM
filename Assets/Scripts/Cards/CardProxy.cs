using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHouse;
using NaughtyAttributes;

public class CardProxy : Card
{
    public Card actualCard;
    public bool isActiveProxy;

    [Button]
    public void setAsActive(){
        if(actualCard==null)
            return;
        isActiveProxy=true;
    }

    [Button]
    public void forceSeeking(){
        setAsActive();
        actualCard.displayHiding(false);
        actualCard.FlipAnimator.SetBool("FalseFaceUp",true);
        actualCard.Homing.StartSeeking(transform.position);
        actualCard.Turning.StartSeeking(transform.rotation.eulerAngles.z);
        actualCard.Scaling.StartSeeking(transform.lossyScale.x);
    }

    [Button]
    public void undoSeeking(){
        isActiveProxy=false;
        actualCard.FlipAnimator.SetBool("FalseFaceUp",false);
        actualCard.Group.ApplyStrategy();
    }

    [Button]
    public void hookProxy(){
        if(actualCard==null)
            return;
        if(!actualCard.proxies.Contains(this)){
            actualCard.proxies.Add(this);
        }
    }

    [Button]
    public void unHookProxy(){
        if(actualCard==null)
            return;
        if(actualCard.proxies.Contains(this)){
            actualCard.proxies.Remove(this);
        }
    }

    void Start()
    {
        Homing.MyStrategy = new InstantVector3Seeker();
        Turning.MyStrategy = new InstantFloatSeeker();
        Scaling.MyStrategy = new InstantFloatSeeker();

    }

    void OnDestroy()
    {
        unHookProxy();
    }
}
