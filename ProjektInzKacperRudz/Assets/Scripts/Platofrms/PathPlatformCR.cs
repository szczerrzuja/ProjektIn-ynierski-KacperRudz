using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlatformCR : PathPlatformC
{
    private short reversed = 0;
    public PathPlatformCR( Vector3[] destinations, float[] movespeed, float acce) : base(destinations, movespeed, acce)
    {
        
    }
    // Start is called before the first frame update
    public override void stateManager()
    {
        state += System.Convert.ToInt16(-reversed+reversed+1);
        if (base.state >= direction.Count)
        {
            reversed = 1;
            state -=2 ;
        }
        else if(base.state <=0 )
        {
            reversed = 0;
            state+=1;
        }
    }
    public override void onStop()
    {
        //destroys object
    }
}
