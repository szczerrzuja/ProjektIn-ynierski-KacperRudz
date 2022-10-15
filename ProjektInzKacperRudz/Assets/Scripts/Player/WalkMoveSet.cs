using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/*
Move set for walking and sprinting. 
*/
public class WalkMoveSet : MonoBehaviour
{
    protected Vector3  movingPlane = new Vector3(1, 1, 0);
    /*  
      movingPlane - nie wiem jak to określić ale w skrócie określa czy postać może poruszać się lewo prawo, przód tył, góra dół
    lookDirection - saved last input vector > 0
    */

   
    protected Rigidbody rb;
    protected PlayerInput input;
    protected HeroStatManager HeroStat;

    public WalkMoveSet()
    {
        rb = GameObject.Find("Player1").GetComponent<Rigidbody>();

        input = GameObject.Find("Player1").GetComponent<PlayerInput>();
        HeroStat = GameObject.Find("Player1").GetComponent<HeroController>().GetHeroStat();
    }
    public virtual void movementController(float groundMultiplayer,  Vector3 inputMoveVector, Quaternion rot)
    {
        Vector3 tmp;
        float speed =HeroStat.getMvSpeed();
        tmp = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z), speed * groundMultiplayer);
        //clamp moving speed
        if (inputMoveVector.sqrMagnitude <= 0.001f)
        {
            if (tmp.sqrMagnitude >= 0.01)
            {
                rb.AddForce(-tmp.normalized * HeroStat.acceleration * 0.8f, ForceMode.Acceleration);
            }
        }
        else
        {
            //tmp = this.transform.rotation * Vector3.Scale(inputMoveVector, movingPlane).normalized * speed *groundMultiplayer;

            rb.velocity = new Vector3(tmp.x, rb.velocity.y, tmp.z);

            tmp = rot * Vector3.Scale(inputMoveVector, movingPlane).normalized * HeroStat.acceleration
            * (((System.Convert.ToInt32(HeroStat.isGrounded) + 1) % 2 * HeroStat.AirControll + (System.Convert.ToInt32(HeroStat.isGrounded))));
            rb.AddForce(tmp, ForceMode.Acceleration);

        }
        //clamp fallling speed
        if (rb.velocity.y * rb.velocity.y > 10000.0f)
        {
            tmp = Vector3.zero;
            tmp.y = rb.velocity.y;
            tmp = Vector3.ClampMagnitude(tmp, 100);
            tmp.x = rb.velocity.x;
            tmp.z = rb.velocity.z;
            rb.velocity = tmp;
        }
    }
    public virtual bool onJump(InputAction.CallbackContext context)
    {
        Vector3 tmp = Vector3.zero;
        if ((HeroStat.jumpsCounter > 0 || HeroStat.isGrounded))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(0.0f, Mathf.Sqrt(-HeroStat.jumpStrength * Physics.gravity.y), 0.0f, ForceMode.VelocityChange);
            HeroStat.jumpsCounter -= 1;
            return true;
        }
        return false;
    
    }

}
