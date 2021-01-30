using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffSpell : MonoBehaviour
{

    bool triggered = false;
    GameObject objToDestroy;

    void Start()
    {
        SpellEventSystem.current.Obj_Destroyed += Spell_Successfull;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Destructable" && !triggered)
        {
            triggered = true;
            objToDestroy = other;
            SpellEventSystem.current.CastStaff_Spell(objToDestroy);
        }
    }

    void Spell_Successfull(GameObject other)
    {
        if( other == objToDestroy)
        {
            triggered = false;
        }
    }
}
