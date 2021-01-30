using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    AudioSource audio;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        int numOfSpells= PlayerPrefs.GetInt("numOfSpells"); // get the number of spells the player has unlocked.
        // gets the staff child from the wizard character.
        Transform staffParent = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0); 
        GameObject staff = staffParent.transform.GetChild(0).gameObject;

        if (other.tag == "Player" && numOfSpells ==3) // if the player enters the trigger and has 3 spells unlocked.
        { 
            // increment the number of spells and assign to the player pref varible.
            numOfSpells++; 
            PlayerPrefs.SetInt("numOfSpells", numOfSpells);

            //set wizards staff child object to visible and set current staff to not visible.
            staff.SetActive(true);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            audio.Play();

        }
    }
}
