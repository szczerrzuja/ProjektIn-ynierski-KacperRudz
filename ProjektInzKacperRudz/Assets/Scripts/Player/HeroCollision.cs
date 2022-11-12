using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCollision : MonoBehaviour
{
    HeroStatManager HeroStat;
    float invincibilityTimer;
    
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        invincibilityTimer = 0.0f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibilityTimer>0.0f){
            invincibilityTimer -= Time.deltaTime;
        }
    }

    public void setHeroStat(ref HeroStatManager HSM)
    {
        HeroStat = HSM;
    }
    //apply damage 
    public void applyDamage(float damage)
    {
        invincibilityTimer = HeroStat.getInvincibilityTime();
        HeroStat.applyDamage(damage);
    }
    //apply velocity vector
    public void applyKnockback(Vector3 knockback){
        
        rb.AddForce(knockback, ForceMode.VelocityChange);
    }
    public bool canBeHitten()
    {
        return invincibilityTimer<0.05f && HeroStat.getState()>0;
    }
}
