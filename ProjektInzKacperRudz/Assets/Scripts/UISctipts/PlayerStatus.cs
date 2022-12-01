using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStatus : MonoBehaviour
{
    public Slider HPSlide;
    public Slider SPSlide;
    float maxHP, maxSP;

    // Start is called before the first frame update
    void Start()
    {

        HPSlide.wholeNumbers = false;
        HPSlide.maxValue = maxHP;
        HPSlide.value = maxHP;

        SPSlide.wholeNumbers = false;
        SPSlide.maxValue = maxSP;
        SPSlide.value = maxSP;
        
    }
    public void Initiate(float maxHealth, float maxStamina)
    {

        maxHP = maxHealth;
        HPSlide.maxValue = maxHP;

        maxSP = maxStamina;
        SPSlide.maxValue = maxSP;
    }
    public void SetCurrentLevels(float curHealth, float curStamina)
    {

        HPSlide.value = curHealth;

        SPSlide.value = curStamina;


    }
    // Update is called once per frame
    void Update()
    {

    }
}
