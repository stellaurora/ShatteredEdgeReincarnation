using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float drag_grounded;
    public float drag_inair;
    public Camera camera;

    public Transform playercontroller;
    public bool sliding;
    public bool sprinting;
    public CapsuleCollider capsuleCollider;


    private bool slidingran;
    private bool sprintingran;

    public GameObject previousobject;
    private GameObject previousobjectreference;
    public float Vel;
    public DetectCrouch detectCrouch;
    public DetectObs DetectWallL; //detects for a wall on the left
    public DetectObs DetectWallR; //detects for a wall on the right

    public Animator cameraAnimator;

    public float WallRunUpForce;
    public float WallRunUpForce_DecreaseRate;

    private float upforce;

    public float WallJumpUpVelocity;
    public float WallJumpForwardVelocity;
    public float drag_wallrun;
    public bool WallRunning;
    public bool WallrunningLeft;
    public bool WallrunningRight;
    public bool canwallrun; // ensure that player can only wallrun once before needing to hit the ground again, can be modified for double wallruns
    
    public bool IsParkour;
    private float t_parkour;
    private float chosenParkourMoveTime = 0f;

    private bool CanVault;
    public float VaultTime; //how long the vault takes
    public Transform VaultEndPoint;

    private bool CanClimb;
    public float ClimbTime; //how long the vault takes
    public Transform ClimbEndPoint;

    private RigidbodyFirstPersonController rbfps;
    private Rigidbody rb;
    private Transform ground;
    private Vector3 RecordedMoveToPosition = Vector3.zero; //the position of the vault end point in world space to move the player to
    private Vector3 RecordedStartPosition = Vector3.zero; // position of player right before vault
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rbfps = GetComponent<RigidbodyFirstPersonController>();
        rb = GetComponent<Rigidbody>();
        previousobjectreference = previousobject;
    }

    // Update is called once per frame
    void Update()
    {
        Vel = camera.fieldOfView;
        if (rbfps.Grounded)
        {
            rb.drag = drag_grounded;
            canwallrun = true;
        }
        else
        {
            rb.drag = drag_inair;
        }
        if(WallRunning)
        {
            rb.drag = drag_wallrun;

        }


        //Parkour movement
        if (IsParkour && t_parkour < 1f)
        {
            t_parkour += Time.deltaTime / chosenParkourMoveTime;
            transform.position = Vector3.Lerp(RecordedStartPosition, RecordedMoveToPosition, t_parkour);

            if (t_parkour >= 1f)
            {
                IsParkour = false;
                t_parkour = 0f;
                rb.isKinematic = false;

            }
        }
        // Sprinting
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }

        if(sprinting)
        {
            rbfps.movementSettings.FS = rbfps.movementSettings.FS < 4f ? rbfps.movementSettings.FS + Mathf.Lerp(0, 1, Time.deltaTime * 1.5f) : 4f;
            rbfps.movementSettings.BS = rbfps.movementSettings.BS < 3f ? rbfps.movementSettings.BS + Mathf.Lerp(0, 1, Time.deltaTime * 1) : 3f;
            rbfps.movementSettings.SS = rbfps.movementSettings.SS < 3f ? rbfps.movementSettings.SS + Mathf.Lerp(0, 1, Time.deltaTime * 1) : 3f;
            if (!WallRunning && camera.fieldOfView < 94f)
            {
                camera.fieldOfView = camera.fieldOfView < 94f ? camera.fieldOfView + Mathf.Lerp(0, 1, Time.deltaTime * 10) : 94f;
            }
            if (!WallRunning & camera.fieldOfView > 95f){
                camera.fieldOfView = camera.fieldOfView > 94f ? camera.fieldOfView - Mathf.Lerp(0, 1, Time.deltaTime * 1500) : 94f;
            }
        }
        else
        {
            rbfps.movementSettings.FS = rbfps.movementSettings.FS > 1f ? rbfps.movementSettings.FS - Mathf.Lerp(0, 1, Time.deltaTime * 5) : 1f;
            rbfps.movementSettings.BS = rbfps.movementSettings.FS > 1f ? rbfps.movementSettings.BS - Mathf.Lerp(0, 1, Time.deltaTime * 5) : 1f;
            rbfps.movementSettings.SS = rbfps.movementSettings.SS > 1f ? rbfps.movementSettings.SS - Mathf.Lerp(0, 1, Time.deltaTime * 5) : 1f;
            camera.fieldOfView = camera.fieldOfView > 84f ? camera.fieldOfView - Mathf.Lerp(0, 1, Time.deltaTime * 1500) : 84f;
        }


        // Sliding
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sliding = true;
        }
        else if(detectCrouch.Crouchable)
        {
            sliding = false;
        }
        if (sliding && !slidingran)
        {
            playercontroller.localScale = new Vector3(1, 0.2f, 1);
            Transform ground = playercontroller.Find("Detection").Find("DetectGround");
            ground.localScale = new Vector3(0.4f, 1.85f, 0.4f);
            capsuleCollider.radius = 0.3f;
            capsuleCollider.height = 0.3f;
            slidingran = true;
        }
        else if(slidingran && !sliding)
        {
            playercontroller.localScale = new Vector3(1, 1f, 1);
            Transform ground = playercontroller.Find("Detection").Find("DetectGround");
            ground.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            capsuleCollider.radius = 0.6f;
            capsuleCollider.height = 0.6f;
            slidingran = false;
        }

        //Wallrun
        if (DetectWallL.Obstruction && !rbfps.Grounded && !IsParkour && canwallrun && Input.GetKey(KeyCode.A) && !GameObject.ReferenceEquals(DetectWallL.Object, previousobject)) // if detect wall on the left and is not on the ground and not doing parkour(climb/vault)
        {
            WallrunningLeft = true;
            canwallrun = false;
            upforce = WallRunUpForce; //refer to line 186
            previousobject = DetectWallL.Object;
        }

            if (DetectWallR.Obstruction && !rbfps.Grounded && !IsParkour && canwallrun && Input.GetKey(KeyCode.D) && !GameObject.ReferenceEquals(DetectWallR.Object, previousobject)) // if detect wall on thr right and is not on the ground
        {
            WallrunningRight = true;
            canwallrun = false;
            upforce = WallRunUpForce;
            previousobject = DetectWallR.Object;
        }
        if (WallrunningLeft && !DetectWallL.Obstruction || Input.GetAxisRaw("Vertical") <= 0f || rbfps.relativevelocity.magnitude < 1f) // if there is no wall on the left or pressing forward or forward speed < 1 (refer to fpscontroller script)
        {
            WallrunningLeft = false;
            WallrunningRight = false;
        }
        if (WallrunningRight && !DetectWallR.Obstruction || Input.GetAxisRaw("Vertical") <= 0f || rbfps.relativevelocity.magnitude < 1f) // same as above
        {
            WallrunningLeft = false;
            WallrunningRight = false;
        }

        if (WallrunningLeft || WallrunningRight) 
        {
            WallRunning = true;
            rbfps.Wallrunning = true; // this stops the playermovement (refer to fpscontroller script)
        }
        else
        {
            WallRunning = false;
            rbfps.Wallrunning = false;
        }

        if (WallrunningLeft)
        {     
            cameraAnimator.SetBool("WallLeft", true); //Wallrun camera tilt
        }
        else
        {
            cameraAnimator.SetBool("WallLeft", false);
        }
        if (WallrunningRight)
        {           
            cameraAnimator.SetBool("WallRight", true);
        }
        else
        {
            cameraAnimator.SetBool("WallRight", false);
        }

        if (WallRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, upforce, rb.velocity.z); //set the y velocity while wallrunning
            upforce -= WallRunUpForce_DecreaseRate * Time.deltaTime; //so the player will have a curve like wallrun, upforce from line 136
            camera.fieldOfView = camera.fieldOfView < 104f ? camera.fieldOfView + Mathf.Lerp(0, 1, Time.deltaTime * 10) : 104f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = transform.forward * WallJumpForwardVelocity + transform.up * WallJumpUpVelocity ; //walljump
                WallrunningLeft = false;
                WallrunningRight = false;
                canwallrun = true;
            }
            if(rbfps.Grounded)
            {
                WallrunningLeft = false;
                WallrunningRight = false;
                canwallrun = true;
            }
        }
        else{
            canwallrun = true;

        }


    }
  
}
