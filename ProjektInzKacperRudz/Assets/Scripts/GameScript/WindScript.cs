using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    
    //frames between changes of wind
    [SerializeField] int MaxtimeBetweenChanges, MinTimeBetweenChanges;
     //max and min value of wind, should be between 0.0 and 1.0
    [SerializeField]float windMin, windMax, 
    randomOffset, currentWind;
    [SerializeField] private int framesToNextChange;
    List<SpikesAnimator> spikes= new List<SpikesAnimator>();
    // Start is called before the first frame update
    void Start()
    {
        framesToNextChange  = 10;
        currentWind = Random.Range(windMin, windMax);
        

    }

    // Update is called once per frame
    void Update()
    {
        changeWind();
    }
    void changeWind(){
        if(framesToNextChange<0)
        {
            framesToNextChange  = Random.Range(MinTimeBetweenChanges, MaxtimeBetweenChanges);
            currentWind = Random.Range(windMin, windMax);
            foreach(SpikesAnimator sp in spikes){
                sp.setWind(currentWind);
            }
        }
        else framesToNextChange -= 1;

        
    }
    public float getCurrentWind(){
        return currentWind;
    }
    public float getMaxOffset(){
        return randomOffset;
    }
    public void addToList(ref SpikesAnimator sp)
    {
        spikes.Add(sp);
    }
}
