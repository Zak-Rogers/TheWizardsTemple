using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : MonoBehaviour
{

    GameObject player, respawnPoint; // GameObject Varibles to hold the player and respawnPoint GameObjects.

    [Header("Frozen Settings")] // adds a header to the inspector for the [SerializedField] and public varibles.
    [SerializeField] float meltTime = 5.0f; // [SerializedField] allows private varible to be visible in the inspector.
    float meltTimer;
    AudioSource freezingAudio; // private AudioSouce for the AudioSouce component attached to the gameObject.
    float audioTimer; // float to contain a random float for randomising the audio play time.

    [Header("Materials")]
    [SerializeField] Material iceMaterial; // Material for the ice shader to change the gameObject's material too when frozen.
    [SerializeField] Material waterMaterial; // Material for the water shader to change the gameObject's material too when not frozen.

    [Header("Physics Material")]
    [SerializeField] PhysicMaterial icePMaterial; // PhysicMaterial for the collider when the water is frozen.

    public bool Frozen { get; private set;} // a public varible which can another class can get but only this class can set ( private set).

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find and Assign Player GameObject.
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");// Find and Assign Respawn GameObject.
        Frozen = false; // set frozen to false.
        freezingAudio = GetComponent<AudioSource>(); // Get AudioSouce component and assign to the freezingAudio varible.
        meltTimer = meltTime;
        SpellEventSystem.current.Ice_Spell += Freeze; // Subscribes to the Ice_Spell event on the SpellEventSystem with the method Freeze.
    }

    private void Update()
    {
        if(Frozen) // If the water is frozen.
        {
            meltTimer -= Time.deltaTime; // Reduce the meltCountDwn by amount of time since last frame.
            audioTimer -= Time.deltaTime; // Reduce audioTimer by amount of time since last frame.

            if (meltTimer <= 0) // if the meltCountDwn reach 0 or below.
            {
                GetComponent<MeshRenderer>().material = waterMaterial; // assign the water material back to the gameObject.
                GetComponent<BoxCollider>().material = null; // set the colliders physics material to null.
                Frozen = false; // set frozen to false.
                meltTimer = meltTime; // reset meltCountDwn.
            }

            if(audioTimer <= 0) // if audioTimer is less than or equal to 0.
            {
                freezingAudio.Play(); // Play audio.
                audioTimer = Random.Range(1.0f, 15.0f); // set the audioTimer to a new random float betwen 1.0f and 15.0f.
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && !Frozen) // if the player collides with the water and its not frozen.
        {
            player.transform.position = respawnPoint.transform.position; // set the players position to the respawnPoint.
        }
    }

    // Called when the Ice_Spell event is triggered with a gameObject as a parameter.
    private void Freeze(GameObject objToFreeze)
    {
        if(objToFreeze == gameObject) // if the gameObject passed in by the eventSystem is the same as this gameObject.
        {
            freezingAudio.Play(); // play audio.
            Frozen = true; // set frozen to true.
            GetComponent<MeshRenderer>().material = iceMaterial; // assign the ice material to the gameObject.
            GetComponent<BoxCollider>().material = icePMaterial;// assign the ice Physics Material to the gameObject's collider.
            SpellEventSystem.current.FrozenSucessfull(objToFreeze); // calls the method to trigger another event to comfirm the water was frozen sucesfully.
            audioTimer = Random.Range(1.0f, 15.0f); 
        }
    }
}
