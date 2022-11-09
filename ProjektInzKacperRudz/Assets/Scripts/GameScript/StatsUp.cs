using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUp : MonoBehaviour
{
    private bool used;
    [SerializeField] public Stats stats = new Stats();
    // Start is called before the first frame update
    void Start()
    {
        used = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player") && !used)
        {
            used =true;
            other.gameObject.GetComponent<HeroController>().upgradeStats(ref stats);
        }

    }
}
