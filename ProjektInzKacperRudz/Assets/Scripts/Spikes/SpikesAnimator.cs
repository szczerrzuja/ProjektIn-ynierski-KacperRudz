using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesAnimator : MonoBehaviour
{
    Animator animator;
    [SerializeField]float maxWind, minWind;
    bool triggerTouched;
    int touchCD, windchangeCD;
    SpikesAnimator gateway;
    [SerializeField]float currentWind;
    void Start()
    {
        gateway = this;
        animator = GetComponent<Animator>();
        animator.Play("Animation|SlowFloating", 0,Random.Range(0f,1f));
        animator.SetFloat("WindMultiplayer", Mathf.Min(Mathf.Max(0.5f, minWind), maxWind));
        GetComponentInParent<WindScript>().addToList(ref this.gateway);
    }

    // Update is called once per frame
    void Update()
    {
        if(touchCD>0)   
        {
            animator.SetBool("Touched", false);
            touchCD-=1;
        }
        if(triggerTouched && touchCD<=0)
        {
            animator.SetBool("Touched", true);
            triggerTouched = false;
            touchCD = 5;
        }
        
    }
    public void iWasTouched(){

            triggerTouched = true;
    }
    public void setWind(float wind){
        currentWind = Mathf.Min(Mathf.Max(wind, minWind), maxWind);
         animator.SetFloat("WindMultiplayer", Mathf.Min(Mathf.Max(wind, minWind), maxWind));
    }
}
