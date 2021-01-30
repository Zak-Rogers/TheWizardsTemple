using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisObject : MonoBehaviour
{

    [SerializeField] List<Vector3> wayPoints;
    int currentPostion = 0;
    bool canMove = false;
    int numOfWayPoints;
    [SerializeField] float movementSpeed = 3;

    [Header("Glow")]
    Material objMaterial;
    Color pinkColour;
    float emissionIntensity;
    bool glow;
    [SerializeField] float maxGlowIntensity = 1.0f;
    [SerializeField] float glowSpeed;
    
    void Start()
    {
        wayPoints = new List<Vector3>(); // create a new list of wayPoint
        numOfWayPoints = transform.childCount-1;
        //loop through child gameobjects and adds their positions to list
        for (int i = 0; i <= numOfWayPoints; i++)
        {
            wayPoints.Add(transform.GetChild(i).transform.position);
        }

        objMaterial = gameObject.GetComponent<Renderer>().material; // get the material on the gameobject
        pinkColour = objMaterial.GetColor("_EmissionColor"); //get the value of the emmisions colour of the material
        emissionIntensity = 0.0f;
        glow = true;
       
        SpellEventSystem.current.Telekinesis_Spell += StartMovement; // subcribe to telekinesis spell event.
    }

    // Update is called once per frame
    void Update()
    {
        Glow();

        if (canMove)
        {
            
            MovePosition(); // move to the current possition.

            if(numOfWayPoints == 1) // if the object only have too waypoints then move back and forth between the too.
            {
                if (transform.position == wayPoints[0])
                {
                    currentPostion++;
                }else if (transform.position ==wayPoints[1])
                {
                    currentPostion--;
                }
            }else if( numOfWayPoints >1) // if it has more than 2
            {
                foreach (Vector3 point in wayPoints) // loop through each vector3 in the list
                {
                    if (transform.position == wayPoints[numOfWayPoints]) // if it is the last position
                    {
                        canMove = false; // stop it from moving.
                    }
                    else if(transform.position == point) // else increment the current possition
                    {
                        currentPostion++;
                    }
                }

            }

        }
    }

    void StartMovement(GameObject objToMove) // method called by the spell event.
    {
        if(objToMove == gameObject)
        {
            canMove = true; // allow object to move.
            gameObject.tag = "Ground"; // change to ground tag to allow the player to walk on it.
            SpellEventSystem.current.MoveSucessful(gameObject); // trigger the comformation event.
        }
    }

    private void MovePosition()
    {
       //move towards the current position.
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentPostion], movementSpeed * Time.deltaTime);
    }

    private void Glow() // function for the glow emmision on the objects material.
    {
        if (glow)
        {
            emissionIntensity += glowSpeed * Time.deltaTime;
        }
        else
        {
            emissionIntensity -= glowSpeed * Time.deltaTime;
        }

        if (emissionIntensity >= maxGlowIntensity)
        {
            glow = false;
        }
        else if (emissionIntensity <= 0.0f)
        {
            glow = true;
        }

        emissionIntensity = Mathf.Clamp(emissionIntensity, 0.0f, maxGlowIntensity);
        objMaterial.SetColor("_EmissionColor", pinkColour * emissionIntensity);
    }
}
