using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This platform stops after compleating a cycle*/
public class PathPlatformS : MonoBehaviour
{
    protected short state = 1;
    [SerializeField] public List<Transform> waypoints;
    [SerializeField] public float mvSpeed;
    [SerializeField] public bool workOnTrigger;
    private bool triggered;
    // Start is called before the first frame update
    private void Start() {
        triggered = false;
    }
    // Start is called before the first frame update
    public virtual void Update()
    {

    }
    // Update is called once per fixed frame
    public virtual void FixedUpdate()
    {
        if (state != -1)
        {
            if(triggered||workOnTrigger)
            {
                Vector3 relPosition = waypoints[state].position - transform.position;
                if (relPosition.sqrMagnitude <= 0.002f)
                {
                    stateManager();
                }
    
                
                transform.position = Vector3.MoveTowards(transform.position, waypoints[state].position, mvSpeed*Time.fixedDeltaTime);
            }
        }
        else
            onStop();
    }
    public virtual void stateManager()
    {
        state += 1;
        if (state >= waypoints.Count)
        {
            state = -1;
        }
    }
    public virtual void onStop()
    {
        this.enabled = false;
    }
    public void setTrigger(bool trig)
    {
        triggered = trig;
    }
}
