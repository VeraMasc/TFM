using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventCallInput : PlayerInputBase
{
    public UnityEvent confirmEvent;
    public UnityEvent cancelEvent;
    public override void confirmChoice(){
        confirmEvent.Invoke();
        base.confirmChoice();
    }

    public override void cancelChoice(){
        cancelEvent.Invoke();
        base.cancelChoice();
    }

    public void quit(){
        Application.Quit();
    }

    public void gotoScene(string name){
        SceneManager.LoadScene(name);
    }
}
