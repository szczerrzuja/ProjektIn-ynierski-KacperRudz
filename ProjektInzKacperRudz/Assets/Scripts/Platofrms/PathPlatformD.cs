using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlatformD : PathPlatformS
{
    // Start is called before the first frame update

    public override void onStop()
    {
        //destroys object
        this.enabled = false;
    }
}
