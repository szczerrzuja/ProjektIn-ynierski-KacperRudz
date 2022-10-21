using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class ClimbMoveset : WalkMoveSet
{
    private RaycastHit hit;
    public ClimbMoveset()
    {
        movingPlane = new Vector3(0, 1, 1);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void movementController(float groundMultiplayer, Vector3 inputMoveVector, Quaternion rot)
    {
        HeroStat.jumpsCounter = HeroStat.getMaxJumps();
        rb.velocity = new Vector3(0.0f, HeroStat.getMvSpeed() * input.actions["Vertical"].ReadValue<float>(), 0.0f);
    }
    public override bool onJump(InputAction.CallbackContext context)
    {
        Vector3 tmp = Vector3.zero;
        if (HeroStat.jumpsCounter>0)
        {
            hit = GameObject.Find("Player1").GetComponent<HeroController>().GetHit();
            tmp = new Vector3(hit.normal.x,hit.normal.y+1, hit.normal.z).normalized;
            rb.AddForce(Mathf.Sqrt(-HeroStat.jumpStrength * Physics.gravity.y)*tmp, ForceMode.VelocityChange);
            HeroStat.jumpsCounter -= 1;
            return true;
        }
        return false;
    }
}

