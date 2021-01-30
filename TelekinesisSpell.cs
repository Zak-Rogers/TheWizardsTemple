using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisSpell : MonoBehaviour
{

    bool triggered = false;
    GameObject objToMove;

    private void Start()
    {
        SpellEventSystem.current.Obj_Moved += MoveSucessful;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Moveable" && !triggered)
        {
            objToMove = other;
            triggered = true;
            SpellEventSystem.current.CastTelekinesis_Spell(objToMove);
        }
    }

    private void MoveSucessful(GameObject objMoved)
    {
        if(objMoved == objToMove)
        {
            triggered = false;
        }
    }
}
