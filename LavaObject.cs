using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaObject : MonoBehaviour
{

    GameObject player, respawnPoint;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") // on collision if other collider is the player rest to respawn point.
        {
            player.transform.position = respawnPoint.transform.position;
        }
    }
}
