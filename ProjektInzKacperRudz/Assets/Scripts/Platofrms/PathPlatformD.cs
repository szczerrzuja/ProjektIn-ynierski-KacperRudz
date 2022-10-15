using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlatformD : PathPlatformS
{
    public PathPlatformD(  Vector3[] destinations, float[] movespeed, float acce ) : base(destinations, movespeed, acce)
    {

    }
    // Start is called before the first frame update

    public override void onStop()
    {
        //destroys object
        rb.isKinematic = true;
        this.enabled = false;
    }
}
