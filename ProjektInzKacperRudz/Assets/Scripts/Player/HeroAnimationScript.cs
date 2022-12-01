using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationScript : MonoBehaviour
{
    [SerializeField] HeroController HC;
    [SerializeField] Animator animator;
    Vector2 mvVeloffset;
    //basic MonoBehaviour
    void Start(){
        mvVeloffset =  new Vector2();
    }
    void Update(){ 
        updateAnimationParameters();
    }
    //methods called on animation event
    void ClimbOnAnimationMoveUpwardsStart(){
        HC.GetClimbMoveset().setgoingUp(true);
    }
    void ClimbOnAnimationMoveUpwardsStop(){
        HC.GetClimbMoveset().setgoingUp(false);
    }
    void ClimbOnAnimationJump(){
        HC.GetClimbMoveset().jump();
    }
    void ClimbOnEdgeUp(){
        HC.GetClimbMoveset().onEdgeUp();
    }
    void ClimbOnEdgeForward(){
        HC.GetClimbMoveset().onEdgeForward();
    }
    //method for setting animation parameters
    private void updateAnimationParameters(){
        aChangeState();
        aChangeMVSpeed();
    }
    public void aChangeJumpFlag()
    {
        animator.SetTrigger("Jump");
    }

    private void aChangeState()
    {
        animator.SetInteger("PrevState", animator.GetInteger("State"));
        animator.SetInteger("State", HC.GetHeroStat().getState());
    }
    private void aChangeMVSpeed()
    {
        Vector3 tmp = HC.getVelocityVector();
        animator.SetFloat("MVSpeed", Mathf.Sqrt(tmp.x * tmp.x + tmp.z * tmp.z));
        animator.SetFloat("YVelocity", tmp.y);
        tmp.Normalize();
        Vector2 tmp2 = new Vector2((HC.getLookDirection().normalized.x - tmp.x)*System.Convert.ToSingle(tmp.x>0.01f && tmp.x<-0.01f),
         (HC.getLookDirection().normalized.z-tmp.z -1))*System.Convert.ToSingle(tmp.z>0.01f && tmp.z<-0.01f);
        mvVeloffset.x -= (mvVeloffset.x/5 + tmp2.SqrMagnitude())/2;
        mvVeloffset.y -= (mvVeloffset.y/8 + tmp.y)/4;
        animator.SetFloat("XVelocityOffset", mvVeloffset.x);
        animator.SetFloat("YVelocityOffset", mvVeloffset.y);

    }
}
