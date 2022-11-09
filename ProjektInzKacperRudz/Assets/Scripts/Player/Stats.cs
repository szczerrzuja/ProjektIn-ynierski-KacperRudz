using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stats
{
  public enum StatisticsCODE
    {
        Max_HP,
        HP_regen,
        Max_SP,
        SP_regen,
        maxJump,        
    };
    [SerializeField] public float[] stats_values;
    public Stats(){
      //sizeof enum return bigest number in enum not actual size, highest number of enum is 1 less than size 
      stats_values = new float[sizeof(StatisticsCODE)+1];
      for(int i =0; i<sizeof(StatisticsCODE); i++)
      {
        stats_values[i] = 0;
      }
    }
   
    void increaseStatsByIndex(int StatCode, int Value){
      stats_values[StatCode] += Value;
    }

}
