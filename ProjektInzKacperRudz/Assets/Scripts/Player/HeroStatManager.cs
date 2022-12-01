
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
This class is for stat related things like HP, MP, SP, running out of this things 
*/
public class HeroStatManager
{
    public enum playerMoveStateTAB
    {
        Dead,
        Walk,
        Sprint,
        Fall,
        Climb
    };
    private  int playerMoveState = 0;
    //curent levels of HP
    public float Health_Points, Stamina_Points, 
    jumpStrength, acceleration, AirControll, staminaDCD;
    
    static float  staminaDepleationCD = 1.0f;
    public int jumpsCounter;
    public float invincibilityTime;
    public List<float> mvSpeeds;
    [HideInInspector] private Stats Base_stats, UPD_Stats;
    
    public bool isGrounded = false, isSprinting = false, isClimbing = false, isdead = false;
    // Start is called before the first frame update
    public HeroStatManager()
    {
        Base_stats = new Stats();
        UPD_Stats = new Stats();
        InitBaseStats();
        AirControll = 0.75f;
        Health_Points = 2.0f;
        Stamina_Points = 2.0f;
        mvSpeeds = new List<float>();
        mvSpeeds.Add(0.0f);
        mvSpeeds.Add(4.0f);
        mvSpeeds.Add(6.0f);
        mvSpeeds.Add(4.0f);
        mvSpeeds.Add(4.0f);
        acceleration = 16.0f;
        jumpsCounter = (int)Base_stats.stats_values[(int)Stats.StatisticsCODE.maxJump]+(int)UPD_Stats.stats_values[(int)Stats.StatisticsCODE.maxJump];
        jumpStrength = mvSpeeds[1]*1.5f;
        staminaDCD = staminaDepleationCD;
        invincibilityTime = 0.3f;

    }
    

    // Update is called once per frame
    public bool Update(bool isMoving)
    {
        if(!isdead){
        
        StaminaManage(isMoving);
        changeState(isMoving);
        return HealthManage();
        }
        return false;
        
    }
    void StaminaManage (bool isMoving)
    {
        float StaminaReg = Base_stats.stats_values[(int)Stats.StatisticsCODE.SP_regen]+UPD_Stats.stats_values[(int)Stats.StatisticsCODE.SP_regen];
        if (isMoving && ((playerMoveState==(int)playerMoveStateTAB.Sprint && isGrounded) || playerMoveState==(int)playerMoveStateTAB.Climb ))
        {
            Stamina_Points -= (StaminaReg)*0.2f * Time.deltaTime;         
            
        }
        else
            Stamina_Points += System.Convert.ToSingle(isGrounded) * StaminaReg * Time.deltaTime;
        if (Stamina_Points <= 0.0f)
            {
                Stamina_Points = 0;
                isSprinting = false;
                isClimbing = false;
                staminaDCD = staminaDepleationCD;
                
            }
        if (Stamina_Points >  Base_stats.stats_values[(int)Stats.StatisticsCODE.Max_SP] + UPD_Stats.stats_values[(int)Stats.StatisticsCODE.Max_SP])
        {
            Stamina_Points = Base_stats.stats_values[(int)Stats.StatisticsCODE.Max_SP]+ UPD_Stats.stats_values[(int)Stats.StatisticsCODE.Max_SP];
        }
        if(staminaDCD<=0.0f)
        {
            staminaDCD = 0.0f;
        }
        else 
            staminaDCD-=Time.deltaTime;
    }
    private bool HealthManage()
    {
        if(Health_Points<0.0f)
        {
            isdead = true;
            playerMoveState = (int)playerMoveStateTAB.Dead;
            changeState(false);
            return true;
        }
        Health_Points += (Base_stats.stats_values[(int)Stats.StatisticsCODE.HP_regen]+UPD_Stats.stats_values[(int)Stats.StatisticsCODE.HP_regen]) * Time.deltaTime;
        if (Health_Points > Base_stats.stats_values[(int)Stats.StatisticsCODE.Max_HP] + UPD_Stats.stats_values[(int)Stats.StatisticsCODE.Max_HP])
        {
            Health_Points = Base_stats.stats_values[(int)Stats.StatisticsCODE.Max_HP]+ UPD_Stats.stats_values[(int)Stats.StatisticsCODE.Max_HP];
        }
        return false;
    }
    public void Heal(float heal)
    {
        Health_Points += heal;
    }
    public void StaminaHeal(float sHeal)
    {
        Stamina_Points +=sHeal;
    }

    public void onTick()
    {
        
    }
    public void applyDamage(float damage){
        Health_Points -= damage;    
    }
    public void changeState(bool isMoving)
    {
        if(playerMoveState==(int)playerMoveStateTAB.Dead)
        {
        }
        else if(isClimbing)
        {
            playerMoveState = (int)playerMoveStateTAB.Climb;
        }
        else if(!isGrounded)
        {
            playerMoveState = (int)playerMoveStateTAB.Fall;
        }
        else if(isSprinting  && !isClimbing && staminaDCD<=0.0f)
        {
            playerMoveState = (int)playerMoveStateTAB.Sprint;
        }
        else if( isMoving && (!isSprinting || staminaDCD>0.0f))
        {
            isSprinting = false;
            playerMoveState = (int)playerMoveStateTAB.Walk;
        }
        
        
    }
    public void resurect()
    {
        playerMoveState = (int)playerMoveStateTAB.Walk;
        isClimbing = false;
        isSprinting = false;
        isdead = false;
        isGrounded =false;
        Health_Points = Base_stats.stats_values[(int)Stats.StatisticsCODE.Max_HP]+UPD_Stats.stats_values[(int)Stats.StatisticsCODE.Max_HP];
    }
    public void upgradeStats(ref Stats Upgrade){
        for(int i=0; i<sizeof(Stats.StatisticsCODE)+1; i++)
        {
            UPD_Stats.stats_values[i] += Upgrade.stats_values[i];
        }
        
    }
    public int  getState()
    {
        return playerMoveState;
    }
    public float getMvSpeed()
    {
        return mvSpeeds[playerMoveState];
    }
    public float getInvincibilityTime()
    {
        return invincibilityTime;
    }
    public int getMaxJumps(){
        return (int)Base_stats.stats_values[(int)Stats.StatisticsCODE.maxJump]+(int)UPD_Stats.stats_values[(int)Stats.StatisticsCODE.maxJump];
    }
    public float getMaxHP(){
        return Base_stats.stats_values[(int)Stats.StatisticsCODE.Max_HP]+UPD_Stats.stats_values[(int)Stats.StatisticsCODE.Max_HP];
    }
    public float getMaxSP(){
        return Base_stats.stats_values[(int)Stats.StatisticsCODE.Max_SP]+UPD_Stats.stats_values[(int)Stats.StatisticsCODE.Max_SP];
    }
    private void InitBaseStats(){
        Base_stats.stats_values[(int)Stats.StatisticsCODE.Max_HP] = 20.0f;
        Base_stats.stats_values[(int)Stats.StatisticsCODE.Max_SP] = 20.0f;
        Base_stats.stats_values[(int)Stats.StatisticsCODE.HP_regen] = 1.0f;
        Base_stats.stats_values[(int)Stats.StatisticsCODE.SP_regen] = 5.0f;
        Base_stats.stats_values[(int)Stats.StatisticsCODE.maxJump] = 2.0f;
    }
    
}
