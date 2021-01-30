using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    Transform pivot, player;
    Vector3 rotation;
    float rotationSpeed =1.0f;
    float mouseX, mouseY, controllerX, controllerY;

    List<MeshRenderer> obstructions = new List<MeshRenderer>(); // a new list of Mesh Renderers to contain camera obstructions.

    void Start()
    {
        pivot = transform.parent;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Cursor.lockState = CursorLockMode.Locked; // locks the cursor to the centre of the screen.
        Cursor.visible = false; // makes cursor not visible.
    }

    private void Update()
    {
        //get mouse x and y
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //get controller right stick input.
        controllerX = Input.GetAxis("Xbox_RightStick_X");
        controllerY = Input.GetAxis("Xbox_RightStick_Y");
    }

    // LateUpdate is called once per frame, after Update()
    void LateUpdate()
    {
        //get rotation vector3 x and y values depending on controller or mouse input.
        if (mouseX != 0 || mouseY != 0 || controllerX != 0 || controllerY != 0)
        {
            rotation.x += mouseX * rotationSpeed;
            rotation.y -= mouseY * rotationSpeed;

            if (controllerX <= -0.4f || controllerX >= 0.4f)
            {
                rotation.x += controllerX * rotationSpeed;
            }
            if (controllerX <= -0.15f || controllerX >= 0.15f)
            {
                rotation.y += controllerY * rotationSpeed;
            }

            rotation.y = Mathf.Clamp(rotation.y, 0, 30); // clamps the y rotation between 0 and 30 to stop the player from moving the camera over the head of the player.
        }

        //make a Quaternion from the roation Vector3 and apply to the pivot
        Quaternion rotationQuaterion = Quaternion.Euler(rotation.y, rotation.x, 0);
        pivot.transform.rotation = rotationQuaterion;

        //cast rays to check for camera obstructions.
        CastRays();
    }

    void CastRays()
    {
        float offSet = 0.33f;
        Vector3 playerPosition = new Vector3(player.transform.position.x + 2.0f, player.transform.position.y + 1, player.transform.position.z);
        //create a layerMask from the camera obstructions layer.
        int layerMask = 1 << LayerMask.NameToLayer("CameraObstruction"); // shifts the bits by one

        if(obstructions != null) // if there is objects in the obstructions list then enable their mesh renderers
        {
            foreach(MeshRenderer o in obstructions)
            {
                o.enabled = true;
            }
            obstructions.Clear(); // clear obstruction list.
        }

        // cast several rays faning over the player.
        for ( int i = 0; i< 11; i++)
        {
            RaycastHit hit;
            playerPosition.x -= offSet;
            Vector3 direction = playerPosition - transform.position;
            Debug.DrawRay(transform.position, direction, Color.red, 3.0f);

            if (Physics.Raycast(transform.position, direction, out hit, 3.5f, layerMask)) // if the ray its something set to the camer obstruction layer.
            {
                if(hit.collider.tag != "Player")
                {
                    MeshRenderer meshRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>(); // get the obstructions meshRenderer.
                    obstructions.Add(meshRenderer); // add it to teh obstructions list.
                }
            }

        }

        if(obstructions != null) // if there are obstructions disable each MeshRenderer
        {
            foreach( MeshRenderer o in obstructions)
            {
                o.enabled = false; 
            }
        }
    }
}
