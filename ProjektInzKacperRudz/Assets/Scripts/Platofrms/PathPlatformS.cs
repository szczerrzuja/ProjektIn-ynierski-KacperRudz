using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This platform stops after compleating a cycle*/
public class PathPlatformS : MonoBehaviour
{
    protected short state = 1;
    public Rigidbody rb;
    public List<Vector3> direction;
    public List<float> mvSpeeds;
    [SerializeField]
    public float acceleration;
    // Start is called before the first frame update
    public PathPlatformS(Vector3[] destinations, float[] movespeed, float acce)
    {
        rb = GetComponent<Rigidbody>();
        direction = new List<Vector3>();
        mvSpeeds = new List<float>();
        acceleration = acce;
        direction.Add(rb.position);
        if (destinations.GetLength(1) > 0)
        {
            for (int i = 0; i < destinations.GetLength(1); i++)
            {
                direction.Add(destinations[i]);
            }
        }
        if (movespeed.GetLength(1) > 0)
        {
            mvSpeeds.Add(movespeed[0]);
            for (int i = 0; i < movespeed.GetLength(1); i++)
            {
                mvSpeeds.Add(movespeed[i]);
            }
        }
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
            Vector3 relPosition = direction[state] - rb.position;
            if (relPosition.sqrMagnitude <= 0.002f)
            {
                rb.position = direction[state];
                rb.velocity = Vector3.zero;
                stateManager();
            }
            else
            {
                if (rb.velocity.sqrMagnitude <= mvSpeeds[state] * mvSpeeds[state])
                    rb.AddForce(relPosition.normalized * acceleration * Time.deltaTime, ForceMode.Acceleration);
                else
                {
                    Vector3 tmp = Vector3.ClampMagnitude(rb.velocity, mvSpeeds[state]);
                    rb.velocity = tmp;
                }

            }
        }
        else
            onStop();
    }
    public virtual void stateManager()
    {
        state += 1;
        if (state >= direction.Count)
        {
            state = -1;
        }
    }
    public virtual void onStop()
    {
        Debug.Log("Here i am");
        rb.isKinematic = true;
        this.enabled = false;
    }
}
