using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEventSystem : MonoBehaviour
{

    public static SpellEventSystem current; // create a static varible of this class.

    private void Awake()
    {
        current = this; 
    }

    public event Action<GameObject> Ice_Spell; // an event with a gameobject parameter for the ice spell.
    public event Action<GameObject> Water_Frozen; // an event to comferm the water was frozen.

    public event Action<GameObject> Staff_Spell;
    public event Action<GameObject> Obj_Destroyed;

    public event Action<GameObject> Telekinesis_Spell;
    public event Action<GameObject> Obj_Moved;

    public void CastIce_Spell(GameObject objToFreeze) // if method is called trigger the ice spell event.
    {
        if( Ice_Spell != null)
        {
            Ice_Spell(objToFreeze);
        }
    }

    public void FrozenSucessfull(GameObject frozenObj)
    {
        if(Water_Frozen != null)
        {
            Water_Frozen(frozenObj);
        }
    }

    public void CastStaff_Spell(GameObject objToDestroy)
    {
        Destroy(objToDestroy);
    }

    public void Destruction_Successful(GameObject objDestroyed)
    {
        Obj_Destroyed(objDestroyed);
    } 

    public void CastTelekinesis_Spell(GameObject objToMove)
    {
        Telekinesis_Spell(objToMove);
    }

    public void MoveSucessful(GameObject objMoved)
    {
        Obj_Moved(objMoved);
    }
}
