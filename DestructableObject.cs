using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{

    [Header("Glow")]
    Material objMaterial;
    Color redColour;
    float emissionIntensity;
    bool glow;
    [SerializeField] float maxGlowIntensity = 1.0f;
    [SerializeField] float glowSpeed;
    //[SerializeField] bool canGlow = true;

    private void Start()
    {
        objMaterial = gameObject.GetComponent<Renderer>().material; // gets the material of the object.
        redColour = objMaterial.GetColor("_EmissionColor"); //gets the EmissionColor varible from the material.
        emissionIntensity = 0.0f;
        glow = true;
    }
    private void Update()
    {
        if (glow)// if glow is true increase the emissionIntesity float by the speed it will change * deltatime.
        {
            emissionIntensity += glowSpeed * Time.deltaTime; 
        }
        else // else decrease 
        {
            emissionIntensity -= glowSpeed * Time.deltaTime;
        }

        if (emissionIntensity >= maxGlowIntensity) // if the emission intensity reachs the max intensity then set glow to false to reverse the glow.
        {
            glow = false;
        }
        else if (emissionIntensity <= 0.0f) // if it reaches 0 start increasing again.
        {
            glow = true;
        }

        emissionIntensity = Mathf.Clamp(emissionIntensity, 0.0f, maxGlowIntensity); // clamps the emissionIntensity between 0 and the max intensity.
        objMaterial.SetColor("_EmissionColor", redColour * emissionIntensity); // set the Emissioncolor material varible to the red colour * the emissionIntensity.
     
        
    }
    private void OnDestroy()// when the object is destroyd call the comformation event.
    {
        SpellEventSystem.current.Destruction_Successful(gameObject); 
    }
}
