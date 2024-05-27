using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHouse;
using NaughtyAttributes;

/// <summary>
/// Representa una carta que en realidad se encuentra en otro sitio.
/// </summary>
public class CardProxy : Card
{
    public Card actualCard;
    public bool isActiveProxy;


    /// <summary>
    /// Crea un proxy de una carta concreta
    /// </summary>
    public static CardProxy createProxy(Card card, Entity user){
        var prefab = GameController.singleton.creationManager.proxyPrefab;
        var transform = card.transform;
        var ret = Instantiate(prefab, transform.position, transform.rotation);
        ret.actualCard = card;
        ret.hookProxy();
        if(user.hand.Strategy is HandLayout hand){
            hand.proxies.Add(ret);
            user.hand.ApplyStrategy();
        }
        return ret;
    }

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
