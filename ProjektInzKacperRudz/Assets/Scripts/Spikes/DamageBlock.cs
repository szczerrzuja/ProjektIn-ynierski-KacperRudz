using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    [SerializeField] float knockback, damage;
    private int attackCooldown;
    const int attackCooldownTime = 2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(attackCooldown>0)
        {
            attackCooldown -= 1; 
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && attackCooldown<=0)
        {
            attackCooldown = attackCooldownTime;
            HeroCollision HCC = collision.gameObject.GetComponent<HeroCollision>();
            if(HCC.canBeHitten())
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                HCC.applyDamage(damage);
              
                HCC.applyKnockback(-collision.contacts[0].normal*knockback);
            }
        }
       

    }


}