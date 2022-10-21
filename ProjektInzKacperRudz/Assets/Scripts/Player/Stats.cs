using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float[] stats_values;
    Stats(){
      stats_values = new float[sizeof(StatisticsCODE)];
      for(int i =0; i<sizeof(StatisticsCODE); i++)
      {
        stats_values[i] = 0;
      }
    }
    void increaseStatsByIndex(int StatCode, int Value){
      stats_values[StatCode] += Value;
    }

}
