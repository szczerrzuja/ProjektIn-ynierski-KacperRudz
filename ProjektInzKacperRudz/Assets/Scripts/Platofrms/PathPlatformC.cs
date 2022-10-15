using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlatformC : PathPlatformS
{

    public PathPlatformC(  Vector3[] destinations, float[] movespeed, float acce ) : base(destinations, movespeed, acce)
    {
        
    }
    // Start is called before the first frame update
     public override void stateManager()
    {
        state += 1;
        if (base.state >= direction.Count)
        {
            state = 0 ;
        }
    }
    public override void onStop()
    {
        //destroys object
        rb.isKinematic = true;
        this.enabled = false;
    }
}
