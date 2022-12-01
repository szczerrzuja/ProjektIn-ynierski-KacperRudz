using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public PathPlatformS platformScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
    if(other.gameObject.tag == "Player")
    {
        other.gameObject.transform.parent = transform;
        platformScript.setTrigger(true);
    }    
    }
    private void OnTriggerExit(Collider other)
    {
    if(other.gameObject.tag == "Player")
    {
        other.gameObject.transform.parent = null;
        platformScript.setTrigger(false);
    }    
    }
}
