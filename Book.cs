using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{

    AudioSource pageTurnAudio, upgradeAudio; 
    bool open;
    
    void Start()
    {
        AudioSource[] audio = new AudioSource[2]; // new array for 2 audio sources.
        audio =  GetComponents<AudioSource>(); // gets audio components attached the the gameobject.
        pageTurnAudio = audio[0];
        upgradeAudio = audio[1];
    }

    private void OnTriggerEnter(Collider other)
    {
        Animator animator = GetComponent<Animator>(); // gets the animator component.
        int numOfSpells = PlayerPrefs.GetInt("numOfSpells"); // get the number of spells the player has already unlocked.
        if(other.tag == "Player" && !open) // if the player enters the trigger and the book isnt open.
        {
            animator.enabled = true; // enable animator.
            pageTurnAudio.Play(); // play audio
            open = true; //set the bool open to true.
            if(numOfSpells < 4) // if the player doesnt have the maximun number of spells.
            {
                numOfSpells++; // add the next spell by incrmention the number of spells the player has.
                PlayerPrefs.SetInt("numOfSpells", numOfSpells); // set the new number of spells varible in the player prefs
                upgradeAudio.Play(); // play upgrade audio.
            }
        }
    }
}
