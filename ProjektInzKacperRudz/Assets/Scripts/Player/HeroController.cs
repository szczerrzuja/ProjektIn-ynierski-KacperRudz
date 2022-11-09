using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
This class is for managing movement, collisions and keyinput.
FixedUpdate is mainly used for movement and colision related calculations
*/
public class HeroController : MonoBehaviour
{

    //private
    [SerializeField] private RespawnController RC;
    private Rigidbody rb;
    private CapsuleCollider collide;
    [SerializeField] private LayerMask hitableMasks;
    private PlayerInput input;
    private RaycastHit hit;
    private HeroStatManager HeroStat;
    private List<WalkMoveSet> moveSet;

    private Animator animator;
    private PlayerStatus playerStatus;
    private bool canMove;
    private float groundMultiplayer = 0.0f;
    private Transform BodyTransform;
    //public
    private Vector3
    /*  
    movingPlane - określa oś (x,y,z) w jakiej postać może sie poruszać 
    */
    movingPlane = new Vector3(1, 1, 0),
    lookDirection = new Vector3(1, 0, 1),
    lastInputVector = new Vector3(1,0,1),
    //vector of combined inputs (ad, jump, ws)
    inputMoveVector = new Vector3(0, 0, 0);

    float capsWidth;
    private int moveSetState;

    // Start is called before the first frame update
    void Start()
    {
        moveSetState = 0;
        rb = GetComponent<Rigidbody>();
        collide = GetComponent<CapsuleCollider>();
        input = GetComponent<PlayerInput>();
        HeroStat = new HeroStatManager();
        BodyTransform = this.transform.Find("Body").transform;
        capsWidth = collide.radius;
        rb.velocity = Vector3.zero;
        moveSet = new List<WalkMoveSet>();
        moveSet.Add(new WalkMoveSet()); 
        moveSet.Add(moveSet[0]); 
        moveSet.Add(moveSet[0]);
        moveSet.Add(new ClimbMoveset(hitableMasks, capsWidth));
        rb.mass = 50.0f;
  
        canMove = true;
        HeroStat.resurect();
        //animations
        animator = this.transform.Find("Body").GetComponent<Animator>();

        //gui
        playerStatus = GameObject.Find("Gui").GetComponent<PlayerStatus>();
        playerStatus.Initiate(HeroStat.getMaxHP(), HeroStat.getMaxSP());

        GetComponent<HeroCollision>().setHeroStat(ref HeroStat);

        
    }

    // Update is called once per frame - better use for graphical effects
    void Update()
    {
        HeroTerrainColidingHandler();
        moveSetState = HeroStat.getState() - 1;
        
        if(HeroStat.Update(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y > 0.1f))
        {
            animator.SetTrigger("Dead");
        }
        updateAnimationParameters();
        playerStatus.SetCurrentLevels(HeroStat.Health_Points, HeroStat.Stamina_Points);

    }   
    private void updateAnimationParameters(){
        aChangeXZSpeed();
        aChangeYSpeed();
        aChangeState();
        aChangeMVSpeed();
    }

    public void respawn(ref GameObject RespawnPoint){
        this.transform.position = RespawnPoint.transform.position;
        HeroStat.resurect();
        animator.SetTrigger("Resurect");
        canMove = true;

    }


    //Fixed update is called once in constant amount of time - better use for phisic related things
    private void FixedUpdate()
    {
        movementController();
        //Debug.Log(lookDirection);

    }


    //Controller of player forces, , Velocity
    private void movementController()
    {
        if ((int)HeroStat.getState() > 0)
        {
            if(moveSetState!= (int)HeroStatManager.playerMoveStateTAB.Climb-1)
            {
                canMove = true;
                if (inputMoveVector.sqrMagnitude > 0)
                {
                    lastInputVector = this.transform.rotation * Vector3.Scale(inputMoveVector, movingPlane);

                }         
                Quaternion tmp = Quaternion.FromToRotation(lastInputVector, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(0, 90, 0);
                BodyTransform.rotation = Quaternion.RotateTowards(BodyTransform.rotation, tmp,10.0f);
                lookDirection = BodyTransform.rotation*(new Vector3(0.0f, 0.0f, 1.0f));
                lookDirection.Normalize();
            }
            moveSet[moveSetState].movementController(groundMultiplayer, inputMoveVector, this.transform.rotation);
        }
        else if(canMove) {
            canMove = false;
            StartCoroutine(RC.respwanOnDelay());
        }

    }


    //method for detecting ground and other colision related things
    private void HeroTerrainColidingHandler()
    {
        //if collider colides with ground
        if (Physics.Raycast(rb.position+collide.center, Vector3.down, out hit, collide.height * 0.55f, hitableMasks) && hit.normal.y > 0.60f)
        {

            groundMultiplayer = hit.normal.y;
            HeroStat.jumpsCounter = HeroStat.getMaxJumps() - 1;
            HeroStat.isGrounded = true;
        }
        else
        {
            HeroStat.isGrounded = false;
        }
        //if is climbing
        //needs changes
        if (moveSetState == (int)HeroStatManager.playerMoveStateTAB.Climb)
        {
            if (!((ClimbMoveset)moveSet[(int)HeroStatManager.playerMoveStateTAB.Climb-1]).canClimb(lookDirection, hitableMasks))
            {
                HeroStat.isClimbing = false;
            }
        }

    }

    //methods called when certain key is pressed
    public void OnJump(InputAction.CallbackContext context)
    {
        if (canMove)
        {

            if (context.started)
            {
                if (moveSet[moveSetState].onJump(context))
                    aChangeJumpFlag();

            }

        }


    }


    public void onMoveHorizontal(InputAction.CallbackContext context)
    {

        inputMoveVector.x = context.ReadValue<float>();
    }


    public void onMoveVertical(InputAction.CallbackContext context)
    {

        inputMoveVector.z = context.ReadValue<float>();
    }


    public void onSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            HeroStat.isSprinting = true;

        }
        if (context.canceled)
        {
            HeroStat.isSprinting = false;
        }
    }


    public void onWallSnap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (moveSetState == (int)HeroStatManager.playerMoveStateTAB.Climb - 1)
            {
                HeroStat.isClimbing = false;
                HeroStat.changeState(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y > 0.1f);
            }
            else if(((ClimbMoveset)moveSet[(int)HeroStatManager.playerMoveStateTAB.Climb-1]).canClimb(lookDirection, hitableMasks))
            {
                HeroStat.isClimbing = true;
                HeroStat.changeState(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y > 0.1f);
            }
            Debug.Log(((ClimbMoveset)moveSet[(int)HeroStatManager.playerMoveStateTAB.Climb-1]).canClimb(lookDirection, hitableMasks));
        }

    }

    public HeroStatManager GetHeroStat()
    {
        return HeroStat;
    }
    public RaycastHit GetHit()
    {
        return hit;
    }
    public void upgradeStats(ref Stats Upgrade){
        HeroStat.upgradeStats(ref Upgrade);
    }

    private void aChangeJumpFlag()
    {
        animator.SetTrigger("Jump");
    }
    private void aChangeXZSpeed()
    {
        animator.SetFloat("XZSpeed", rb.velocity.x + rb.velocity.z);
    }
    private void aChangeYSpeed()
    {
        animator.SetFloat("YSpeed", rb.velocity.y);
    }
    private void aChangeState()
    {
        animator.SetInteger("PrevState", animator.GetInteger("State"));
        animator.SetInteger("State", HeroStat.getState());
    }
    private void aChangeMVSpeed()
    {

        animator.SetFloat("MVSpeed", (rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z) / 36);
    }

}

