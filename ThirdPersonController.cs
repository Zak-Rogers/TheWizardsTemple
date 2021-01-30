using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] [Range(1,10)] float speed;
    [SerializeField] [Range(5,20)] float maxSpeed;
    [SerializeField] [Range(1, 10)] float jumpForce;
    [SerializeField] [Range(1, 10)] float levitateDuration;
    float levitateTimer;
    [SerializeField] [Range(1, 10)] float coolDownTime = 5.0f;
    float coolDownTimer;
    Rigidbody rb;
    [SerializeField] PhysicMaterial icePMaterial;
    bool inputEnabled;
    bool jump = false;
    bool canJump = false;
    bool levitate = false;
    bool canLevitate = true;
    Animator animator;

    //Audio 
    [Header("Audio")]
    [SerializeField] AudioSource walkingAudio;
    [SerializeField] AudioSource jumpAudio;

    [Header("Staff Orb Material")]
    [SerializeField] GameObject staffOrb;
    Material orbMaterial;

    //Spells
    [Header("Spells")]
    ParticleSystem currentSpell;
    [SerializeField] Color iceSpellColour, leviateSpellColour, staffSpellColour, telekinesisSpellColour; // move to heading above?
    [SerializeField] ParticleSystem iceSpell;
    [SerializeField] ParticleSystem levitateSpell;
    [SerializeField] ParticleSystem staffSpell;
    [SerializeField] ParticleSystem telekinesisSpell;
    int spellSelection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputEnabled = true;
        animator = GetComponent<Animator>();
        orbMaterial = staffOrb.GetComponent<Renderer>().material;
        if(PlayerPrefs.GetInt("numOfSpells") > 0)
          {
               spellSelection = 1;
          }
    }

    void Update()
    {
        CheckGround();
        GetInput();

        levitateTimer -= Time.deltaTime;
        levitateTimer = Mathf.Clamp(levitateTimer, -1.0f, 5.0f);
        
        if(levitateTimer <=0)
        {
            rb.useGravity = true;            
        }

        coolDownTimer -= Time.deltaTime;
        coolDownTimer = Mathf.Clamp(coolDownTimer, -1.0f, coolDownTime);
    }

    void FixedUpdate()
    {
        if(inputEnabled)
        {
            Movement();
      
            if (jump) //jump
            {
                jump = false;
                animator.SetBool("Jump", false);
                rb.velocity = rb.velocity + new Vector3(0, jumpForce, 0);
            }

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

            if(levitate)
            {
                rb.useGravity = false;
                transform.position = new Vector3(transform.position.x, transform.position.y + (jumpForce/10), transform.position.z);
                levitate = false;
                levitateTimer = levitateDuration;
                canLevitate = false;
            }
        }        
    }

    void CheckGround() // Casts a ray downwards to dectect if the player is touching the ground or if incontact with water..
    {
        RaycastHit hit;
        Vector3 rayCastPoint = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Debug.DrawRay(rayCastPoint, -transform.up, Color.yellow, 0.5f);
        if (Physics.Raycast(rayCastPoint, -transform.up, out hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj.tag == "Water")
            {
                bool frozen = hitObj.GetComponent<WaterObject>().Frozen; // gets the bool Frozen value from the water object using the public getter.
                if (frozen) // if frozen set drag to 0 and apply the ice physics material.
                {
                    rb.drag = 0;
                    rb.angularDrag = 0;
                    GetComponent<CapsuleCollider>().material = icePMaterial;

                    canLevitate = false;                   // Stop player from levate whilst jumping
                    if (rb.velocity.magnitude > 2.5f) // if the player is moving quickly over ice play the falling animation and disable input.
                    {
                        inputEnabled = false;
                        animator.SetBool("Falling", true);
                    }
                    else
                    {
                        inputEnabled = true;
                        animator.SetBool("Falling", false);
                    }
                }
                
            }

            if (hit.collider.gameObject.tag == "Ground")
            {
                canJump = true; // allow the player to jump again.
                rb.drag = 1; // adds back drag.
                rb.angularDrag = 0.05f;
                GetComponent<CapsuleCollider>().material = null; // removes ice physics material.
                inputEnabled = true; // enable player input.
                animator.SetBool("Falling", false);  // stop the falling animation.
                canLevitate = true;  //  enable levitate again.
            }

            // if the player is in the air stop the player from being able to jump again or levitate.
            if (hit.distance > jumpForce/4)
            {
                canJump = false;

                canLevitate = false;
            }
        }
    }

    void GetInput() // gets input from keyboard and xbox controller.
    {
        //Jump Input.
        {
            if (Input.GetKeyDown(KeyCode.Space) && canJump) // if canJump is true and the player presses space then jump.
            {
                Jump();
            }

            if(Input.GetButtonDown("Xbox_A") && canJump) // if canJump is true and the player presses A then jump.
            {
                Jump();
            }
        }

        //Cast Spell Input.
        {
            if (Input.GetKeyDown(KeyCode.E) && coolDownTimer <=0)
            {
                CastSpell();
            }
            else if(Input.GetAxis("Xbox_Trigger_R") > 0 && coolDownTimer <= 0) 
            {
                CastSpell();
            }
            else 
            { 
                animator.SetBool("Attack", false); 
            }
        }

        //Spell selection Input
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (PlayerPrefs.GetInt("numOfSpells") > 0)//
                {
                    spellSelection = 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (PlayerPrefs.GetInt("numOfSpells") > 1)//
                {
                    spellSelection = 2;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (PlayerPrefs.GetInt("numOfSpells") > 2)//
                {
                    spellSelection = 3;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {

                if (PlayerPrefs.GetInt("numOfSpells") > 3)//
                {
                    spellSelection = 4;
                }
            }


            if (Input.GetButtonDown("Xbox_Bumper_L") || Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (spellSelection <= 1)
                {
                    spellSelection = PlayerPrefs.GetInt("numOfSpells");
                }
                else
                {
                    spellSelection--;
                }
            }

            if (Input.GetButtonDown("Xbox_Bumper_R") || Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (spellSelection >= PlayerPrefs.GetInt("numOfSpells"))
                {
                    spellSelection = 1;
                }
                else
                {
                    spellSelection++;
                }
            }
        }
        //set spell.
        { 
            switch (spellSelection)
            {
                case 1:
                    if (PlayerPrefs.GetInt("numOfSpells") >= 1)
                    {
                        currentSpell = iceSpell;
                        orbMaterial.SetColor("StaffOrbColour", iceSpellColour);

                    }
                    break;
                case 2:
                    if (PlayerPrefs.GetInt("numOfSpells") >= 2)
                    {
                        currentSpell = levitateSpell;
                        orbMaterial.SetColor("StaffOrbColour", leviateSpellColour);
                    }
                    break;
                case 3:
                    if (PlayerPrefs.GetInt("numOfSpells") >= 3)
                    {
                        currentSpell = telekinesisSpell;
                        orbMaterial.SetColor("StaffOrbColour", telekinesisSpellColour);
                    }
                    break;
                case 4:
                    if (PlayerPrefs.GetInt("numOfSpells") == 4)
                    {
                        currentSpell = staffSpell;
                        orbMaterial.SetColor("StaffOrbColour", staffSpellColour);
                    }
                    break;
                default:
                    currentSpell = null;
                    break;
            }
        }
    }

    void Jump()
    {
        jumpAudio.Play();
        jump = true;
        animator.SetBool("Jump", true);
    }

    void CastSpell()
    {
        AudioSource spellAudio;
        if(currentSpell != null) // if there is a set spell.
        {
            spellAudio = currentSpell.GetComponent<AudioSource>(); // get spell audio.

            if(currentSpell != levitateSpell) //if the set spell is not the leviate spell
            {
                spellAudio.Play();
                currentSpell.Play();
                animator.SetBool("Attack", true);
            }

            if(currentSpell == levitateSpell && canLevitate) // if the current spell is the levitate spell and canLevitate is true.
            {
                levitate = true;
                spellAudio.Play();
                currentSpell.Play();
                animator.SetBool("Attack", true);
            }

            coolDownTimer = coolDownTime; // reset the cooldown timer for casting spells.
        }
    }

    void Movement()
    {
        float x, z;
        //get horizontal and vertical axis inputs from controller or keyboard.
        x = Input.GetAxisRaw("Horizontal") * speed; 
        z = Input.GetAxisRaw("Vertical") * speed;

        //get the direction for the camera's forward.
        Vector3 cameraForward = Camera.main.transform.forward.normalized;
        //make a new vector3 from the camera forward x and z with y set to 0.
        Vector3 faceingDirection = new Vector3(cameraForward.x, 0, cameraForward.z);

        if (x != 0 || z != 0) // if the play is inputing movement then
        {
            transform.LookAt(transform.position + faceingDirection); // look towards the direction the camera is facing
            animator.SetBool("Walking", true); // play walk animation.
            if (!walkingAudio.isPlaying) // if the walking audio isn't playing then play.
            {
                walkingAudio.Play();
            }
        }
        else // the player isnt moving.
        {
            animator.SetBool("Walking", false); // stop walk animation.
            walkingAudio.Stop(); // stop walk audio.
        }

        if (z != 0) // travel forward relative to camera using instant force on the player.
        {
            rb.AddForce(faceingDirection * z * speed * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        if (x != 0) // travel sideways relative to the player.
        {
            rb.AddForce(transform.right * x * speed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
}
