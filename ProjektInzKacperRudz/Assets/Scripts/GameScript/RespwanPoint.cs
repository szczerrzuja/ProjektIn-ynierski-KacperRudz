using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RespwanPoint : MonoBehaviour
{
    [SerializeField] private int RespawnPointID;
    [SerializeField] private RespawnController respContr;
    private GameObject gateway;
    // Start is called before the first frame update
    void Start()
    {
        gateway =  transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player"))
        {
            respContr.setSpawnPoint(ref gateway);
        }

    }
}
