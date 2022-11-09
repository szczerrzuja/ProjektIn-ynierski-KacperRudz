using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class ClimbMoveset : WalkMoveSet
{
    private RaycastHit hit;
    private float capsWidth;
    private Vector3 lookDirection;
    private bool isJumping;
    float timeToJump;
    public ClimbMoveset(int hitmask, float capwidth)
    {
        movingPlane = new Vector3(0, 1, 1);
        capsWidth = capwidth;
        isJumping = false;
        timeToJump = 0.0f;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void jump()
    {
        Vector3 tmp = Vector3.zero;
        tmp = new Vector3(hit.normal.x, hit.normal.y + 1, hit.normal.z).normalized;
        Debug.Log(tmp);
        rb.AddForce(Mathf.Sqrt(-HeroStat.jumpStrength * Physics.gravity.y) * tmp, ForceMode.VelocityChange);
        HeroStat.jumpsCounter -= 1;
        isJumping = false;
        HeroStat.isClimbing = false;
        HeroStat.changeState(true);
        rb.constraints = rb.constraints ^ RigidbodyConstraints.FreezePositionY;
        Debug.Log(HeroStat.getState());
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
        return ((Physics.Raycast(rb.position, lookDirection, out hit, capsWidth * 1.2f, hitableMasks) ||
                Physics.Raycast(rb.position + new Vector3(0.0f, 0.7f, 0.0f), lookDirection, out hit, capsWidth * 1.2f, hitableMasks) ||
                Physics.Raycast(rb.position + new Vector3(0.0f, -0.7f, 0.0f), lookDirection, out hit, capsWidth * 1.2f, hitableMasks)));
    }
    public override void movementController(float groundMultiplayer, Vector3 inputMoveVector, Quaternion rot)
    {
        HeroStat.jumpsCounter = HeroStat.getMaxJumps();
        rb.velocity = new Vector3(0.0f, HeroStat.getMvSpeed() * input.actions["Vertical"].ReadValue<float>(), 0.0f);
        if(timeToJump > 0.0f )
        {
            timeToJump -= Time.deltaTime;
        }
        else if (timeToJump<0.01f && isJumping)
        {
           jump();
        }      
        

    }
    public override bool onJump(InputAction.CallbackContext context)
    {
        if (HeroStat.jumpsCounter > 0 && !isJumping)
        {
            timeToJump = 1.0f;
            isJumping = true;
            lookDirection = -lookDirection;
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
        }


        return isJumping;
    }
    

}

