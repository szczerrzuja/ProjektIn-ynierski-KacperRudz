using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlatformC : PathPlatformS
{


    // Start is called before the first frame update
     public override void stateManager()
    {
        state += 1;
        if (base.state >= waypoints.Count)
        {
            state = 0 ;
        }
    }
    public override void onStop()
    {

        this.enabled = false;
    }
}
