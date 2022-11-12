using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class ClimbMoveset : WalkMoveSet
{
    private RaycastHit hit;
    private float capsWidth, capsheight;
    private Vector3 lookDirection;
    private bool isJumping, goingUp, iWantClimb, wantEdgeJump, edgeClimb;
    Animator anim;
    
    public ClimbMoveset(int hitmask, float capwidth, float capHeig)
    {
        movingPlane = new Vector3(0, 1, 1);
        capsWidth = capwidth;
        capsheight = capHeig;


    }
    // Start is called before the first frame update
    void Start()
    {
        isJumping = false;
        iWantClimb = false;
    }
    public void jump()
    {
        rb.constraints = (rb.constraints | RigidbodyConstraints.FreezePositionY)^RigidbodyConstraints.FreezePositionY;
        Vector3 tmp = Vector3.zero;
        tmp = new Vector3(hit.normal.x, hit.normal.y + 1, hit.normal.z).normalized;
        Debug.Log(tmp);
        rb.AddForce(Mathf.Sqrt(-HeroStat.jumpStrength * Physics.gravity.y) * tmp, ForceMode.VelocityChange);
        HeroStat.jumpsCounter -= 1;
        HeroStat.isClimbing = false;
        isJumping = false;
        HeroStat.changeState(true);
        ExitState();
    }
    public bool canClimb(Vector3 lookDir, int hitableMasks)
    {
        if (!isJumping )
        {
            lookDirection = lookDir;
        }
        else {
            return true;
        }
        iWantClimb = (Physics.Raycast(rb.position + new Vector3(0.0f, capsheight*0.8f, 0.0f), lookDirection, out hit, capsWidth * 1.1f, hitableMasks));
        wantEdgeJump = !iWantClimb && (Physics.Raycast(rb.position, lookDirection, out hit, capsWidth * 1.1f, hitableMasks) ||
        (Physics.Raycast(rb.position + new Vector3(0.0f, -capsheight*0.8f, 0.0f), lookDirection, out hit, capsWidth * 1.1f, hitableMasks)));
        if(wantEdgeJump && !edgeClimb && input.actions["Vertical"].ReadValue<float>()>0)
        {
            edgeClimb = true;  
            anim.Play("Climb.ClimbOnEdge",anim.GetLayerIndex("Base Layer"));
            rb.constraints = (rb.constraints | RigidbodyConstraints.FreezePositionY)^RigidbodyConstraints.FreezePositionY;
        }
        return iWantClimb || wantEdgeJump;
    }
    public override void movementController(float groundMultiplayer, Vector3 inputMoveVector, Quaternion rot)
    {
        bool climbUp = false;
        //if climbing upwards
        if(input.actions["Vertical"].ReadValue<float>() > 0 && !isJumping && !edgeClimb)
        {
            //make sure that rb unfreezes
            rb.constraints = (rb.constraints | RigidbodyConstraints.FreezePositionY)^RigidbodyConstraints.FreezePositionY;
            HeroStat.jumpsCounter = HeroStat.getMaxJumps();
            rb.velocity = new Vector3(0.0f, HeroStat.getMvSpeed() * System.Convert.ToSingle(goingUp), 0.0f);
            climbUp = true;
        }
        //if climbing upwards
        else if(input.actions["Vertical"].ReadValue<float>() < 0 && !isJumping)
        {
            //make sure that rb unfreezes
            rb.constraints = (rb.constraints | RigidbodyConstraints.FreezePositionY)^RigidbodyConstraints.FreezePositionY;
            rb.velocity = new Vector3(0.0f, HeroStat.getMvSpeed() * input.actions["Vertical"].ReadValue<float>(), 0.0f);
        }
        //if staing in position
        else if(!isJumping && iWantClimb)
        {
            //make sure that rb freezes
            rb.constraints = (rb.constraints | RigidbodyConstraints.FreezePositionY);
        }
        anim.SetBool("ClimbUp", climbUp);
        

    }
    public override bool onJump(InputAction.CallbackContext context)
    {
        if (HeroStat.jumpsCounter > 0)
        {
            anim.Play("Climb.ClimbJump", anim.GetLayerIndex("Base Layer"));
            lookDirection = -lookDirection;
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
            isJumping = true;
            iWantClimb = false;
        }
        return isJumping;
    }
    
    public void setAnimator(ref Animator animat)
    {
        anim = animat;
    }
    public void setgoingUp(bool goup)
    {
        goingUp = goup;
    }
    public void onEdgeUp(){
        edgeClimb = false;
        rb.AddForce(Vector3.up*5.0f, ForceMode.VelocityChange);
    }
    public void onEdgeForward(){
        rb.AddForce(lookDirection.normalized*2.0f, ForceMode.VelocityChange);
        edgeClimb = false;
        ExitState();

    }
    public void ExitState(){
        rb.constraints = (rb.constraints | RigidbodyConstraints.FreezePositionY)^RigidbodyConstraints.FreezePositionY;
        anim.SetBool("ClimbUp", false);
        edgeClimb = false;

    }
    public void EnterState(){

    }

}

