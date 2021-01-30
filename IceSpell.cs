using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpell : MonoBehaviour
{

    bool triggered;
    GameObject objToFreeze;
    
    void Start()
    {
        triggered = false;
        SpellEventSystem.current.Water_Frozen += FreezeSucessfull; // subscribe the the water frozen sucessfull event/
    }

    private void OnParticleCollision(GameObject other)
    {
        //if a particle collides with water and the spell isn't triggered.
        if (!triggered && other.tag == "Water")
        {
            objToFreeze = other; // get the game object it collided with.
            triggered = true; // set spell to have been triggered.
            SpellEventSystem.current.CastIce_Spell(objToFreeze); // call the cast ice spell method on the spell event system with 
            //the gameobject as the parameter.
        }
    }

    private void FreezeSucessfull(GameObject other) // if the freeze sucessfull event if triggered this is run.
    {
        if(other == objToFreeze) // if the game object that was frozen is the gameobject that we were trying to freeze
        {
            triggered = false; // enable the spell again.
        }
        
    }
}
