using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        audio.Play();
        int level = PlayerPrefs.GetInt("Level"); // get the level the player is currently on.
        level++; // increment.
        if(level == 5) // when the game is complete.
        {
            PlayerPrefs.SetInt("Level", 0); // set the current level to 0.
            SceneManager.LoadScene(0); // loads the menu scene.
            PlayerPrefs.SetInt("Victory", 1); // sets the victory int to 1.
        }
        else // else set the new level and load the corisponding scene.
        {
            PlayerPrefs.SetInt("Level", level);
            SceneManager.LoadScene(level);
        }
    }
}
