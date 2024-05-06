using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Gestiona el display de la salud del personaje
/// </summary>
public class HealthDisplay : MonoBehaviour
{

    public int health=20;
    public int maxHealth=20;
    public TextMeshPro textBox;

    private Entity entity;


    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity>();
    }

    public void onHealthChanged(){
        health =entity.health;
        maxHealth = entity.maxHealth;
        refresh();
    }

    [NaughtyAttributes.Button]
    public void refresh(){
        if(!textBox)
            return;
        
        string healthText = health.ToString();
        if(health <=0){
            healthText = $"<color=#330000>{healthText}</color>";
        }else if(health > maxHealth){
            healthText = $"<color=#f2d00d>{healthText}</color>";
        } else if( health == maxHealth){
            healthText = $"<color=#0ac20a>{healthText}</color>";
        }

        textBox.text = $" {healthText}<color=#800000ff>/\n<size=1>{maxHealth}</size></color>";
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
